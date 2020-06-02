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
    public class CustomerServiceBusiness : ICustomerServiceBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly CustomerServiceRepository customerServiceRepository; // repo
        public CustomerServiceBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            customerServiceRepository = new CustomerServiceRepository(this._configurationDb);
        }
        public bool DeleteCustomerService(long id, long orgId)
        {
            customerServiceRepository.DeleteOneByOrg(services => services.CsId == id && services.OrganizationId == orgId);
            return customerServiceRepository.Save();
        }

        public IEnumerable<CustomerService> GetAllCustomerServiceByOrgId(long orgId)
        {
            return customerServiceRepository.GetAll(services => services.OrganizationId == orgId).ToList();
        }

        public CustomerService GetCustomerServiceOneByOrgId(long id, long orgId)
        {
            return customerServiceRepository.GetOneByOrg(services => services.CsId == id && services.OrganizationId == orgId);
        }

        public bool IsDuplicateCustomerServiceName(string name, long id, long orgId)
        {
            return customerServiceRepository.GetOneByOrg(services => services.Name == name && services.CsId != id && services.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveCustomerService(CustomerServiceDTO customerServiceDTO, long userId, long orgId)
        {
            CustomerService customerService = new CustomerService();
            if (customerServiceDTO.CsId == 0)
            {
                customerService.CsId = customerServiceDTO.CsId;
                customerService.Name = customerServiceDTO.Name;
                customerService.Address = customerServiceDTO.Address;
                customerService.PhoneNumber = customerServiceDTO.PhoneNumber;
                customerService.Remarks = customerServiceDTO.Remarks;
                customerService.OrganizationId = orgId;
                customerService.EUserId = userId;
                customerService.EntryDate = DateTime.Now;
                customerServiceRepository.Insert(customerService);
            }
            else
            {
                customerService = GetCustomerServiceOneByOrgId(customerServiceDTO.CsId, orgId);
                customerService.Name = customerServiceDTO.Name;
                customerService.Address = customerServiceDTO.Address;
                customerService.PhoneNumber = customerServiceDTO.PhoneNumber;
                customerService.Remarks = customerServiceDTO.Remarks;
                customerService.OrganizationId = orgId;
                customerService.UpUserId = userId;
                customerService.UpdateDate = DateTime.Now;
                customerServiceRepository.Update(customerService);
            }
            return customerServiceRepository.Save();
        }
    }
}
