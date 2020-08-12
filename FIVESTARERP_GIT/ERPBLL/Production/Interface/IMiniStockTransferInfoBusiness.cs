using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IMiniStockTransferInfoBusiness
    {
        IEnumerable<MiniStockTransferInfo> GetMiniStockTransferInfosByOrg(long orgId);
        MiniStockTransferInfo GetMiniStockTransferInfosById(long id,long orgId);
        IEnumerable<MiniStockTransferInfo> GetMiniStockTransferInfosByFloor(long floorId,long orgId);
        IEnumerable<MiniStockTransferInfo> GetMiniStockTransferInfosByPackaging(long packagingId,long floorId, long orgId);
        IEnumerable<MiniStockTransferInfoDTO> GetMiniStockTransferInfosByQuery(long? packagingId ,long? floorId,long? transferId, string transferCode, string status, string fromDate, string toDate, long orgId);
        bool SaveMiniStockTransfer(MiniStockTransferInfoDTO model, long userId, long orgId);
        bool SaveMiniStockTranferStatus(long transferId,string status, long userId, long orgId); 
    }
}
