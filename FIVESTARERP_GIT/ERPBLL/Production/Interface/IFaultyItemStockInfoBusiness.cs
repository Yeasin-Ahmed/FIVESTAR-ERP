using ERPBO.Production.DomainModels;
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
    }
}
