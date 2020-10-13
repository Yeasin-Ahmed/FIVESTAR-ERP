using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class TsStockReturnDetailViewModel
    {
        public long ReturnDetailId { get; set; }
        public long ReqInfoId { get; set; }
        public long JobOrderId { get; set; }
        [Required,StringLength(100)]
        public string RequsitionCode { get; set; }
        public long PartsId { get; set; }
        public int Quantity { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long ReturnInfoId { get; set; }
        //c
        [Required,StringLength(100)]
        public string PartsName { get; set; }
        [Required,StringLength(100)]
        public string PartsCode { get; set; }
        [Required,StringLength(100)]
        public string EntryUser { get; set; }
        public string JobOrderCode { get; set; }

    }
}
