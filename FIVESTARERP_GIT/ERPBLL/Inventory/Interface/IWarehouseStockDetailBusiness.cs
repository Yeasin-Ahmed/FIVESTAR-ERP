using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
   public interface IWarehouseStockDetailBusiness
    {
        IEnumerable<WarehouseStockDetail> GelAllWarehouseStockDetailByOrgId(long orgId);
        bool SaveWarehouseStockIn(List<WarehouseStockDetailDTO> warehouseStockDetailDTOs, long userId, long orgId);
        bool SaveWarehouseStockOutByProductionRequistion(long reqId,string status , long orgId, long userId);
        bool SaveWarehouseStockOut(List<WarehouseStockDetailDTO> warehouseStockDetailDTOs, long userId, long orgId, string flag);
        bool SaveWarehouseStockInByProductionItemReturn(long irInfoId, string status, long orgId, long userId);

        IEnumerable<WarehouseStockDetailInfoListDTO> GetWarehouseStockDetailInfoLists(long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum,long orgId);
    }
}
