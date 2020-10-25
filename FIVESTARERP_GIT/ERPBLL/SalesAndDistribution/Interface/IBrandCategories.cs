using ERPBO.SalesAndDistribution.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IBrandCategories
    {
        BrandCategories GetBrandCategoriesByIds(long brandId, long categoryId, long orgId);
        bool SaveBrandCategories(long brandId,long[] brandCategories, long userId, long branchId, long orgId);
    }
}
