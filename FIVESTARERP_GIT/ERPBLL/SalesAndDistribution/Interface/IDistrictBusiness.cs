using ERPBO.Common;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IDistrictBusiness
    {
        IEnumerable<District> GetDistricts(long orgId);
        District GetDistrictById(long districtId, long orgId);
        bool SaveDistrict(DistrictDTO dto, long userId, long orgId);
        IEnumerable<Dropdown> GetDistrictWithDivision(long orgId);
    }
}
