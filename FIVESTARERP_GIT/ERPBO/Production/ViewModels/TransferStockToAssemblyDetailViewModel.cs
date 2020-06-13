using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{    
    public class TransferStockToAssemblyDetailViewModel
    {
        public long TSADetailId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long TSAInfoId { get; set; }

        // Custom Properties
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
