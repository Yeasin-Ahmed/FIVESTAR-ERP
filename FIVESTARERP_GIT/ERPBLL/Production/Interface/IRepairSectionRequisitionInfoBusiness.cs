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
        IEnumerable<RepairSectionRequisitionInfoDTO> GetRepairSectionRequisitionInfoList(long ?repairLineId, long? modelId, long? warehouseId,string status, string requisitionCode, string fromDate, string toDate ,long orgId);
    }
}
