using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class ModelAndColorWiseItemStockViewModel
    {
        public string ModelName { get; set; }
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public long StockInQty { get; set; }
        public long StockOutQty { get; set; }
    }
}
