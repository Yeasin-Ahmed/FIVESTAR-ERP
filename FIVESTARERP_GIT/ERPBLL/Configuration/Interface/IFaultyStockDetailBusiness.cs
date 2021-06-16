using ERPBO.Configuration.DTOModels;
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
        bool SaveFaultyStockIn(List<ERPBO.Configuration.DTOModels.FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool SaveFaultyStock(List<ERPBO.Configuration.DTOModels.FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool SaveMobilePartStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId);
    }
}
