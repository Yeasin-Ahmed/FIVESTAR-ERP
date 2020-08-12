using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IMiniStockTransferDetailBusiness
    {
        IEnumerable<MiniStockTransferDetail> GetMiniStockTransfersByOrg(long orgId);
        IEnumerable<MiniStockTransferDetail> GetMiniStockTransfersByInfo(long infoId,long orgId);
        IEnumerable<MiniStockTransferDetailDTO> GetMiniStockTransfersDetailByQuery(long? modelId,long? warehouseId,long? itemTypeId,long? itemId ,long? infoId, long orgId);

    }
}
