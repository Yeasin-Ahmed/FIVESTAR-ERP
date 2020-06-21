using ERPBO.Production.DomainModels;
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
    }
}
