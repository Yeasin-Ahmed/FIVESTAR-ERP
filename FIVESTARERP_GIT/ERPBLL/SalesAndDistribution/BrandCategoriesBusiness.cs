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
    public class BrandCategoriesBusiness : IBrandCategoriesBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly BrandCategoriesRepository _brandCategoriesRepository;

        public BrandCategoriesBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._brandCategoriesRepository = new BrandCategoriesRepository(this._salesAndDistribution);
        }

        public IEnumerable<BrandAndCategoriesDTO> GetBrandAndCategories(long brandId, long orgId)
        {
            string query = string.Format(@"Select c.CategoryId,c.CategoryName From tblBrandCategories bc
Inner Join tblCategory c on bc.CategoryId = c.CategoryId and bc.BrandId={0}
Where c.OrganizationId={1}", brandId, orgId);
            return this._salesAndDistribution.Db.Database.SqlQuery<BrandAndCategoriesDTO>(query).ToList();
        }

        public BrandCategories GetBrandCategoriesByIds(long brandId, long categoryId, long orgId)
        {
            return _brandCategoriesRepository.GetOneByOrg(s => s.BrandId == brandId && s.CategoryId == categoryId && s.OrganizationId == orgId);
        }

        public bool SaveBrandCategories(long brandId, long[] brandCategories, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;
            List<BrandCategories> brandCategoriesList = new List<BrandCategories>();
            foreach (var brandCategory in brandCategories)
            {
                var isCategoryExistInThisBrand = this.GetBrandCategoriesByIds(brandId, brandCategory, orgId);
                if(isCategoryExistInThisBrand == null)
                {
                    BrandCategories brand = new BrandCategories()
                    {
                        BranchId = branchId,
                        CategoryId = brandCategory,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        BrandId =  brandId
                    };
                    brandCategoriesList.Add(brand);
                }
            }
            if(brandCategoriesList.Count > 0)
            {
                _brandCategoriesRepository.InsertAll(brandCategoriesList);
                IsSuccess = _brandCategoriesRepository.Save();
            }
            else
            {
                IsSuccess = true;
            }
            return IsSuccess;
        }
    }
}
