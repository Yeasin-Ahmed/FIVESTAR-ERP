using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly CustomerRepository customerRepository; // repo
        public CustomerBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            customerRepository = new CustomerRepository(this._configurationDb);
        }
        public bool DeleteCustomer(long id, long orgId, long branchId)
        {
            customerRepository.DeleteOneByOrg(cus => cus.CustomerId == id && cus.OrganizationId == orgId &&cus.BranchId==branchId);
            return customerRepository.Save();
        }

        public IEnumerable<Customer> GetAllCustomerByOrgId(long orgId, long branchId)
        {
            return customerRepository.GetAll(cus => cus.OrganizationId == orgId && cus.BranchId==branchId).ToList();
        }

        public Customer GetCustomerByMobileNo(string mobileNo,long orgId, long branchId)
        {
            mobileNo = mobileNo.Trim();
            return customerRepository.GetOneByOrg(cus => cus.CustomerPhone == mobileNo && cus.OrganizationId == orgId && cus.BranchId==branchId);
        }

        public Customer GetCustomerOneByOrgId(long id, long orgId, long branchId)
        {
            return customerRepository.GetOneByOrg(cus => cus.CustomerId == id && cus.OrganizationId == orgId && cus.BranchId==branchId);
        }

        public bool IsDuplicateCustomerPhone(string customerPhone, long id, long orgId, long branchId)
        {
            return customerRepository.GetOneByOrg(cus => cus.CustomerPhone == customerPhone && cus.CustomerId != id && cus.OrganizationId == orgId && cus.BranchId==branchId) != null ? true : false;
        }

        public bool SaveCustomer(CustomerDTO customerDTO, long userId, long orgId, long branchId)
        {
            Customer customer = new Customer();
            if (customerDTO.CustomerId == 0)
            {
                customer.CustomerName = customerDTO.CustomerName;
                customer.CustomerAddress = customerDTO.CustomerAddress;
                customer.CustomerPhone = customerDTO.CustomerPhone;
                customer.Remarks = customerDTO.Remarks;
                customer.OrganizationId = orgId;
                customer.BranchId = branchId;
                customer.EUserId = userId;
                customer.EntryDate = DateTime.Now;
                customerRepository.Insert(customer);
            }
            else
            {
                customer = GetCustomerOneByOrgId(customerDTO.CustomerId, orgId,branchId);
                customer.CustomerName = customerDTO.CustomerName;
                customer.CustomerAddress = customerDTO.CustomerAddress;
                customer.CustomerPhone = customerDTO.CustomerPhone;
                customer.Remarks = customerDTO.Remarks;
                customer.OrganizationId = orgId;
                customer.BranchId = branchId;
                customer.UpUserId = userId;
                customer.UpdateDate = DateTime.Now;
                customerRepository.Update(customer);
            }
            return customerRepository.Save();
        }
    }
}
