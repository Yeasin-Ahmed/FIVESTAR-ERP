using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class RepairItemPartsViewModel
    {
        public long RIPartsId { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        public long? QRCodeId { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        public long? WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        public long? ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        public long? ItemId { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public long? UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long RepairItemId { get; set; }
    }
}
