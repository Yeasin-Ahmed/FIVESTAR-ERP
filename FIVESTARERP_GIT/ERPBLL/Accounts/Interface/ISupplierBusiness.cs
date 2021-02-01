using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface ISupplierBusiness
    {
        IEnumerable<AccountsSupplierDTO> GetAllSupplierList(long orgId);
        bool SaveAccountsSuppliers(AccountsSupplierDTO dto, long userId, long orgId);
        AccountsSupplier GetSupplierByOrgId(long suppId, long orgId);
        AccountsSupplier GetSupplierByMobileNo(string mobile, long orgId);
        bool IsDuplicateSupplierMobile(string mobile, long id, long orgId);
    }
}
