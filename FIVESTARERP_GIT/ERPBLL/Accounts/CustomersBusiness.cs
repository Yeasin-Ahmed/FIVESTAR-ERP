using ERPBLL.Accounts.Interface;
using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts
{
   public class CustomersBusiness: ICustomersBusiness
    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly CustomerRepository _customerRepository; // repo
        private readonly IAccountsHeadBusiness _accountsHeadBusiness;
        private readonly AccountsHeadRepository _accountsHeadRepository; // repo
        public CustomersBusiness(IAccountsUnitOfWork accountsDb, IAccountsHeadBusiness accountsHeadBusiness, AccountsHeadRepository accountsHeadRepository)
        {
            this._accountsDb = accountsDb;
            _customerRepository = new CustomerRepository(this._accountsDb);
            this._accountsHeadBusiness = accountsHeadBusiness;
            this._accountsHeadRepository = accountsHeadRepository;
        }

        public IEnumerable<AccountsCustomerDTO> GetAllCustomerList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<AccountsCustomerDTO>(
                string.Format(@" Select * From tblCustomers Where OrganizationId={0}", orgId)).ToList();
        }

        public AccountsCustomer GetCustomerByMobileNo(string mobile, long orgId)
        {
            return _customerRepository.GetOneByOrg(c => c.MobileNumber == mobile && c.OrganizationId == orgId);
        }

        public AccountsCustomer GetCustomerByOrgId(long cusId, long orgId)
        {
            return _customerRepository.GetOneByOrg(c => c.CustomerId == cusId && c.OrganizationId == orgId);
        }

        public bool IsDuplicateCustomerMobile(string mobile, long id, long orgId)
        {
            return _customerRepository.GetOneByOrg(s => s.MobileNumber == mobile  && s.CustomerId != id && s.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveAccountsCustomers(AccountsCustomerDTO dto,long userId, long orgId)
        {
            bool isSuccess = false;
            var CustomerAncId = _accountsHeadBusiness.GetCustomerAncestorId(orgId).AncestorId;
            var CustomerGHId = _accountsHeadBusiness.GetCustomerAncestorId(orgId).AccountId;
            if (dto.CustomerId==0)
            {
                AccountsCustomer customer = new AccountsCustomer();
                customer.CustomerName = dto.CustomerName;
                customer.PhoneNumber = dto.PhoneNumber;
                customer.MobileNumber = dto.MobileNumber;
                customer.Address = dto.Address;
                customer.Email = dto.Email;
                customer.Remarks = dto.Remarks;
                customer.EUserId = userId;
                customer.EntryDate = DateTime.Now;
                customer.OrganizationId = orgId;
                _customerRepository.Insert(customer);
                isSuccess = _customerRepository.Save();
                if(dto.CustomerId == 0)
                {
                    var cusMobile = GetCustomerByMobileNo(dto.MobileNumber, orgId);
                    if(cusMobile != null)
                    {
                        Account head = new Account();
                        head.CustomerId = customer.CustomerId;
                        head.AccountName = dto.CustomerName;
                        head.AncestorId = "," + CustomerGHId + CustomerAncId;
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
                var cus = GetCustomerByOrgId(dto.CustomerId, orgId);
                var accCustomer = _accountsHeadBusiness.GetCustomerByCustomerId(dto.CustomerId, orgId);
                cus.CustomerName = dto.CustomerName;
                cus.PhoneNumber = dto.PhoneNumber;
                cus.MobileNumber = dto.MobileNumber;
                cus.Address = dto.Address;
                cus.Email = dto.Email;
                cus.Remarks = dto.Remarks;
                cus.UpUserId = userId;
                cus.UpdateDate = DateTime.Now;
                _customerRepository.Update(cus);
                isSuccess = _customerRepository.Save();
                if (accCustomer != null)
                {
                    accCustomer.AccountName = dto.CustomerName;
                    accCustomer.UpUserId = userId;
                    accCustomer.UpdateDate = DateTime.Now;
                    _accountsHeadRepository.Update(accCustomer);
                    _accountsHeadRepository.Save();
                }
            }             
            return isSuccess;
        }
    }
}
