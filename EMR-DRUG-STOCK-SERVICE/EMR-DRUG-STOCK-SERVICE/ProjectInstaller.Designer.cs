
namespace EMR_DRUG_STOCK_SERVICE
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
            this.MunichDrugSyncProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.MunichDrugSyncServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // MunichDrugSyncProcessInstaller1
            // 
            this.MunichDrugSyncProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.MunichDrugSyncProcessInstaller1.Password = null;
            this.MunichDrugSyncProcessInstaller1.Username = null;
            // 
            // MunichDrugSyncServiceInstaller
            // 
            this.MunichDrugSyncServiceInstaller.Description = "Concept Munich Drug Stock Sync Service";
            this.MunichDrugSyncServiceInstaller.DisplayName = "Concept Munich Drug Stock Sync Service";
            this.MunichDrugSyncServiceInstaller.ServiceName = "Concept Munich Drug Stock Sync Service";
            this.MunichDrugSyncServiceInstaller.AfterUninstall += new System.Configuration.Install.InstallEventHandler(this.MunichDrugSyncServiceInstaller_AfterUninstall);
            this.MunichDrugSyncServiceInstaller.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.MunichDrugSyncServiceInstaller_BeforeUninstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MunichDrugSyncProcessInstaller1,
            this.MunichDrugSyncServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller MunichDrugSyncProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller MunichDrugSyncServiceInstaller;
    }
}