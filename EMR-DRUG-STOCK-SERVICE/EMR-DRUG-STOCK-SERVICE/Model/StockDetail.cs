using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMR_DRUG_STOCK_SERVICE.Model
{
    public class StockDetail
    {
        public string DRUG_NAME { get; set; }
        public string DRUG_CODE { get; set; }
        public decimal DRUG_QTY { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
