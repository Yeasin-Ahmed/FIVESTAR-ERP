using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class SupplierBusiness : ISupplierBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly SupplierRepository _supplierRepository;

        public SupplierBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            this._supplierRepository = new SupplierRepository(this._inventoryDb);
        }
        public Supplier GetSupplierById(long supplierId, long orgId)
        {
            return _supplierRepository.GetOneByOrg(s => s.SupplierId == supplierId && s.OrganizationId == orgId);
        }
        public IEnumerable<Supplier> GetSuppliers(long orgId)
        {
            return _supplierRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public bool SaveSupplier(SupplierDTO dto, long userId, long orgId)
        {
            if (dto.SupplierId == 0)
            {
                Supplier supplier = new Supplier
                {
                    SupplierName = dto.SupplierName,
                    Address = dto.Address,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    MobileNumber = dto.MobileNumber,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now
                };
                _supplierRepository.Insert(supplier);
            }
            else
            {
                var supplierInDb = this.GetSupplierById(dto.SupplierId, orgId);
                if(supplierInDb != null)
                {
                    supplierInDb.SupplierName = dto.SupplierName;
                    supplierInDb.Address = dto.Address;
                    supplierInDb.Email = dto.Email;
                    supplierInDb.PhoneNumber = dto.PhoneNumber;
                    supplierInDb.MobileNumber = dto.MobileNumber;
                    supplierInDb.IsActive = dto.IsActive;
                    supplierInDb.Remarks = dto.Remarks;
                    supplierInDb.UpUserId = userId;
                    supplierInDb.UpdateDate = DateTime.Now;
                    _supplierRepository.Update(supplierInDb);
                }
            }
            return _supplierRepository.Save();
        }
    }
}
