using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IBrandBusiness
    {
        Brand GetBrandById(long id,long orgId);
        IEnumerable<Brand> GetBrands(long orgId);
        bool SaveBrand(BrandDTO brand, long[] categories, long orgId, long branchId, long userId);
    }
}
