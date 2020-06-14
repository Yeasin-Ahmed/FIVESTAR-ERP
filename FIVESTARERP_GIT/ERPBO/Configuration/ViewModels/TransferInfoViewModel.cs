using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
   public class TransferInfoViewModel
    {
        public long TransferInfoId { get; set; }
        public string TransferCode { get; set; }
        [Range(1,long.MaxValue)]
        public long? BranchTo { get; set; }
        public long? SWarehouseId { get; set; }
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //custom p
        [StringLength(100)]
        public string SWarehouseName { get; set; }
        [StringLength(100)]
        public string BranchName { get; set; }

    }
}
