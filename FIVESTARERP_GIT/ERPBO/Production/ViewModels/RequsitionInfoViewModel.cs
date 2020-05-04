using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
   public class RequsitionInfoViewModel
    {
        public long ReqInfoId { get; set; }
        [StringLength(100)]
        public string ReqInfoCode { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long LineId { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }

        //Custom

        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string LineNumber { get; set; }
        public int Qty { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [Required,StringLength(50)]
        public string RequisitionType { get; set; }
        public int TotalReqCount { get; set; }
    }
}
