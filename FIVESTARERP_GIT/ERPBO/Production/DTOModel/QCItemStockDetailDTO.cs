using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class QCItemStockDetailDTO
    {
        public long QCItemStockDetailId { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? QCId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? RepairLineId { get; set; }
        public long? PackagingLineId { get; set; }
        public long? LabId { get; set; }
        public long? DescriptionId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int Quantity { get; set; }
        public string StockStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Property
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        [StringLength(100)]
        public string QCName { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        [StringLength(100)]
        public string PackagingLineName { get; set; }
        [StringLength(100)]
        public string LabName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
    }
}
