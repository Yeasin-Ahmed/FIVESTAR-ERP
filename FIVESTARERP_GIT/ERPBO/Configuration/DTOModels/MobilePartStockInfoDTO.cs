using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
   public class MobilePartStockInfoDTO
    {
        public long MobilePartStockInfoId { get; set; }
        public long? MobilePartId { get; set; }
        public long? SWarehouseId { get; set; }
        public int? StockInQty { get; set; }
        public int? StockOutQty { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom p
        public string MobilePartName { get; set; }
        public string ServicesWarehouseName { get; set; }
        //
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public string PartsCode { get; set; }
        public int Quantity { get; set; }
    }
}
