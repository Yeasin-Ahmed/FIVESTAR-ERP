using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
   public class DealerSSDTO
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string Address { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string ZoneName { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobile { get; set; }
        public string Remarks { get; set; }
        public string Flag { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string EntryUser { get; set; }
        public string Dealer { get; set; }
        public int Qty { get; set; }
    }
}
