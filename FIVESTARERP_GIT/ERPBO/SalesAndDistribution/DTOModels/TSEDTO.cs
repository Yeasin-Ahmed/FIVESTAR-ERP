using ERPBO.SalesAndDistribution.CommonModels;
using System.Collections.Generic;

namespace ERPBO.SalesAndDistribution.DTOModels
{
    public class TSEDTO : SalesHierarchyCommonModel
    {
        public long TSEID { get; set; }
        public long ASMUserId { get; set; }
        public long ZoneId { get; set; }
        // Customer Properties
        public string ZoneName { get; set; }
        public List<SRUser> SRUsers { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ASMName { get; set; }
    }
}
