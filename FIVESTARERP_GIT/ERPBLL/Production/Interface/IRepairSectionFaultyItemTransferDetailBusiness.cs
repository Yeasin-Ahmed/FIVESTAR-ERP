using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairSectionFaultyItemTransferDetailBusiness
    {
        IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetails(long orgId);
        IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetailsByInfo(long transferId,long orgId);
        IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetailByInfo(long transferInfoId, long orgId);
        IEnumerable<RepairSectionFaultyItemTransferDetailDTO> GetRepairSectionFaultyItemTransferDetailsByQuery(long? floorId, long? repairId ,string status ,long? transferInfoId, long orgId);
    }
}
