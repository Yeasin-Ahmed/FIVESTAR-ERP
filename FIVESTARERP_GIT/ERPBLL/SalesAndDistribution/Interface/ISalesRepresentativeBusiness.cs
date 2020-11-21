using ERPBO.Common;
using ERPBO.SalesAndDistribution.CommonModels;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface ISalesRepresentativeBusiness
    {
        IEnumerable<SalesRepresentativeDTO> GetSalesRepresentatives(long orgId);
        IEnumerable<Dropdown> GetSalesRepresentativesBySeniorId(long userId, long orgId);
        SalesRepresentative GetSalesRepresentativeById(long id, long orgId);
        IEnumerable<SalesRepresentative> GetSalesRepresentativesByType(string srType, long orgId);
        bool SaveSalesRepresentative(SalesRepresentativeDTO dto, SRUser sRUser, long userId, long branchId, long orgId);
        IEnumerable<Dropdown> GetReportingSR(long orgId, long districtId, long zoneId, string srtype);
        IEnumerable<Dropdown> GetSRByDistrict(long districtId, long orgId);
        IEnumerable<Dropdown> GetSRByZone(long zoneId, long orgId);
    }
}
