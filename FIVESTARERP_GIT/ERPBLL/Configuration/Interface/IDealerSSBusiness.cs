using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IDealerSSBusiness
    {
        IEnumerable<DealerSS> GetAllDealerByOrgId(long orgId);
        DealerSS GetDealerById(long dealerId, long orgId);
        bool SaveDealer(DealerSSDTO dto,long orgId, long branchId, long userId);
        bool IsDuplicateDealer(string mobile, long id, long orgId);
        IEnumerable<DealerSSDTO> GetAllDealerForD(long orgId);
        DealerSS GetDealerByMobile(string mobile, long orgId);
    }
}
