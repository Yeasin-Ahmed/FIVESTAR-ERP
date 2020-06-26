using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingItemStockInfoBusiness
    {
        IEnumerable<PackagingItemStockInfo> GetPackagingItemStocks(long orgId);
        PackagingItemStockInfo GetPackagingItemStockInfoByPackagingId(long packagingLineId, long modelId, long itemId, long orgId);
        IEnumerable<PackagingItemStockInfo> GetPackagingItemStockInfoByModelAndItem(long modelId, long itemId, long orgId);
    }
}
