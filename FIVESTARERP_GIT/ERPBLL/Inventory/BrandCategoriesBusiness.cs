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
    public class BrandCategoriesBusiness : IBrandCategoriesBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly BrandCategoriesRepository _brandCategoriesRepository;

        public BrandCategoriesBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            this._brandCategoriesRepository = new BrandCategoriesRepository(this._inventoryDb);
        }
        public IEnumerable<BrandAndCategoriesDTO> GetBrandAndCategories(long brandId, long orgId)
        {
            string query = string.Format(@"Select c.CategoryId,c.CategoryName From tblBrandCategories bc
Inner Join tblCategory c on bc.CategoryId = c.CategoryId and bc.BrandId={0}
Where c.OrganizationId={1}", brandId, orgId);
            return this._inventoryDb.Db.Database.SqlQuery<BrandAndCategoriesDTO>(query).ToList();
        }
        public BrandCategories GetBrandCategoriesByIds(long brandId, long categoryId, long orgId)
        {
            return _brandCategoriesRepository.GetOneByOrg(s => s.BrandId == brandId && s.CategoryId == categoryId && s.OrganizationId == orgId);
        }
        public IEnumerable<BrandDTO> GetBrandsByCategory(long categoryId, long orgId)
        {
            string query = string.Format(@"Select b.BrandId,b.BrandName From tblBrandCategories bc
Inner Join tblBrand b on b.BrandId = bc.BrandId and bc.CategoryId={0}
Where bc.OrganizationId={1}", categoryId, orgId);
            return this._inventoryDb.Db.Database.SqlQuery<BrandDTO>(query).ToList();
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
