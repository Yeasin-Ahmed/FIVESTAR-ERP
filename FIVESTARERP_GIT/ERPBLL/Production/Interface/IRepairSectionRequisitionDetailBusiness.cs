using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairSectionRequisitionDetailBusiness
    {
        IEnumerable<RepairSectionRequisitionDetailDTO> GetRepairSectionRequisitionDetailPendingByReqId(long reqId, long orgId);
        IEnumerable<RepairSectionRequisitionDetail> GetRepairSectionRequisitionDetailByInfoId(long reqId,long orgId);

        RepairSectionRequisitionDetail GetRepairSectionRequisitionDetailById(long reqDetailId, long reqInfo, long orgId);

    }
}
