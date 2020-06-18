using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferFromQCInfoBusiness
    {
        TransferFromQCInfo GetTransferFromQCInfoById(long transferId, long orgId);
        IEnumerable<TransferFromQCInfo> GetTransferFromQCInfos(long orgId);
        bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId);
        bool SaveTransfer(TransferFromQCInfoDTO infoDto, List<TransferFromQCDetailDTO> detailDto, long userId, long orgId);
    }
}
