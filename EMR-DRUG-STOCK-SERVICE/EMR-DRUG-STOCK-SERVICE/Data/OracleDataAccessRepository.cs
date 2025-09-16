using EMR_DRUG_STOCK_SERVICE.Model;
using log4net;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EMR_DRUG_STOCK_SERVICE.Data
{
    public class OracleDataAccessRepository : OracleRepositoryBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OracleDataAccessRepository));
        private static readonly Lazy<OracleDataAccessRepository> lazy = new Lazy<OracleDataAccessRepository>(() => new OracleDataAccessRepository());
        private OracleDataAccessRepository()
        {
        }
        public static OracleDataAccessRepository GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }
        
        public void BulkInsertDrugStock(List<StockDetail> items)
        {
            try
            {
                using (var con = OpenConnection())
                {
                    var OraCmd = new OracleCommand();
                    OraCmd.Connection = con;
                    OraCmd.CommandText = "INSERT INTO MUNICH_DRUG_SYNC_STG (DRUG_CODE, DRUG_QTY) VALUES (:DRUG_CODE, :DRUG_QTY)";
                    OraCmd.CommandType = CommandType.Text;
                    // Create arrays
                    string[] codes = items.Select(x => x.DRUG_CODE).ToArray();
                    decimal[] qtys = items.Select(x => x.DRUG_QTY).ToArray();

                    // Define parameters with arrays
                    OraCmd.Parameters.Add(":DRUG_CODE", OracleDbType.NVarchar2, codes, ParameterDirection.Input);
                    OraCmd.Parameters.Add(":DRUG_QTY", OracleDbType.Int32, qtys, ParameterDirection.Input);

                    // ArrayBindCount must equal number of records
                    OraCmd.ArrayBindCount = items.Count;

                    if (con.State == ConnectionState.Closed) con.Open();
                    // Truncate Existing Data
                    TruncateExisting(con);
                    // Execute Command
                    OraCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Bulk Insert {ex.Message}", ex);
            }
        }

        private void TruncateExisting(OracleConnection con)
        {
            try
            {
                //var query = MUNICH_DRUG_SYNC_STG_TRUNCAT;
                var cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandText = "BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE MUNICH_DRUG_SYNC_STG DROP STORAGE'; END;";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
