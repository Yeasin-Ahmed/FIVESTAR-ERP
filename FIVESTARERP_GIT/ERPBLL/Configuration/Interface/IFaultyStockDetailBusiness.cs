using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockDetailBusiness
    {
        bool SaveFaultyStockIn(List<FaultyStockDetailsDTO> faultyStocksDto, long userId, long orgId, long branchId);
    }
}
