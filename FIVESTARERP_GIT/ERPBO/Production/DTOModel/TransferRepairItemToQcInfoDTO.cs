using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class TransferRepairItemToQcInfoDTO
    {
        public long TRQInfoId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long? DescriptionId { get; set; }
        public long? LineId { get; set; }
        public long? WarehouseId { get; set; }
        public long? QCLineId { get; set; }
        public long? RepairLineId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int? ForQty { get; set; }

        // Custom Property
        public string ModelName { get; set; }
        public string LineName { get; set; }
        public string WarehouseName { get; set; }
        public string QCLineName { get; set; }
        public string RepairLineName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public int ItemCount { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
    }
}
