using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class TSStockInfoDTO
    {
        public string TsRepairStatus { get; set; }
        public long JobOrderId { get; set; }
        public long TSId { get; set; }
        public List<TSStockDTO> StockDetails { get; set; }
    }

    public class TSStockDTO
    {
        public long RequsitionInfoForJobOrderId { get; set; }
        public string RequsitionCode { get; set; }
        public long PartsId { get; set; }
        public string PartsName { get; set; }
        public int Quantity { get; set; }
        public int UsedQty { get; set; }
    }
}
