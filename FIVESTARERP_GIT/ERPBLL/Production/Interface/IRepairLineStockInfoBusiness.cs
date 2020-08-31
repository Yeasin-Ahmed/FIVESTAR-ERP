using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairLineStockInfoBusiness
    {
        IEnumerable<RepairLineStockInfo> GetRepairLineStockInfos(long orgId);
        IEnumerable<RepairLineStockInfo> GetRepairLineStockInfoByRepairAndItemId(long repairId, long itemId, long orgId);
        RepairLineStockInfo GetRepairLineStockInfoByRepairAndItemAndModelId(long repairId, long itemId, long modelId, long orgId);
        RepairLineStockInfo GetRepairLineStockInfoByRepairQCAndItemAndModelId(long repairId, long itemId,long qcId, long modelId, long orgId);
        Task<RepairLineStockInfo> GetRepairLineStockInfoByRepairAndItemAndModelIdAsync(long repairId, long itemId, long modelId, long orgId);
        IEnumerable<RepairLineStockInfoDTO> GetRepairLineStockInfosQuery(long? floorId, long? modelId, long? qcId, long? repairId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);

        IEnumerable<RepairLineStockInfoDTO> GetRepairLineStocksForReturnStock(long repairLineId, long floorId, long modelId, long orgId);
    }
}
