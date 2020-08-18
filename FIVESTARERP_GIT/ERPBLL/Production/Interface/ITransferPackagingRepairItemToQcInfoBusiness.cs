using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferPackagingRepairItemToQcInfoBusiness
    {
        Task<bool> SaveTransferByQRCodeScanningAsync(TransferPackagigRepairItemByIMEIScanningDTO dto, long user, long orgId);
        Task<TransferPackagingRepairItemToQcInfo> GetTransferPackagingRepairItemToQcInfoByIdAsync(long floorId, long packagingLineId, long modelId, long itemId, string status, long orgId);
    }
}
