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
    }
}
