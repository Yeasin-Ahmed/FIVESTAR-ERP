using ERPBO.Common;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
   public interface IDescriptionBusiness
    {
        IEnumerable<Description> GetDescriptionByOrgId(long orgId);
        Description GetDescriptionOneByOrdId(long id, long orgId);
        List<ModelColors> GetModelColors(long modelId, long orgId);
        IEnumerable<Dropdown> GetAllDescriptionsInProductionStock(long orgId);
        bool UpdateDescriptionTAC(DescriptionDTO model, long userId, long orgId);
        
    }
}
