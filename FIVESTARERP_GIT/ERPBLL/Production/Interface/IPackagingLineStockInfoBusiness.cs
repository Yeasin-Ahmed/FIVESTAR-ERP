using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
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
        IEnumerable<PackagingLineStockInfoDTO> GetPackagingLineStockInfosQuery(long? floorId, long? modelId, long? packagingId,long? warehouseId,long? itemTypeId, long? itemId,string lessOrEq,long orgId);
        IEnumerable<PackagingLineStockInfoDTO> GetPackagingLineStocksForReturnStock(long packagingLine, long floorId, long modelId, long orgId);
    }
}
