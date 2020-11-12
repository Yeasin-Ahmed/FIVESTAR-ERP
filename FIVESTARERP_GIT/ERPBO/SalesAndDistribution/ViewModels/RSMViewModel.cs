using ERPBO.SalesAndDistribution.CommonModels;
using System.Collections.Generic;


namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class RSMViewModel : SalesHierarchyCommonModel
    {
        public long RSMID { get; set; }

        // Customer Properties
        public List<SRUser> SRUsers { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string ZoneName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
