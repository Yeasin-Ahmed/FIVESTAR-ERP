using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("tblRequsitionDetailForJobOrders")]
   public class RequsitionDetailForJobOrder
    {
        [Key]
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
        //
        [ForeignKey("RequsitionInfoForJobOrder")]
        public long RequsitionInfoForJobOrderId { get; set; }
        public RequsitionInfoForJobOrder RequsitionInfoForJobOrder { get; set; }


    }
}
