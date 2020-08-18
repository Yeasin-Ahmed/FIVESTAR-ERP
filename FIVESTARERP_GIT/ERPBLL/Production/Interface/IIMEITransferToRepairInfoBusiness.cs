using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IIMEITransferToRepairInfoBusiness
    {
        Task<bool> SaveIMEITransferToRepairInfoAsync(string imei, List<QRCodeProblemDTO> problems, long userId,long orgId);
        IEnumerable<IMEITransferToRepairInfoDTO> GetIMEITransferToRepairInfosByQuery(long? transferId, string qrCode, string imei,long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId,string status,string transferCode, long orgId);
        IEnumerable<IMEITransferToRepairInfo> GetIMEITransferToRepairInfosByTransferId(long transferId, long orgId);
        bool SaveIMEIStatusByTransferInfoId(long transferId, string status, long userId, long orgId);
        bool IsIMEIExistInTransferWithStatus(string imei, string status, long orgId);
        IMEITransferToRepairInfoDTO GetIMEIWiseItemInfo(string imei, string qrCode, string status, long orgId);
        ExecutionStateWithText CheckingAvailabilityOfPackagingRepairRawStock(long modelId, long itemId, long packagingLineId, long orgId);
    }
}
