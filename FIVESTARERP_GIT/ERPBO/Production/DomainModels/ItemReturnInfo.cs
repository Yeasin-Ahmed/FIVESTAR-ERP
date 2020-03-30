using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblItemReturnInfo")]
    public class ItemReturnInfo
    {
        [Key]
        public long IRInfoId { get; set; }
        [StringLength(50)]
        public string IRCode { get; set; }
        [StringLength(100)]
        public string ReturnType { get; set; }
        [StringLength(100)]
        public string FaultyCase { get; set; }
        public long?  LineId { get; set; }
        public long? WarehouseId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<ItemReturnDetail> ItemReturnDetails { get; set; }
    }
}
