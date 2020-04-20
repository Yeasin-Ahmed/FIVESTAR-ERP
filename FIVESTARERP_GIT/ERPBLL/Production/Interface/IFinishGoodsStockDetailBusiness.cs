using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsStockDetailBusiness
    {
        IEnumerable<FinishGoodsStockDetail> GelAllFinishGoodsStockDetailByOrgId(long orgId);
        bool SaveFinshGoodsStockIn(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId);

        bool SaveFinshGoodsStockOut(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId);
        IEnumerable<FinishGoodsStockDetailInfoListDTO> GetFinishGoodsStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum);
        IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseDailyProduction(long orgId);

        IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseOverAllProduction(long orgId);
    }
}
