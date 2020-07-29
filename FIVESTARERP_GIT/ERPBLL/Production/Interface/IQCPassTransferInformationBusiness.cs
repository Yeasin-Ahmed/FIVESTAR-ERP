using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCPassTransferInformationBusiness
    {
        IEnumerable<QCPassTransferInformation> GetQCPassTransferInformation(long orgId);
        QCPassTransferInformation GetQCPassTransferInformationById(long qcPassId,long orgId);
        bool SaveQCPassTransferInformation(QCPassTransferInformationDTO qcPassInfo, long userId, long orgId);
        Task<bool> SaveQCPassTransferToMiniStockByQRCodeAsync(QCPassTransferInformationDTO qcPassInfo, string qrCode, long userId, long orgId);
        Task<QCPassTransferInformation> GetQCPassTransferInformationByFloorAssemblyQcModelItemTypeItem(long floorId, long assembly,long qc, long model, long itemType, long item, string status, long orgId);

        IEnumerable<QCPassTransferInformationDTO> GetQCPassTransferInformationsByQuery(long? floorId, long? assemblyId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string qcPassCode, string status, string fromDate, string toDate, long orgId);
    }
}
