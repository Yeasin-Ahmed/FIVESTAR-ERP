using ERPBLL.Accounts.Interface;
using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts
{
   public class SupplierBusiness: ISupplierBusiness
    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly SupplierRepository _supplierRepository; // repo
        private readonly IAccountsHeadBusiness _accountsHeadBusiness;
        private readonly AccountsHeadRepository _accountsHeadRepository; // repo
        public SupplierBusiness(IAccountsUnitOfWork accountsDb, IAccountsHeadBusiness accountsHeadBusiness, AccountsHeadRepository accountsHeadRepository)
        {
            this._accountsDb = accountsDb;
            _supplierRepository = new SupplierRepository(this._accountsDb);
            this._accountsHeadBusiness = accountsHeadBusiness;
            this._accountsHeadRepository = accountsHeadRepository;
        }

        public IEnumerable<AccountsSupplierDTO> GetAllSupplierList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountsSupplierDTO>(
                string.Format(@" Select * From tblSuppliers Where OrganizationId={0}", orgId)).ToList();
        }

        public AccountsSupplier GetSupplierByMobileNo(string mobile, long orgId)
        {
            return _supplierRepository.GetOneByOrg(s => s.MobileNumber == mobile && s.OrganizationId == orgId);
        }

        public AccountsSupplier GetSupplierByOrgId(long suppId, long orgId)
        {
            return _supplierRepository.GetOneByOrg(s => s.SupplierId == suppId && s.OrganizationId == orgId);
        }

        public bool IsDuplicateSupplierMobile(string mobile, long id, long orgId)
        {
            return _supplierRepository.GetOneByOrg(s => s.MobileNumber == mobile && s.SupplierId != id && s.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveAccountsSuppliers(AccountsSupplierDTO dto, long userId, long orgId)
        {
            bool isSuccess = false;
            var supplierAncId = _accountsHeadBusiness.GetSupplierAncestorId(orgId).AncestorId;
            var supplierGHId = _accountsHeadBusiness.GetSupplierAncestorId(orgId).AccountId;
            if (dto.SupplierId == 0)
            {
                AccountsSupplier supplier = new AccountsSupplier();
                supplier.SupplierName = dto.SupplierName;
                supplier.MobileNumber = dto.MobileNumber;
                supplier.PhoneNumber = dto.PhoneNumber;
                supplier.Email = dto.Email;
                supplier.Address = dto.Address;
                supplier.Remarks = dto.Remarks;
                supplier.EUserId = userId;
                supplier.OrganizationId = orgId;
                supplier.EntryDate = DateTime.Now;
                _supplierRepository.Insert(supplier);
                isSuccess =_supplierRepository.Save();
                if (dto.SupplierId == 0)
                {
                    var supmobile = GetSupplierByMobileNo(dto.MobileNumber, orgId);
                    if(supmobile != null)
                    {
                        Account head = new Account();
                        head.AccountName = dto.SupplierName;
                        head.SupplierId = supmobile.SupplierId;
                        head.AncestorId = "," + supplierGHId + supplierAncId;
                        head.IsGroupHead = false;
                        head.AccountType = "Balance Sheet";
                        head.OrganizationId = orgId;
                        head.EUserId = userId;
                        head.EntryDate = DateTime.Now;
                        _accountsHeadRepository.Insert(head);
                    }
                   _accountsHeadRepository.Save();
                }
            }
            else
            {
                var supp = GetSupplierByOrgId(dto.SupplierId, orgId);
                var accSupplier = _accountsHeadBusiness.GetSupplierBySupplierId(dto.SupplierId, orgId);
                supp.SupplierName = dto.SupplierName;
                supp.MobileNumber = dto.MobileNumber;
                supp.PhoneNumber = dto.PhoneNumber;
                supp.Email = dto.Email;
                supp.Address = dto.Address;
                supp.Remarks = dto.Remarks;
                supp.EUserId = userId;
                supp.OrganizationId = orgId;
                supp.EntryDate = DateTime.Now;
                _supplierRepository.Update(supp);
                isSuccess = _supplierRepository.Save();
                if (accSupplier != null)
                {
                    accSupplier.AccountName = dto.SupplierName;
                    accSupplier.UpUserId = userId;
                    accSupplier.UpdateDate = DateTime.Now;
                    _accountsHeadRepository.Update(accSupplier);
                    _accountsHeadRepository.Save();
                }
            }
            return isSuccess;
        }
    }
}
