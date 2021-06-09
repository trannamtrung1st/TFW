using DeviceId;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;

namespace TFW.IdService
{
    partial class IdServiceWorker : ServiceBase
    {
        public const string ConfigFile = "config";
        public const string LocalhostIpAddr = "127.0.0.1";
        public const int DefaultPort = 7777;

        private TcpListener _server;
        private BackgroundWorker _worker;
        private bool _stop;

        public IdServiceWorker()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _worker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };
            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerAsync(args);
            base.OnStart(args);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _stop = false;
            string[] args = e.Argument as string[];
            var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFile);
            string[] configs = File.ReadAllText(configFile).Split('\n').Select(item => item.Trim()).ToArray();

            string deviceId = new DeviceIdBuilder()
                .AddMachineName()
                .AddProcessorId()
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber()
                .ToString();
            byte[] deviceIdData = EncodeOutgoingMessage(deviceId);

            string portArgs = configs.FirstOrDefault(x => x.StartsWith("port="))?.Split('=')[1];
            int port = string.IsNullOrWhiteSpace(portArgs) ? DefaultPort : int.Parse(portArgs);
            _server = new TcpListener(IPAddress.Parse(LocalhostIpAddr), port);
            _server.Start();

            while (!e.Cancel)
            {
                TcpClient client = _server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                bool sendResp = false;

                while (!sendResp)
                {
                    while (!stream.DataAvailable && !e.Cancel) ;
                    while (client.Available < 3 && !e.Cancel) ;

                    if (e.Cancel) return;

                    byte[] bytes = new byte[client.Available];
                    stream.Read(bytes, 0, client.Available);
                    string s = Encoding.UTF8.GetString(bytes);

                    if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase))
                    {
                        string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                        string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                        byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
                        string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

                        byte[] response = Encoding.UTF8.GetBytes(
                            "HTTP/1.1 101 Switching Protocols\r\n" +
                            "Connection: Upgrade\r\n" +
                            "Upgrade: websocket\r\n" +
                            "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

                        stream.Write(response, 0, response.Length);
                    }
                    else
                    {
                        stream.Write(deviceIdData, 0, deviceIdData.Length);
                        client.Close();
                        sendResp = true;
                    }
                }
            }

            _stop = true;
        }

        protected override void OnStop()
        {
            StopWork();
            base.OnStop();
        }

        protected override void OnContinue()
        {
            _server?.Start();
            base.OnContinue();
        }

        protected override void OnPause()
        {
            _server?.Stop();
            base.OnPause();
        }

        protected override void OnShutdown()
        {
            StopWork();
            base.OnShutdown();
        }

        private void StopWork()
        {
            _server?.Stop();
            _worker.CancelAsync();
            _worker.Dispose();
        }

        private byte[] EncodeOutgoingMessage(string text, bool masked = false)
        {
            byte[] header = new byte[] { 0x81, (byte)((masked ? 0x1 << 7 : 0x0) + text.Length) };
            byte[] maskKey = new byte[4];
            if (masked)
            {
                Random rd = new Random();
                rd.NextBytes(maskKey);
            }
            byte[] payload = Encoding.UTF8.GetBytes(text);
            byte[] frame = new byte[header.Length + (masked ? maskKey.Length : 0) + payload.Length];
            Array.Copy(header, frame, header.Length);
            if (masked && maskKey.Length > 0)
            {
                Array.Copy(maskKey, 0, frame, header.Length, maskKey.Length);
                for (int i = 0; i < payload.Length; i++)
                {
                    payload[i] = (byte)(payload[i] ^ maskKey[i % maskKey.Length]);
                }
            }
            Array.Copy(payload, 0, frame, header.Length + (masked ? maskKey.Length : 0), payload.Length);
            return frame;
        }
    }
}
