using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IProductionToPackagingStockTransferInfoBusiness
    {
        IEnumerable<ProductionToPackagingStockTransferInfo> GetProductionToPackagingStockTransferInfos(long orgId);
        ProductionToPackagingStockTransferInfo GetProductionToPackagingStockTransferInfoById(long transferInfoId, long orgId);
        bool SaveProductionToPackagingStockTransfer(ProductionToPackagingStockTransferInfoDTO transferInfoDto, long userId, long orgId);
        bool SaveProductionToPackagingStockTransferState(long transferInfoId,string status, long userId, long orgId);
        IEnumerable<ProductionToPackagingStockTransferInfoDTO> GetProductionToPackagingStockTransferInfosByQuery(long? floorId, long? packagingId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, long orgId);
    }
}
