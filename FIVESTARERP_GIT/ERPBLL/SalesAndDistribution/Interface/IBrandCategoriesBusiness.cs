using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IBrandCategoriesBusiness
    {
        IEnumerable<BrandAndCategoriesDTO> GetBrandAndCategories(long brandId, long orgId);
        BrandCategories GetBrandCategoriesByIds(long brandId, long categoryId, long orgId);
        bool SaveBrandCategories(long brandId,long[] brandCategories, long userId, long branchId, long orgId);
    }
}
