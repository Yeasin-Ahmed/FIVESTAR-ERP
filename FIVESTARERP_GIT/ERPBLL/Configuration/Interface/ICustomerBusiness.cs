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
        IEnumerable<Customer> GetAllCustomerByOrgId(long orgId,long branchId);
        bool SaveCustomer(CustomerDTO customerDTO, long userId, long orgId, long branchId);
        bool IsDuplicateCustomerPhone(string customerPhone, long id, long orgId, long branchId);
        Customer GetCustomerOneByOrgId(long id, long orgId, long branchId);
        bool DeleteCustomer(long id, long orgId, long branchId);
        Customer GetCustomerByMobileNo(string mobileNo, long orgId, long branchId);
    }
}
