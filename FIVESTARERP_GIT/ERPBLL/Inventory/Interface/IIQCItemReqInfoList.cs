using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IIQCItemReqInfoList
    {
        IEnumerable<IQCItemReqInfoList> GetIQCItemReqInfoListByOrgId(long orgId);
        IQCItemReqInfoList GetIQCItemReqById(long? reqId, long orgId);
        IQCItemReqInfoListDTO GetIQCItemReqDataById(long reqId, long orgId);
        bool SaveIQCReqInfoStatus(long reqId, string status, long orgId, long userId);
        IEnumerable<IQCItemReqInfoListDTO> GetIQCItemReqInfoLists(long? warehouseId, long? modelId, string status, long orgId);
    }
}
