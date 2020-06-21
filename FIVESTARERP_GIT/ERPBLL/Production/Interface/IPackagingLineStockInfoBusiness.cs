using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingLineStockInfoBusiness
    {
        IEnumerable<PackagingLineStockInfo> GetPackagingLineStockInfos(long orgId);
        IEnumerable<PackagingLineStockInfo> GetPackagingLineStockInfoByPackagingAndItemId(long packagingId, long itemId, long orgId);
        PackagingLineStockInfo GetPackagingLineStockInfoByPackagingAndItemAndModelId(long packagingId, long itemId, long modelId, long orgId);
    }
}
