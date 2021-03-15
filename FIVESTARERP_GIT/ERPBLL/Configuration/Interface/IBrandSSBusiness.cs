using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IBrandSSBusiness
    {
        IEnumerable<BrandSS> GetAllBrandByOrgId(long orgId);
        BrandSS GetOneBrandById(long brandId, long orgId);
        bool SaveBrandSS(BrandSSDTO dto, long orgId, long branchId, long userId);
        bool IsDuplicateBrandName(string name, long brandId, long orgId);
    }
}
