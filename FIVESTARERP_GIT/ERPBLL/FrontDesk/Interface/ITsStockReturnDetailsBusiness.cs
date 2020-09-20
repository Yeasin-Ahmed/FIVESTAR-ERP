using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
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
        IEnumerable<TsStockReturnDetailDTO> GetReturnParts(long orgId, long branchId, long? tsId, long? partsId, string jobCode, string fromDate, string toDate);
    }
}
