
namespace TFW.IdService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.idServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.idServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // idServiceProcessInstaller
            // 
            this.idServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.idServiceProcessInstaller.Password = null;
            this.idServiceProcessInstaller.Username = null;
            // 
            // idServiceInstaller
            // 
            this.idServiceInstaller.Description = "Get your unique Device ID";
            this.idServiceInstaller.DisplayName = "IdService";
            this.idServiceInstaller.ServiceName = "IdService";
            this.idServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.idServiceProcessInstaller,
            this.idServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller idServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller idServiceInstaller;
    }
}