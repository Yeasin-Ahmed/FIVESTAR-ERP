using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblIQCItemReqDetailList")]
    public class IQCItemReqDetailList
    {
        [Key]
        public long IQCItemReqDetailId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal IssueQty { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("IQCItemReqInfoList")]
        public long IQCItemReqInfoId { get; set; }
        public IQCItemReqInfoList IQCItemReqInfoList { get; set; }
    }
}
