using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFaultyItemStockInfoBusiness
    {
        IEnumerable<FaultyItemStockInfo> GetFaultyItemStockInfos(long orgId);
        IEnumerable<FaultyItemStockInfo> GetFaultyItemByQCandRepair(long qcId, long repairLine, long orgId);
        FaultyItemStockInfo GetFaultyItemStockInfoByQCandRepairandItem(long qcId, long repairLine, long itemId,long orgId);
        FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndItem(long repairLine, long itemId, long orgId);
        FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndModelAndItem(long repairLine,long modelId, long itemId, long orgId);
        IEnumerable<FaultyItemStockInfoDTO> GetFaultyItemStockInfosByQuery(long? floorId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
