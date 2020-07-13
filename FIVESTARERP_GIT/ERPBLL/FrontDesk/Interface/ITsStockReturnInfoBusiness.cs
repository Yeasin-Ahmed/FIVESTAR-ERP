using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface ITsStockReturnInfoBusiness
    {
        bool SaveTsReturnStock(List<TsStockReturnInfoDTO> returnInfoList, long userId, long orgId, long branchId);
        IEnumerable<DashbordTsPartsReturnDTO> DashboardReturnParts(long orgId, long branchId);
    }
}
