using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution
{
    public class BrandBusiness : IBrandBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly BrandRepository _brandRepository;
        public BrandBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._brandRepository = new BrandRepository(this._salesAndDistribution);
        }
        public Brand GetBrandById(long id, long orgId)
        {
            return _brandRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.BrandId == id);
        }
        public IEnumerable<Brand> GetBrands(long orgId)
        {
            return _brandRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public bool SaveBrand(BrandDTO model, long[] categories, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            if(model.BrandId == 0)
            {
                Brand brand = new Brand()
                {
                    BrandName = model.BrandName,
                    BranchId = branchId,
                    IsActive = model.IsActive,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    Remarks = model.Remarks
                };
                _brandRepository.Insert(brand);
            }
            else
            {
                var brandInDb = _brandRepository.GetOneByOrg(s => s.BrandId == model.BrandId && s.OrganizationId == orgId);
                if(brandInDb != null)
                {
                    brandInDb.BrandName = model.BrandName;
                    brandInDb.IsActive = model.IsActive;
                    brandInDb.Remarks = model.Remarks;
                    brandInDb.UpUserId = userId;
                    brandInDb.UpdateDate = DateTime.Now;
                    _brandRepository.Update(brandInDb);
                }
            }
            if (_brandRepository.Save()) {

            }
            return IsSuccess;
        }
    }
}
