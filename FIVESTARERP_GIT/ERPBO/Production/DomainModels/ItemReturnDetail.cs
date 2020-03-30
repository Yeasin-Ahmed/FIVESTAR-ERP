using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblItemReturnDetail")]
    public class ItemReturnDetail
    {
        [Key]
        public long IRDetailId { get; set; }
        [StringLength(50)]
        public string IRCode { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public long? UnitId { get; set; }
        public int Quantity { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("ItemReturnInfo")]
        public long IRInfoId { get; set; }
        public ItemReturnInfo ItemReturnInfo { get; set; }
    }
}
