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
    public interface IASMBusiness
    {
        IEnumerable<ASMDTO> GetASMInformations(long orgId, long userId);
        ASM GetASMById(long id, long orgId);
        bool SaveASM(ASMDTO dto, SRUser sRUser, long userId, long branchId, long orgId);
    }
}
