using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly CategoryRepository _categoryRepository;
        public CategoryBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            this._categoryRepository = new CategoryRepository(this._inventoryDb);
        }
        public Category GetCategoryById(long id, long orgId)
        {
            return _categoryRepository.GetOneByOrg(s => s.CategoryId == id && s.OrganizationId == orgId);
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

        public bool IsDuplicateCategory(long categoryId, string categoryName, long orgId)
        {
            return _categoryRepository.GetOneByOrg(c => c.CategoryName == categoryName && c.CategoryId != categoryId && c.OrganizationId == orgId) != null ? true : false;
        }
    }
}
