using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class RequsitionInfoForJobOrderViewModel
    {
        public long RequsitionInfoForJobOrderId { get; set; }
        public string RequsitionCode { get; set; }
        [Range(1, long.MaxValue)]
        public long? SWarehouseId { get; set; }
        public string StateStatus { get; set; }
        [Range(1, long.MaxValue)]
        public long? JobOrderId { get; set; }
        public string JobOrderCode { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
       
        public long? BranchId { get; set; }
        
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }

        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom p
        [StringLength(100)]
        public string SWarehouseName { get; set; }
        [StringLength(100)]
        public string Branch { get; set; }
    }
}
