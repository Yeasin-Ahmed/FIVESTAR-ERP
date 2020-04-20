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
        FinishGoodsSendToWarehouseInfo GetFinishGoodsSendToWarehouseById(long id,long orgId);
        bool SaveFinishGoodsSendToWarehouse(FinishGoodsSendToWarehouseInfoDTO info, List<FinishGoodsSendToWarehouseDetailDTO> detail, long userId, long orgId );
        bool SaveFinishGoodsStatus(long sendId, long userId, long orgId);
    }
}
