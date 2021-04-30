using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using DeviceId;

namespace TFW.IdService
{
    class Server
    {
        public static void Main(string[] args)
        {
            string deviceId = new DeviceIdBuilder()
                .AddMachineName()
                .AddProcessorId()
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber()
                .ToString();

            string ip = "127.0.0.1";
            string portArgs = args.FirstOrDefault(x => x.StartsWith("port="))?.Split('=')[1];
            int port = string.IsNullOrWhiteSpace(portArgs) ? 7777 : int.Parse(portArgs);
            var server = new TcpListener(IPAddress.Parse(ip), port);

            server.Start();
            Console.WriteLine("Server has started on {0}:{1}, Waiting for a connection...", ip, port);
            System.Threading.CancellationToken token = default;

            while (!token.IsCancellationRequested)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("A client connected.");

                NetworkStream stream = client.GetStream();

                // enter to an infinite cycle to be able to handle every change in stream
                while (true)
                {
                    while (!stream.DataAvailable) ;
                    while (client.Available < 3) ; // match against "get"

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
                        byte[] resp = EncodeOutgoingMessage(deviceId);
                        stream.Write(resp, 0, resp.Length);
                        client.Close();
                        Console.WriteLine("Disconnected.");
                        break;
                    }
                }
            }
        }

        private static byte[] EncodeOutgoingMessage(string text, bool masked = false)
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
