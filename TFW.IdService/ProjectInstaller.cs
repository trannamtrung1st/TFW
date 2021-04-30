using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;

namespace TFW.IdService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public static class Properties
        {
            public const string Port = "PORT";
        }

        public static class Parameters
        {
            public const string AssemblyPath = "assemblypath";
        }

        public const string ConfigFile = "config";

        private string _installationFolder;
        private Dictionary<string, object> _props;

        public ProjectInstaller()
        {
            _props = new Dictionary<string, object>();
            InitializeComponent();

            this.BeforeInstall += IdServiceInstaller_BeforeInstall;
            this.AfterInstall += IdServiceInstaller_AfterInstall;
        }

        private void IdServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            File.WriteAllText(Path.Combine(_installationFolder, ConfigFile),
                $"port={_props[Properties.Port]}");

            ServiceController service = new ServiceController(nameof(IdService));
            service.Start(new string[0]);
            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
        }

        private void IdServiceInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            int port = 7777;
            string portArgs = this.Context.Parameters[Properties.Port];
            if (!string.IsNullOrWhiteSpace(portArgs) && !int.TryParse(portArgs, out port))
                throw new ArgumentException("Invalid format", Properties.Port);

            _props[Properties.Port] = port;

            var assPath = Context.Parameters[Parameters.AssemblyPath];
            _installationFolder = Path.GetDirectoryName(assPath);
        }
    }
}
