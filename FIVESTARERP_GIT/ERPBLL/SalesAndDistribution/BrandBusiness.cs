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
        private readonly IBrandCategoriesBusiness _brandCategoriesBusiness;
        public BrandBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution, IBrandCategoriesBusiness brandCategoriesBusiness)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._brandCategoriesBusiness = brandCategoriesBusiness;
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
            Brand brand = null;
            if (model.BrandId == 0)
            {
                brand = new Brand()
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
                brand = _brandRepository.GetOneByOrg(s => s.BrandId == model.BrandId && s.OrganizationId == orgId);
                if(brand != null)
                {
                    brand.BrandName = model.BrandName;
                    brand.IsActive = model.IsActive;
                    brand.Remarks = model.Remarks;
                    brand.UpUserId = userId;
                    brand.UpdateDate = DateTime.Now;
                    _brandRepository.Update(brand);
                }
            }
            if (_brandRepository.Save()) {

                if(categories != null && categories.Length > 0)
                {
                    IsSuccess = _brandCategoriesBusiness.SaveBrandCategories(brand.BrandId, categories, userId, branchId, orgId);
                }
                else
                {
                    IsSuccess = true;
                }
            }
            return IsSuccess;
        }
    }
}
