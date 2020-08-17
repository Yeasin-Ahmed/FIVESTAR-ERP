using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferToPackagingRepairInfoBusiness
    {
        Task<TransferToPackagingRepairInfo> GetNonReceivedTransferToPackagingRepairInfoAsync(long floorId, long packagingLineId, long modelId, long itemId, long orgId);
        IEnumerable<TransferToPackagingRepairInfoDTO> GetTransferToPackagingRepairInfosByQuery(long? floorId, long ?packagingLineId, long ? modelId,long? warehouseId, long? itemTypeId, long? itemId,string status,string fromDate,string toDate, string transferCode, long? transferId, long orgId);
        TransferToPackagingRepairInfo GetPackagingRepairInfoById(long transferId, long orgId);
        bool SavePakagingQCTransferStateStatus(long transferId,string status, long userId, long orgId);
    }
}
