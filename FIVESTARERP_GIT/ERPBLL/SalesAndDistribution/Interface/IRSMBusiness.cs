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
    public interface IRSMBusiness
    {
        IEnumerable<RSMDTO> GetRSMInformations(long orgId,long userId);
        RSM GetRSMById(long id, long orgId);
        bool SaveRSM(RSMDTO dto, SRUser sRUser, long userId, long branchId, long orgId);
        IEnumerable<RSM> GetRSMByOrg(long orgId);
    }
}
