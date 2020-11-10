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
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        private readonly CategoryRepository _categoryRepository;
        public CategoryBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._categoryRepository = new CategoryRepository(this._salesAndDistributionDb);
        }
        public IEnumerable<Category> GetCategories(long orgId)
        {
            return _categoryRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public bool SaveCategory(CategoryDTO model, long userId, long branchId, long orgId)
        {
            if(model.CategoryId == 0)
            {
                Category category = new Category()
                {
                    CategoryName = model.CategoryName,
                    IsActive= model.IsActive,
                    Remarks = model.Remarks,
                    EUserId = userId,
                    BranchId = branchId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now
                };
                _categoryRepository.Insert(category);
            }
            else if(model.CategoryId > 0)
            {
                var categoryInDb = _categoryRepository.GetById(model.CategoryId);
                if(categoryInDb != null)
                {
                    categoryInDb.CategoryName = model.CategoryName;
                    categoryInDb.IsActive = model.IsActive;
                    categoryInDb.Remarks = model.Remarks;
                    categoryInDb.UpUserId = userId;
                    categoryInDb.UpdateDate = DateTime.Now;
                    _categoryRepository.Update(categoryInDb);
                }
            }
            return _categoryRepository.Save();
        }
        public Category GetCategoryById(long id, long orgId)
        {
            return _categoryRepository.GetOneByOrg(s => s.CategoryId == id && s.OrganizationId == orgId);
        }
    }
}
