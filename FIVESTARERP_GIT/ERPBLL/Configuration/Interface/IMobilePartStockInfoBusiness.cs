using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IMobilePartStockInfoBusiness
    {
        IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByOrgId(long orgId);
    }
}
