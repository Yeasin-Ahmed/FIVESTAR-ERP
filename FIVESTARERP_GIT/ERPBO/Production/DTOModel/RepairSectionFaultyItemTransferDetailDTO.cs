using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class RepairSectionFaultyItemTransferDetailDTO
    {
        public long RSFIRDetailId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long ProductionFloorId { get; set; }
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        public long RepairLineId { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        public long QCLineId { get; set; }
        [StringLength(100)]
        public string QCLineName { get; set; }
        public long DescriptionId { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        public long WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName{ get; set; }
        public long ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        public long ItemId { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        public long UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        public int FaultyQty { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }        
        public long RSFIRInfoId { get; set; }

        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
        public string StateStatus { get; set; }
    }
}
