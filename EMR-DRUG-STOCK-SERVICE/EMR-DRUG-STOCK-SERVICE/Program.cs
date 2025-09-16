using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EMR_DRUG_STOCK_SERVICE
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static readonly bool EnabledDebugg = string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnabledDebugg"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnabledDebugg"]);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log.Info($"Service has been started. {DateTime.Now.ToLongDateString()}");
            try
            {
                if (EnabledDebugg)
                {
                    Debugg();
                    Console.ReadKey();
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] { new Service1() };
                    ServiceBase.Run(ServicesToRun);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in service {ex.Message}", ex);
            }
        }

        private static void Debugg()
        {
            try
            {
                var task = DrugStockSyncService.GetInstance.Sync();
                Task.WaitAll(task);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                log.Error($"Error in Processing Pedning Request {ex.Message}", ex);
            }
        }
    }
}
