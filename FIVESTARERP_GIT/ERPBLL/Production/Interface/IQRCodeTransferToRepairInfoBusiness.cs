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
    public interface IQRCodeTransferToRepairInfoBusiness
    {
        IEnumerable<QRCodeTransferToRepairInfo> GetRCodeTransferToRepairInfos(long orgId);
        IEnumerable<QRCodeTransferToRepairInfo> GetQRCodeTransferToRepairInfoByTransferId(long transferId, long orgId);
        Task<bool> SaveQRCodeTransferToRepairAsync(QRCodeTransferToRepairInfoDTO dto, long userId, long orgId);
        IEnumerable<QRCodeTransferToRepairInfoDTO> GetQRCodeTransferToRepairInfoByTransferIdByQuery(long transferId, long orgId);
        QRCodeTransferToRepairInfoDTO GetQRCodeWiseItemInfo(string qrCode, string status, long orgId);
        bool SaveQRCodeStatusByTrasnferInfoId(long transferId,string status,long userId, long orgId);
        bool StockOutByAddingFaultyWithQRCode(FaultyInfoByQRCodeDTO model, long userId, long orgId);
        ExecutionStateWithText CheckingAvailabilityOfSparepartsWithRepairLineStock(long modelId, long itemId, long repairLineId,long orgId);
        Task<QRCodeTransferToRepairInfo> GetQRCodeTransferToRepairInfoByIdAsync(long id, long orgId);
        bool IsQRCodeExistInTransferWithStatus(string qrCode, string status, long orgId);
        IEnumerable<QRCodeTransferToRepairInfoDTO> GetQRCodeTransferToRepairInfosByQuery(long? floorId, long? assemblyId, long? qcLineId, long? repairLineId, string qrCode,string transferCode ,string status, string date, long? userId, long orgId);
        IEnumerable<QRCodeTransferToRepairInfoDTO> GetRepairSectionReceiveQRCode(long? modelId, long orgId, long? lineId, long? qclineId, long? repairlineId);
        QRCodeTransferToRepairInfo GetOneByQRCodeById(long qrId, long orgId);
        bool QRCodeUpdateStatus(long[] qrCodes, long orgId,long userId);
        bool QRCodeStatusUpdate(string qrCode, long userId, long orgId);
        IEnumerable<QRCodeTransferToRepairInfo> GetQRCodeByQRCode(string qrCode,string status, long orgId);
    }
}
