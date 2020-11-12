using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsSendToWarehouseInfoBusiness
    {
        IEnumerable<FinishGoodsSendToWarehouseInfo> GetFinishGoodsSendToWarehouseList(long orgId);
        IEnumerable<FinishGoodsSendToWarehouseInfoDTO> GetFinishGoodSendInfomations(long? lineId, long? warehouseId, long? modelId, string status, string fromDate, string toDate, string refNo, long orgId);
        FinishGoodsSendToWarehouseInfo GetFinishGoodsSendToWarehouseById(long id,long orgId);
        bool SaveFinishGoodsSendToWarehouse(FinishGoodsSendToWarehouseInfoDTO info, List<FinishGoodsSendToWarehouseDetailDTO> detail, long userId, long orgId );
        bool SaveFinishGoodsStatus(long sendId, long userId, long orgId);
        Task<bool> SaveFinishGoodsCartonAsync(FinishGoodsSendToWarehouseInfoDTO dto, long userId, long orgId);
        IEnumerable<FinishGoodsSendToWarehouseInfoDTO> GetFinishGoodsSendToWarehouseInfosByQuery(long? floorId, long? packagingLineId, long? warehouseId, long? modelId, string status, string transferCode, string fromDate, string toDate, long? transferId, long orgId);
    }
}
