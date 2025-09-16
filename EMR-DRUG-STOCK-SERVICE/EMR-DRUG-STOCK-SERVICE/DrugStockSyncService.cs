using EMR_DRUG_STOCK_SERVICE.Data;
using EMR_DRUG_STOCK_SERVICE.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMR_DRUG_STOCK_SERVICE
{
    public class DrugStockSyncService
    {
        private readonly ILog log = LogManager.GetLogger(typeof(DrugStockSyncService));

        private static readonly Lazy<DrugStockSyncService> lazy = new Lazy<DrugStockSyncService>(() => new DrugStockSyncService());
        private DrugStockSyncService()
        {
        }
        public static DrugStockSyncService GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }
        public async Task Sync()
        {
            log.Info("Start Processing Data Sync");
            try
            {
                // Clear Existing Records           
                var items = await SqlDataAccessRepository.GetInstance.GetPharmacyStockInfo();
                var uniqueItems = items.GroupBy(x => x.DRUG_CODE).Select(g => g.First()).ToList();
                // Insert Bulk
                OracleDataAccessRepository.GetInstance.BulkInsertDrugStock(uniqueItems);
            }
            catch (Exception ex)
            {
                log.Info($"Error in Sync Data {ex.Message}", ex);
            }
            log.Info("End Processing Data Sync");
        }
    }
}
