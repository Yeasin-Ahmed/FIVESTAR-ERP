using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblIMEITransferToRepairInfo")]
    public class IMEITransferToRepairInfo
    {
        [Key]
        public long IMEITRInfoId { get; set; }
        public long ProductionFloorId { get; set; }
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        public long PackagingLineId { get; set; }
        [StringLength(100)]
        public string PackagingLineName { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        [StringLength(150)]
        public string IMEI { get; set; }
        public long DescriptionId { get; set; }
        public long WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        [StringLength(150)]
        public string StateStatus { get; set; }
        public long OrganizationId { get; set; }
        public long EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string TransferCode { get; set; }
        [ForeignKey("TransferToPackagingRepairInfo")]
        public long TransferId { get; set; }
        public TransferToPackagingRepairInfo TransferToPackagingRepairInfo { get; set; }
        public ICollection<IMEITransferToRepairDetail> IMEITransferToRepairDetails { get; set; }
    }
    
}
