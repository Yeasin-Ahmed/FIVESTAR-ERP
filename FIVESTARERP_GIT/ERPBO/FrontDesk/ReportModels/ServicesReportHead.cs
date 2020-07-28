using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ReportModels
{
   public class ServicesReportHead
    {
        public string BranchName { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public byte[] ReportImage { get; set; }
    }
}
