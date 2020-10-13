using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
    public class JobOrderTSViewModel
    {
        public long JTSId { get; set; }
        [StringLength(100)]
        public string JobOrderCode { get; set; }
        public long? TSId { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public Nullable<DateTime> AssignDate { get; set; }
        public Nullable<DateTime> SignOutDate { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long JodOrderId { get; set; }
        // Custom Property
        [StringLength(100)]
        public string TSName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string JobOrderStateStatus { get; set; }
    }
}
