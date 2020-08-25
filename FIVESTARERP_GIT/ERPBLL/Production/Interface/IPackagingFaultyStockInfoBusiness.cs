using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingFaultyStockInfoBusiness
    {
        IEnumerable<PackagingFaultyStockInfo> GetPackagingFaultyStockInfos(long orgId);
        PackagingFaultyStockInfo GetPackagingFaultyStockInfoByPackagingLineAndItem(long packagingLineId, long itemId, long orgId);
        PackagingFaultyStockInfo GetPackagingFaultyStockInfoByPackagingLineAndModelAndItem(long packagingLineId, long modelId, long itemId, long orgId);
        IEnumerable<PackagingFaultyStockInfoDTO> GetPackagingFaultyStockInfosByQuery(long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
        PackagingFaultyStockInfo GetPackagingFaultyStockInfoByRepairAndModelAndItemAndFultyType(long packagingLineId, long modelId, long itemId, bool isChinaFaulty, long orgId);
    }
}
