using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IRequsitionInfoBusiness
    {
        IEnumerable<RequsitionInfo> GetAllReqInfoByOrgId(long orgId);
        bool SaveRequisition(ReqInfoDTO requsitionInfoDTO, long userId, long orgId);
        RequsitionInfo GetRequisitionById(long reqId, long orgId);
        bool SaveRequisitionStatus(long reqId, string status, long orgId,long userId);
        IEnumerable<DashboardRequisitionSummeryDTO> DashboardRequisitionSummery(long orgId);
        bool SaveRequisitionWithItemInfoAndDetail(RequsitionInfoDTO infoDTO, long userId, long orgId);
        IEnumerable<RequsitionInfoDTO> GetRequsitionInfosByQuery(long? floorId, long? assemblyId, long? warehouseId,long? modelId,string reqCode,string reqType, string reqFor, string fromDate, string toDate,string status, string reqFlag,long? reqInfoId, long orgId);
        
    }
}
