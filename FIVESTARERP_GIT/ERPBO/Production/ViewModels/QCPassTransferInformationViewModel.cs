using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class QCPassTransferInformationViewModel
    {
        public long QPassId { get; set; }
        [Range(1,long.MaxValue)]
        public long ProductionFloorId { get; set; }
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        public long AssemblyLineId { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        public long QCLineId { get; set; }
        [StringLength(100)]
        public string QCLineName { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        public long UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        public string StateStatus { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public List<QCPassTransferDetailViewModel> QCPassTransferDetails { get; set; }
    }
}
