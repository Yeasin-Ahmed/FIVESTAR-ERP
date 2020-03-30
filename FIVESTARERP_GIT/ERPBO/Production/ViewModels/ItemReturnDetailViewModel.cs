using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class ItemReturnDetailViewModel
    {
        public long IRDetailId { get; set; }
        [StringLength(50)]
        public string IRCode { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
        public long? UnitId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long IRInfoId { get; set; }
        //Custom Property
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
    }
}
