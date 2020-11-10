using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.CommonModels
{
    public class SalesHierarchyCommonModel
    {
        [StringLength(150)]
        public string FullName { get; set; }
        public long DivisionId { get; set; }
        public long DistrictId { get; set; }
        public bool IsAllowToLogIn { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public long BranchId { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public long OrganizationId { get; set; }
        public long? UserId { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
    public class SRUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
