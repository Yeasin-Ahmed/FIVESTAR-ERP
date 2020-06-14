using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class TransferStockToQCInfoViewModel
    {
        public long TSQInfoId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        [Range(1, long.MaxValue)]
        public long? DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long? LineId { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long? AssemblyId { get; set; }
        [Range(1, long.MaxValue)]
        public long? QCLineId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custome Property
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string ProductionLineName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        [StringLength(100)]
        public string QCLineName { get; set; }
        public int? ItemCount { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
