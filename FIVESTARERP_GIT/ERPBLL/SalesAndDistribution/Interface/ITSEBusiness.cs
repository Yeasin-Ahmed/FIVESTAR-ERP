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
    public interface ITSEBusiness
    {
        IEnumerable<TSEDTO> GetTSEInformations(long orgId, long userId);
        TSE GetTSEById(long id, long orgId);
        bool SaveTSE(TSEDTO dto, SRUser sRUser, long userId, long branchId, long orgId);
    }
}
