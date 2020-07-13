using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
    public interface IRequsitionInfoForJobOrderBusiness
    {
        IEnumerable<RequsitionInfoForJobOrder> GetAllRequsitionInfoForJobOrder(long jobordeId,long orgId, long branchId);
        IEnumerable<RequsitionInfoForJobOrder> GetAllRequsitionInfoForJob(long orgId, long branchId);
        RequsitionInfoForJobOrder GetAllRequsitionInfoOneByOrgId(long ReqId,long orgId, long branchId);
        RequsitionInfoForJobOrder GetAllRequsitionInfoForJobOrderId(long reqInfoId,long orgId);
        bool SaveRequisitionInfoForJobOrder(RequsitionInfoForJobOrderDTO requsitionInfoDTO, List<RequsitionDetailForJobOrderDTO> details, long userId, long orgId, long branchId);
        bool SaveRequisitionStatus(long reqId, string status, long userId, long orgId,long branchId);

        IEnumerable<DashboardRequestSparePartsDTO> DashboardRequestSpareParts(long orgId, long branchId);
    }
}
