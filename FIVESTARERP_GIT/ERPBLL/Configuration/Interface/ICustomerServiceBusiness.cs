using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface ICustomerServiceBusiness
    {
        IEnumerable<CustomerService> GetAllCustomerServiceByOrgId(long orgId);
        bool SaveCustomerService(CustomerServiceDTO customerServiceDTO, long userId, long orgId);
        bool IsDuplicateCustomerServiceName(string name, long id, long orgId);
        CustomerService GetCustomerServiceOneByOrgId(long id, long orgId);
        bool DeleteCustomerService(long id, long orgId);
    }
}
