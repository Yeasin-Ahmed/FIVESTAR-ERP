using ERPBO.Accounts.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface ISupplierBusiness
    {
        IEnumerable<SupplierDTO> GetAllSupplierList(long orgId);
    }
}
