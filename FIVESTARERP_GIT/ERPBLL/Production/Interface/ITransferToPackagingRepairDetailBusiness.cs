using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferToPackagingRepairDetailBusiness
    {
        IEnumerable<TransferToPackagingRepairDetailDTO> GetTransferToPackagingRepairDetailsByQuery(long? transferId,long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long ? itemTypeId, long? itemId, string transferCode,long orgId);
        IEnumerable<TransferToPackagingRepairDetail> GetTransferToPackagingRepairDetailsByTransferId(long transferId, long orgId);
    }
}
