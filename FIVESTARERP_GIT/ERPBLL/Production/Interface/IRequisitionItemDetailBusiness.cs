using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRequisitionItemDetailBusiness
    {
        IEnumerable<RequisitionItemDetail> GetRequisitionItemDetails(long orgId);

        IEnumerable<RequisitionItemDetail> GetRequisitionItemDetailsByReqItemInfos(List<long> reqInfoItems,long orgId);
        IEnumerable<RequisitionItemDetailDTO> GetRequisitionItemDetailsByQuery(long? reqInfoId, long? reqItemInfoId, long ? reqItemDetailId, long ? itemTypeId, long? itemId,  long orgId);
    }
}
