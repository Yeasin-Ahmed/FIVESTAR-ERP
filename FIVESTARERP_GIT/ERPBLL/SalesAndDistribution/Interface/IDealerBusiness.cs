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
    public interface IDealerBusiness
    {
        IEnumerable<Dealer> GetDealers(long orgId);
        Dealer GetDealerByUserId(long userId, long orgId);
        IEnumerable<DealerDTO> GetDealerInformations(long orgId);
        Dealer GetDealerById(long id, long orgId);
        bool SaveDealer(DealerDTO dealer, SRUser user, long userId,long branchId, long orgId);
        IEnumerable<Dropdown> GetDealerRepresentatives(long orgId);
        IEnumerable<Dropdown> GetDealersByRepresentative(long representativeUserId, long orgId);
        IEnumerable<Dropdown> GetDealerByIndividualSRUserId(long userId, long orgId);
        IEnumerable<Dropdown> GetDealersByDistrict(long districtId, long orgId);
        IEnumerable<Dropdown> GetDealersByZone(long zoneId, long orgId);
    }
}
