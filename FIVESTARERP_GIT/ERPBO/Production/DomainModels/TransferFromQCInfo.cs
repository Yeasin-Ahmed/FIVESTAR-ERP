using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblTransferFromQCInfo")]
    public class TransferFromQCInfo
    {
        [Key]
        public long TFQInfoId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long? DescriptionId { get; set; }
        public long? LineId { get; set; }
        public long? WarehouseId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? QCLineId { get; set; }
        public long? RepairLineId { get; set; }
        public long? PackagingLineId { get; set; }
        [StringLength(100)]
        public string TransferFor { get; set; }
        [StringLength(100)]
        public string RepairTransferReason { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<TransferFromQCDetail> TransferFromQCDetails { get; set; }
        // 19-Jun-2020
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int? ForQty { get; set; }
    }
}
