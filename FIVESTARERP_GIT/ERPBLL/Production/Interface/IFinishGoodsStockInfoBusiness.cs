using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsStockInfoBusiness
    {
        IEnumerable<FinishGoodsStockInfo> GetAllFinishGoodsStockInfoByOrgId(long orgId);
        FinishGoodsStockInfo GetAllFinishGoodsStockInfoByItemLineId(long orgId, long itemId, long lineId);
        FinishGoodsStockInfo GetAllFinishGoodsStockInfoByLineAndModelId(long orgId, long itemId, long lineId, long modelId);
        Task<FinishGoodsStockInfo> GetAllFinishGoodsStockInfoByLineAndModelIdAsync(long orgId, long itemId, long lineId, long packagingLineId, long modelId);
        FinishGoodsStockInfo GetFinishGoodsStockInfoByAll(long orgId, long lineId,long warehouseId, long itemId, long modelId);
        IEnumerable<FinishGoodsStockInfoDTO> GetFinishGoodsStockInfosQuery(long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
