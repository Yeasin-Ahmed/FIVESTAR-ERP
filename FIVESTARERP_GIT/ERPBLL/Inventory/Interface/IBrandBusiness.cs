using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IBrandBusiness
    {
        Brand GetBrandById(long id,long orgId);

        IEnumerable<Brand> GetClientMobileBrand(string clientMobileBrandName, long orgId);
        IEnumerable<Brand> GetBrands(long orgId);
        bool SaveBrand(BrandDTO brand, long[] categories, long orgId, long branchId, long userId);
        bool IsDuplicateBrand(long brandId, string brandName, long orgId);
    }
}
