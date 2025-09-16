using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace EMR_DRUG_STOCK_SERVICE
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void MunichDrugSyncServiceInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {

        }

        private void MunichDrugSyncServiceInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            ServiceController sc = new ServiceController("Concept Munich Drug Stock Sync Service");
            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    new ServiceController(MunichDrugSyncServiceInstaller.ServiceName).Stop();
                    break;
            }
        }
    }
}
