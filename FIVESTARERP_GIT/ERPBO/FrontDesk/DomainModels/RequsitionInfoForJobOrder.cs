using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("tblRequsitionInfoForJobOrders")]
   public class RequsitionInfoForJobOrder
    {
        [Key]
        public long RequsitionInfoForJobOrderId { get; set; }
        public string RequsitionCode { get; set; }
        public long? SWarehouseId { get; set; }
        public string StateStatus { get; set; }
        public long? JobOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<RequsitionDetailForJobOrder> RequsitionDetailForJobOrders { get; set; }
    }
}
