using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.DTOModels
{
    public class ZoneDTO
    {
        public long ZoneId { get; set; }
        [StringLength(150)]
        public string ZoneName { get; set; }
        [StringLength(20)]
        public string PostalCode { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long? DivisionId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long DistrictId { get; set; }
        // Customer property
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string DistrictName { get; set; }
        public string DivisionName { get; set; }
    }
}
