using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class DealerViewModel
    {
        public long DealerId { get; set; }
        [Required,StringLength(200)]
        public string DealerName { get; set; }
        [Required, StringLength(300)]
        public string Address { get; set; }
        [StringLength(100)]
        public string TelephoneNo { get; set; }
        [StringLength(100)]
        public string MobileNo { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(150)]
        public string ContactPersonName { get; set; }
        [StringLength(100)]
        public string ContactPersonMobile { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long ZoneId { get; set; }
        public long DistrictId { get; set; }
        public long DivisionId { get; set; }
        public long RepresentativeId { get; set; }
        public long RepresentativeUserId { get; set; }
        [StringLength(50)]
        public string RepresentativeFlag { get; set; }
        public bool IsAllowToLogIn { get; set; }
        public long? UserId { get; set; }

        // Custom Properties //
        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
        public string ZoneName { get; set; }
        public string DistrictName { get; set; }
        public string DivisionName { get; set; }
        public string RepresentativeName { get; set; }
    }
}
