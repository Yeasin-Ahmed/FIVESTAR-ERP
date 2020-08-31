using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IStockItemReturnInfoBusiness
    {
        StockItemReturnInfo GetStockItemReturnInfoById(long id, long orgId);
        IEnumerable<StockItemReturnInfo> GetStockItemReturnInfoByOrg(long orgId);
        bool SaveStockItemReturn(List<StockItemReturnDetailDTO> items, long userId, long orgId);
        bool UpdateStockItemReturnStatus(long id, string status, long userId, long orgId);
        IEnumerable<StockItemReturnInfoDTO> GetStockItemReturnInfosByQuery(long? modelId,long? floorId, long? assemblyId, long? repairId, long? packagingId, long? warehouse, long? transferId, string transferCode, string returnFlag, string status, string fromDate, string toDate, long orgId);
        bool SaveReturnItemsInWarehouseStockByStoreStockReturn(long returnId, string status, long userId, long orgId);
    }
}
