using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRequisitionItemInfoBusiness
    {
        IEnumerable<RequisitionItemInfo> GetRequisitionItemInfos(long orgId);
        IEnumerable<RequisitionItemInfoDTO> GetRequisitionItemInfosByQuery(long? reqItemIfoId, long? floorId, long? assembly, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? reqInfoId,string status, string reqCode, string fromDate, string toDate, long orgId);

    }
}
