using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferPackagingRepairItemToQcDetailBusiness
    {
        IEnumerable<TransferPackagingRepairItemToQcDetail> GetPackagingRepairItemToQcDetailsByTransferId(long transferId, long orgId);
        Task<IEnumerable<TransferPackagingRepairItemToQcDetail>> GetPackagingRepairItemToQcDetailsByTransferIdAsync(long transferId, long orgId);

        IEnumerable<TransferPackagingRepairItemToQcDetailDTO> GetTransferPackagingRepairItemToQcDetailByQuery(string qrCode, string imei, long? transferId, long orgId);
    }
}
