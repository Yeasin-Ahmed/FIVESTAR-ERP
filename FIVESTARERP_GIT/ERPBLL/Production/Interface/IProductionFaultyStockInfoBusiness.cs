using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;

namespace ERPBLL.Production.Interface
{
    public interface IProductionFaultyStockInfoBusiness
    {
        IEnumerable<ProductionFaultyStockInfo> GetProductionFaultyStockInfos(long orgId);
        IEnumerable<ProductionFaultyStockInfo> GetProductionFaultyByFloor(long floorId, long orgId);
        IEnumerable<ProductionFaultyStockInfo> GetProductionFaultyStockInfoByFloorAndItem(long floorId, long itemId,long orgId);        
        ProductionFaultyStockInfo GetProductionFaultyStockInfoByFloorAndModelAndItem(long floorId, long modelId, long itemId, long orgId);
        IEnumerable<ProductionFaultyStockInfoDTO> GetProductionFaultyStockInfosByQuery(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
