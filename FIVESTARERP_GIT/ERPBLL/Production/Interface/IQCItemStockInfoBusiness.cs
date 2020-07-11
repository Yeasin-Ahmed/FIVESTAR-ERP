using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCItemStockInfoBusiness
    {
        IEnumerable<QCItemStockInfo> GetQCItemStocks(long orgId);
        QCItemStockInfo GetQCItemStockInfById(long qcId, long modelId, long itemId, long orgId);
        QCItemStockInfo GetQCItemStockInfoByFloorAndQcAndModelAndItem(long floorId,long qcId, long modelId, long itemId, long orgId);
    }
}
