using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferStockToQCInfoBusiness
    {
        IEnumerable<TransferStockToQCInfo> GetStockToQCInfos(long orgId);
        TransferStockToQCInfo GetStockToQCInfoById(long id,long orgId);
        bool SaveTransferStockQC(TransferStockToQCInfoDTO info, List<TransferStockToQCDetailDTO> details, long userId, long orgId);
        bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId);
    }
}
