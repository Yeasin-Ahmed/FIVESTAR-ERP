using ERPBO.FrontDesk.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IRequsitionDetailForJobOrderBusiness
    {
        IEnumerable<RequsitionDetailForJobOrder> GetAllRequsitionDetailForJobOrder(long orgId, long branchId);
        IEnumerable<RequsitionDetailForJobOrder> GetAllRequsitionDetailForJobOrderId(long reqInfoId,long orgId, long branchId);
    }
}
