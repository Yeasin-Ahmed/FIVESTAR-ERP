using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairSectionRequisitionInfoBusiness
    {
        IEnumerable<RepairSectionRequisitionInfo> GetRepairSectionRequisitionInfos(long orgId);
        bool SaveRepairSectionRequisition(RepairSectionRequisitionInfoDTO model, long userId, long orgId );
        IEnumerable<RepairSectionRequisitionInfoDTO> GetRepairSectionRequisitionInfoList(long ?repairLineId, long? packagingLineId, long? modelId, long? warehouseId,string status, string requisitionCode, string fromDate, string toDate, string queryFor, string reqFor, long orgId);
        RepairSectionRequisitionInfo GetRepairSectionRequisitionById(long reqId, long orgId);
        RepairSectionRequisitionInfoDTO GetRepairSectionRequisitionDataById(long reqId, long orgId);
        bool SaveRepairSectionRequisitionIssueByWarehouse(RepairRequisitionInfoStateDTO model, long orgId, long userId);
        bool SaveRepairSectionRequisitionStatus(long requisitionId, string status, long orgId, long userId);
    }
}
