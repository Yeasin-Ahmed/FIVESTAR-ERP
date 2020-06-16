using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface ITransferInfoBusiness
    {
        IEnumerable<TransferInfo> GetAllStockTransferByOrgIdAndBranch(long orgId, long branchId);
        IEnumerable<TransferInfo> GetAllStockTransferByOrgId(long orgId);
        TransferInfo GetStockTransferInfoById(long id, long orgId, long branchId);
        TransferInfo GetStockTransferInfoById(long id, long orgId);
        bool SaveTransferStockInfo(TransferInfoDTO info, List<TransferDetailDTO> details, long userId, long orgId, long branchId);
        bool SaveTransferInfoStateStatus(long transferId,long swarehouse, string status, long userId, long orgId, long branchId);
    }
}
