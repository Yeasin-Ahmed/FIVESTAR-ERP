using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ReportModels
{
    public class ReportHead
    {
        public string BranchName { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public byte[] OrgLogo { get; set; }
        public byte[] ReportLogo { get; set; }
        public string OrgLogoPath { get; set; }
    }
}
