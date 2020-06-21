using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class TransferStockToAssemblyInfoViewModel
    {
        public long TSAInfoId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        [Range(1,long.MaxValue)]
        public long? DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long? LineId { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long? AssemblyId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [Range(1,long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [Range(1,long.MaxValue)]
        public long? ItemId { get; set; }
        [Range(1, int.MaxValue)]
        public int? ForQty { get; set; }

        // Custom Property
        public string ModelName { get; set; }
        public string LineNumber { get; set; }
        public string WarehouseName { get; set; }
        public string AssemblyName { get; set; }
        public int ItemCount { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
    }
}
