using ERPBO.FrontDesk.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface ITsStockReturnDetailsBusiness
    {
        IEnumerable<TsStockReturnDetail> GetAllTsStockReturn(long orgId, long BranchId);
    }
}
