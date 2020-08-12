using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IIQCItemReqDetailList
    {
        IEnumerable<IQCItemReqDetailList> GetIQCItemReqInfoListByOrgId(long orgId);
        IEnumerable<IQCItemReqDetailList> GetIQCItemReqInfoListByInfo(long infoId,long orgId);
        bool SaveIQCItemReq(List<IQCItemReqDetailListDTO> iQCItemReqDetailListDTO, long userId, long orgId);
        IEnumerable<IQCItemReqDetailListDTO> GetIQCItemReqDetails(long reqId, long orgId);
        IQCItemReqInfoList GetIQCItemReqInfoById(long reqInfoId, long orgId);
        IEnumerable<IQCItemReqDetailList> GetIQCReqDetailDetailByInfoId(long id, long orgId);
        IQCItemReqDetailList GetIQCItemReqDetailById(long reqDetailId, long reqInfo, long orgId);
        bool SaveIQCItemRequestStatus(long reqId, string status, long orgId, long userId);
        bool SaveIQCItemRequestIssueByWarehouse(IQCItemReqInfoListDTO model, long orgId, long userId);
        IEnumerable<IQCItemReqDetailListDTO> GetIQCItemReqDetailList(long? itemTypeId, long? itemId, long? unitId, string lessOrEq, string fDate, string tDate, long orgId);
    }
}
