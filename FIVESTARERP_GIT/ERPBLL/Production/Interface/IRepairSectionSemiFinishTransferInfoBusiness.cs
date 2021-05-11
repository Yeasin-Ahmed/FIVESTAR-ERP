using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IRepairSectionSemiFinishTransferInfoBusiness
    {
        bool SaveRepairSectionSemiFinishTransferItem(long[] qRCodesId, int qty, long userId, long orgId);
        IEnumerable<RepairSectionSemiFinishTransferInfoDTO> RepairSectionSemiFinishGoodReceive(long orgId);
        RepairSectionSemiFinishTransferInfo GetQRCodeDetailsByInfoId(long infoId,long orgId);
        bool UpdateStatusRepairSection(long infoId, long userId, long orgId);
    }
}
