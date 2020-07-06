using ERPBO.Production.DomainModels;
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
        IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetailByInfo(long transferInfoId, long orgId);
    }
}
