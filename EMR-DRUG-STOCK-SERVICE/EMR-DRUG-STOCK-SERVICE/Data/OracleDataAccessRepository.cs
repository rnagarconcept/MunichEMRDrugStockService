using EMR_DRUG_STOCK_SERVICE.Model;
using log4net;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace EMR_DRUG_STOCK_SERVICE.Data
{
    public class OracleDataAccessRepository : OracleRepositoryBase
    {
        private static readonly string packageName = ConfigurationManager.AppSettings["PackageName"];
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
                    OraCmd.Parameters.Add(":DRUG_QTY", OracleDbType.Decimal, qtys, ParameterDirection.Input);

                    // ArrayBindCount must equal number of records
                    OraCmd.ArrayBindCount = items.Count;

                    if (con.State == ConnectionState.Closed) con.Open();
                    // Truncate Existing Data
                    //TruncateExisting(con);
                    // Execute Command
                    OraCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Bulk Insert {ex.Message}", ex);
            }
        }

        public List<StockDetail> GetExistingDrugStock()
        {
            var result = new List<StockDetail>();
            try
            {
                using (var con = OpenConnection())
                {
                    var OraCmd = new OracleCommand();
                    OraCmd.Connection = con;
                    OraCmd.CommandText = "SELECT DRUG_CODE, DRUG_QTY FROM MUNICH_DRUG_SYNC_STG;";
                    OraCmd.CommandType = CommandType.Text;
                    var reader = OraCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new StockDetail();
                            obj.DRUG_CODE = reader["DRUG_CODE"].ToString();
                            result.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Bulk Insert {ex.Message}", ex);
            }
            return result;
        }

        public void BulkInsertDrugStock_Updated(List<StockDetail> items)
        {
            try
            {
                string[] drugCodes = items.Select(x => x.DRUG_CODE).ToArray();
                decimal[] quantities = items.Select(x => x.DRUG_QTY).ToArray();

                using (var con = OpenConnection())
                {
                    var cmd = new OracleCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"{packageName}.SAVE_UPDATE_DRUG_STOCK";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var pDrug = new OracleParameter
                    {
                        ParameterName = "p_drugcodes",
                        OracleDbType = OracleDbType.Varchar2,
                        CollectionType = OracleCollectionType.PLSQLAssociativeArray,
                        Value = drugCodes,
                        Size = drugCodes.Length,
                        Direction = System.Data.ParameterDirection.Input,                        
                        ArrayBindSize = GetArrayBindSizes(drugCodes.Length, 255)
                    };
                    cmd.Parameters.Add(pDrug);
                    
                    var pQty = new OracleParameter
                    {
                        ParameterName = "p_quantities",
                        OracleDbType = OracleDbType.Decimal,
                        CollectionType = OracleCollectionType.PLSQLAssociativeArray,
                        Value = quantities,
                        Size = quantities.Length,
                        Direction = System.Data.ParameterDirection.Input
                    };
                    cmd.Parameters.Add(pQty);
                    if (con.State == ConnectionState.Closed) con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Bulk Insert {ex.Message}", ex);
            }
        }

        static int[] GetArrayBindSizes(int len, int maxLen)
        {
            var arr = new int[len];
            for (int i = 0; i < len; i++) arr[i] = maxLen;
            return arr;
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
