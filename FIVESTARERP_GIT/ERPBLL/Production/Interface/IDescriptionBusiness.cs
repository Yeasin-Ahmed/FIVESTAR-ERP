using ERPBO.Common;
using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IDescriptionBusiness
    {
        IEnumerable<Description> GetDescriptionByOrgId(long orgId);
        Description GetDescriptionOneByOrdId(long id, long orgId);
        IEnumerable<Dropdown> GetAllDescriptionsInProductionStock(long orgId);
        
    }
}
