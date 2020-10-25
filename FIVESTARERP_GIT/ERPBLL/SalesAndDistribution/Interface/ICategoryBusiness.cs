using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface ICategoryBusiness
    {
        IEnumerable<Category> GetCategories(long orgId);
        bool SaveCategory(CategoryDTO category, long userId,long branchId, long orgId);
    }
}
