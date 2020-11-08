using ERPBO.SalesAndDistribution.CommonModels;
using System.Collections.Generic;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class ASMViewModel : SalesHierarchyCommonModel
    {
        public long ASMID { get; set; }
        public long RSMID { get; set; }
        // Customer Properties
        public List<SRUser> SRUsers { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string RSMName { get; set; }
    }
}
