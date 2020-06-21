using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface ICustomerBusiness
    {
        IEnumerable<Customer> GetAllCustomerByOrgId(long orgId);
        bool SaveCustomer(CustomerDTO customerDTO, long userId, long orgId);
        bool IsDuplicateCustomerPhone(string customerPhone, long id, long orgId);
        Customer GetCustomerOneByOrgId(long id, long orgId);
        bool DeleteCustomer(long id, long orgId);
        Customer GetCustomerByMobileNo(string mobileNo, long orgId);
    }
}
