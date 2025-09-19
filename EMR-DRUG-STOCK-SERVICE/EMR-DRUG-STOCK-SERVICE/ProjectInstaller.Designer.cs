
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
            this.PharmacyDrugSyncProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.PharmacyDrugSyncServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // PharmacyDrugSyncProcessInstaller1
            // 
            this.PharmacyDrugSyncProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.PharmacyDrugSyncProcessInstaller1.Password = null;
            this.PharmacyDrugSyncProcessInstaller1.Username = null;
            // 
            // PharmacyDrugSyncServiceInstaller
            // 
            this.PharmacyDrugSyncServiceInstaller.Description = "Concept Pharmacy Drug Stock Sync Service";
            this.PharmacyDrugSyncServiceInstaller.DisplayName = "Concept Pharmacy Drug Stock Sync Service";
            this.PharmacyDrugSyncServiceInstaller.ServiceName = "Concept Pharmacy Drug Stock Sync Service";
            this.PharmacyDrugSyncServiceInstaller.AfterUninstall += new System.Configuration.Install.InstallEventHandler(this.PharmacyDrugSyncServiceInstaller_AfterUninstall);
            this.PharmacyDrugSyncServiceInstaller.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.PharmacyDrugSyncServiceInstaller_BeforeUninstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PharmacyDrugSyncProcessInstaller1,
            this.PharmacyDrugSyncServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PharmacyDrugSyncProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller PharmacyDrugSyncServiceInstaller;
    }
}