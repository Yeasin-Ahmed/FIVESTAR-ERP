using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCLineStockInfoBusiness
    {
        IEnumerable<QualityControlLineStockInfo> GetQCLineStockInfos(long orgId);
        IEnumerable<QualityControlLineStockInfo> GetQCLineStockInfoByQCAndItemId(long qcId,long itemId,long orgId);
        QualityControlLineStockInfo GetQCLineStockInfoByQCAndItemAndModelId(long QcId, long itemId,long modelId ,long orgId);
    }
}
