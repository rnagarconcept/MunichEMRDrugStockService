using log4net;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace EMR_DRUG_STOCK_SERVICE
{
    public partial class Service1 : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Service1));
        private static readonly int transactionInterval = string.IsNullOrEmpty(ConfigurationManager.AppSettings["TransactionInterval"]) ? 600 : Convert.ToInt32(ConfigurationManager.AppSettings["TransactionInterval"]);
        Timer timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ScheduleDrugSync();
        }

        protected override void OnStop()
        {
            log.Info($"Service has been stopped at {DateTime.Now}");
        }
        public void ScheduleDrugSync()
        {
            try
            {

                DateTime nowTime = DateTime.Now;
                var tickTime = transactionInterval * 1000;
                timer = new Timer(tickTime);
                timer.Elapsed += new ElapsedEventHandler(transactionTimer_Elapsed);
                timer.Start();
            }
            catch (Exception ex)
            {
                log.Error($"Error Drug Sync Schedule Service {ex.Message}", ex);
                throw ex;
            }
        }
        private void transactionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            log.Info($"Drug Sync Service has been started to processing {DateTime.Now}");
            ProcessDrugSync();
            log.Info($"Drug Sync Service processing has been completed {DateTime.Now}");
            Console.WriteLine("### Timer Stopped ### \n");
            timer.Stop();
            Console.WriteLine("### Scheduled Task Started ### \n\n");
            ScheduleDrugSync();
        }
        private void ProcessDrugSync()
        {
            try
            {
                Task.Run(async () =>
                {
                    await DrugStockSyncService.GetInstance.Sync();
                }).GetAwaiter().GetResult();               
            }
            catch (Exception ex)
            {
                log.Error($"Error in ProcessDrugSync {ex.Message}", ex);
            }
        }
    }
}


