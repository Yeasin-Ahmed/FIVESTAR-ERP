using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class TempQRCodeTraceDTO
    {
        public long CodeId { get; set; }
        [StringLength(200)]
        public string CodeNo { get; set; }
        public byte?[] CodeImage { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? AssemblyId { get; set; }
        public long? DescriptionId { get; set; }
        public long? ColorId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        [StringLength(150)]
        public string ColorName { get; set; }
        [StringLength(200)]
        public string ReferenceNumber { get; set; }
        public string ReferenceId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Property
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
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
        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
        [StringLength(300)]
        public string IMEI { get; set; }
        [StringLength(200)]
        public string BatteryCode { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public long? PackagingLineId { get; set; }
        [StringLength(300)]
        public string PreviousIMEI { get; set; }
        public long? QCLineId { get; set; }
        public string QCLineName { get; set; }
        public string PackagingLineName { get; set; }
    }
}
