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
        Task<bool> SaveTransferByIMEIScanningAsync(TransferPackagigRepairItemByIMEIScanningDTO dto, long user, long orgId);
        Task<TransferPackagingRepairItemToQcInfo> GetTransferPackagingRepairItemToQcInfoByIdAsync(long floorId, long packagingLineId, long modelId, long itemId, string status, long orgId);
        Task<bool> SavePackagingRapairToQCTransferInfoStateStatusAsync(long transferId, string status, long userId, long orgId);

        Task<TransferPackagingRepairItemToQcInfo> GetTransferPackagingRepairItemToQcInfoByIdAsync(long transferId, long orgId);
        IEnumerable<TransferPackagingRepairItemToQcInfoDTO> GetTransferPackagingRepairItemToQcInfosByQuery(long? floorId, long? packagingLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId);
    }
}
