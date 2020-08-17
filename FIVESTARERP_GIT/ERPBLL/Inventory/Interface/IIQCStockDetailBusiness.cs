using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IIQCStockDetailBusiness
    {
        bool SaveIQCStockIn(List<IQCStockDetailDTO> warehouseStockDetailDTOs, long userId, long orgId);
        bool SaveIQCStockInByIQCRequest(long reqId, string status, long orgId, long userId);
        IEnumerable<IQCStockDetailDTO> GetAllIQCStockDetailList(string refNum, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string status, string stockStatus, string formDate, string toDate, long orgId);
        bool SaveWarehouseStockInByIQCReturnReacive(long reqId, string status, long orgId, long userId);
        bool SaveIQCStockOutByIQCReturn(IQCItemReqInfoListDTO model, long orgId, long userId);
    }
}
