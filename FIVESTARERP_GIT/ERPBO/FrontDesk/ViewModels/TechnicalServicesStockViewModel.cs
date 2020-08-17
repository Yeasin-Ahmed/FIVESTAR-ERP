using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class TechnicalServicesStockViewModel
    {
        public long TsStockId { get; set; }
        public long? JobOrderId { get; set; }
        public long? SWarehouseId { get; set; }
        public long? PartsId { get; set; }
        public int Quantity { get; set; }
        public int UsedQty { get; set; }
        public int ReturnQty { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //custom p
        //
        public long RequsitionInfoForJobOrderId { get; set; }
        public string RequsitionCode { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public long TSId { get; set; }
        public string StateStatus { get; set; }
        //
        public string SWarehouseName { get; set; }
        public string PartsName { get; set; }
        public int TotalQty
        {
            get
            {
                return (this.Quantity - this.UsedQty);
            }
        }
        public string MobilePartCode { get; set; }
        public string UserName { get; set; }
    }
}
