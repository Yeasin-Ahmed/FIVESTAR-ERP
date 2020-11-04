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
        IEnumerable<RSMDTO> GetRSMInformations(long orgId);
        RSM GetRSMById(long id, long orgId);
        bool SaveRSM(RSMDTO dto, long userId, long orgId);
    }
}
