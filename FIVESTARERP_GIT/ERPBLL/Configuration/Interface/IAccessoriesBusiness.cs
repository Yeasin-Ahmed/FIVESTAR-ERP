using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IAccessoriesBusiness
    {
        IEnumerable<Accessories> GetAllAccessoriesByOrgId(long orgId);
        bool SaveAccessories(AccessoriesDTO accessoriesDTO, long userId, long orgId);
        bool IsDuplicateAccessoriesName(string accessoriesName, long id, long orgId);
        Accessories GetAccessoriesOneByOrgId(long id, long orgId);
        bool DeleteAccessories(long id, long orgId);
    }
}
