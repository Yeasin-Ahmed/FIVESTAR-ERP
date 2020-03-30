using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.ControlPanel.DTOModels
{
    public class BranchDTO
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string ShortName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
        public bool IsActive { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long OrgId { get; set; }
        public string Remarks { get; set; }
    }
}
