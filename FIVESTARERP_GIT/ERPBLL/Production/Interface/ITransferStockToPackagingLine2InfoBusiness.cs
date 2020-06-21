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
    public interface ITransferStockToPackagingLine2InfoBusiness
    {
        IEnumerable<TransferStockToPackagingLine2Info> GetStockToPL2Infos(long orgId);
        TransferStockToPackagingLine2Info GetStockToPL2InfoById(long id,long orgId);
        bool SaveTransferStockToPackaging2(TransferStockToPackagingLine2InfoDTO info, List<TransferStockToPackagingLine2DetailDTO> details, long userId, long orgId);
        bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId);
    }
}
