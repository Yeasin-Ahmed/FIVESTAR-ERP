using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferRepairItemToQcInfoBusiness
    {
        TransferRepairItemToQcInfo GetTransferRepairItemToQcInfoById(long transferId, long orgId);
        IEnumerable<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfos(long orgId);
        bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId);
        bool SaveTransfer(TransferRepairItemToQcInfoDTO infoDto, List<TransferRepairItemToQcDetailDTO> detailDto, long userId, long orgId);
    }
}
