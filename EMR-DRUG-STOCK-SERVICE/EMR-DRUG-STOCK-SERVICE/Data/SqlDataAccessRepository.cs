using EMR_DRUG_STOCK_SERVICE.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMR_DRUG_STOCK_SERVICE.Data
{
   public class SqlDataAccessRepository
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourceConnectionString"].ConnectionString;
        private readonly string Query = ConfigurationManager.AppSettings["SourceQuery"];
        private readonly int Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["SourceConnectionTimeout"]);

        private static readonly ILog log = LogManager.GetLogger(typeof(SqlDataAccessRepository));
        private static readonly Lazy<SqlDataAccessRepository> lazy = new Lazy<SqlDataAccessRepository>(() => new SqlDataAccessRepository());
        private SqlDataAccessRepository()
        {
        }
        public static SqlDataAccessRepository GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<List<StockDetail>> GetPharmacyStockInfo()
        {            
            var result = new List<StockDetail>();
            using (var con = new SqlConnection(ConnectionString))
            {
                await con.OpenAsync();
                using (var cmd = new SqlCommand(Query, con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = Timeout;
                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new StockDetail();
                            obj.DRUG_CODE = reader["DrugCode"] is DBNull ? "" : Convert.ToString(reader["DrugCode"]);
                            obj.DRUG_QTY = reader["AvQty"] is DBNull ? 0m : Convert.ToDecimal(reader["AvQty"]);
                            result.Add(obj);
                        }
                    }
                }
            }
            
            return result;
        }
    }
}
