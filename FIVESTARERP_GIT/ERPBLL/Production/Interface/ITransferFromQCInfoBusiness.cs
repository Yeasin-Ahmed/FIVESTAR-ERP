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
        Task<TransferFromQCInfo> GetNonReceivedTransferFromQCInfoByQRCodeKeyAsync(long AssemblyId, long qcLineId, long repairLineId, long modelId, long warehouseId, long itemTypeId, long itemId, long orgId);
        IEnumerable<TransferFromQCInfo> GetTransferFromQCInfoByTransferFor(string transferFor, long orgId);
        bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId);
        bool SaveTransfer(TransferFromQCInfoDTO infoDto, List<TransferFromQCDetailDTO> detailDto, long userId, long orgId);
        IEnumerable<TransferFromQCInfoDTO> GetTransferFromQCInfos(long? floorId, long? qcLineId, long? repairLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string fromDate, string toDate,string transferCode, long? transferInfoId, long orgId);
    }
}
