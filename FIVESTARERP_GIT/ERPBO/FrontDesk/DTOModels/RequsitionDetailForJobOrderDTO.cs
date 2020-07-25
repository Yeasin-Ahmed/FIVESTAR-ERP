using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class RequsitionDetailForJobOrderDTO
    {
        public long RequsitionDetailForJobOrderId { get; set; }
        public long? PartsId { get; set; }
        public int Quantity { get; set; }
        public long? JobOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string Remarks { get; set; }
        public long? SWarehouseId { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom p
        public string PartsName { get; set; }
        public string SWarehouseName { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public int AvailableQty { get; set; }
    }
}
