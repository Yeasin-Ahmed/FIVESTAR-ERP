using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class QRCodeTransferToRepairInfoViewModel
    {
        public long QRTRInfoId { get; set; }
        public long TransferId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        [Range(1, long.MaxValue)]
        public long FloorId { get; set; }
        [Range(1, long.MaxValue)]
        public long QCLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long RepairLineId { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        [Range(1, long.MaxValue)]
        public long AssemblyLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public List<QRCodeProblemViewModel> QRCodeProblems { get; set; }

        // Custom Properties
        [StringLength(100)]
        public string FloorName { get; set; }
        [StringLength(100)]
        public string QCLineName { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
    }
}
