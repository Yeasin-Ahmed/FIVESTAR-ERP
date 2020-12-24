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
    public class BrandBusiness : IBrandBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly BrandRepository _brandRepository;
        private readonly IBrandCategoriesBusiness _brandCategoriesBusiness;
        public BrandBusiness(IInventoryUnitOfWork inventoryDb, IBrandCategoriesBusiness brandCategoriesBusiness)
        {
            this._inventoryDb = inventoryDb;
            this._brandCategoriesBusiness = brandCategoriesBusiness;
            this._brandRepository = new BrandRepository(this._inventoryDb);
        }
        public Brand GetBrandById(long id, long orgId)
        {
            return _brandRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.BrandId == id);
        }
        public IEnumerable<Brand> GetBrands(long orgId)
        {
            return _brandRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public IEnumerable<Brand> GetClientMobileBrand(string clientMobileBrandName, long orgId)
        {
            return _brandRepository.GetAll(s => s.OrganizationId == orgId && (string.IsNullOrEmpty(clientMobileBrandName) || s.BrandName == clientMobileBrandName.Trim()));
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
        public bool IsDuplicateBrand(long brandId, string brandName, long orgId)
        {
            return _brandRepository.GetOneByOrg(b => b.BrandName == brandName && b.BrandId != brandId && b.OrganizationId == orgId) != null ? true : false;
        }
    }
}
