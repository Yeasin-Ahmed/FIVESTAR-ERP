using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
   public interface IWarehouseStockInfoBusiness
    {
        IEnumerable<WarehouseStockInfo> GetAllWarehouseStockInfoByOrgId(long orgId);
        int? GetWarehouseStock(long? warehouseId, long? modelId, long? unitId, long? itemTypeId, long? itemId, long orgId);
        IEnumerable<WarehouseStockInfoDTO> GetWarehouseStockInfoLists(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
