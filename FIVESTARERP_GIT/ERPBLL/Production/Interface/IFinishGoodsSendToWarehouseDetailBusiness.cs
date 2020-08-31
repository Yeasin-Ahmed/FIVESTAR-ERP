using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsSendToWarehouseDetailBusiness
    {
        IEnumerable<FinishGoodsSendToWarehouseDetail> GetFinishGoodsDetailByInfoId(long infoId, long orgId);
        IEnumerable<FinishGoodsSendDetailListDTO> GetGoodsSendDetailList(long? lineId, long? warehouseId, long? modelId, long ?itemTypeId, long? itemId, string status, string refNum, long orgId, string fromDate, string toDate);
        IEnumerable<FinishGoodsSendToWarehouseDetailDTO> GetFinishGoodsSendToWarehouseDetailsByQuery(long? warehouseId, long? itemTypeId, long? itemId, string imei, string qrCode, long? transferId, string refNum, long orgId);
    }
}
