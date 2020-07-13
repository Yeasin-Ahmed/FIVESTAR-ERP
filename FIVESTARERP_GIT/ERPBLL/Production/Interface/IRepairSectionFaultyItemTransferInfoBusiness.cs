using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairSectionFaultyItemTransferInfoBusiness
    {
        IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByOrg(long orgId);
        IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByFloor(long floorId, long orgId);
        IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByRepairLine(long floorId,long RepairLine, long orgId);
        bool SaveRepairSectionFaultyItemTransfer(RepairSectionFaultyItemTransferInfoDTO faultyItems, long orgId, long userId);
        IEnumerable<RepairSectionFaultyItemTransferInfoDTO> GetRepairSectionFaultyItemTransferInfoList(long ?floorId, long? repairLineId,string transferCode,string status,string fromDate, string toDate, long orgId);
        RepairSectionFaultyItemTransferInfo GetRepairSectionFaultyTransferInfoById(long transferId,long orgId);
    }
}
