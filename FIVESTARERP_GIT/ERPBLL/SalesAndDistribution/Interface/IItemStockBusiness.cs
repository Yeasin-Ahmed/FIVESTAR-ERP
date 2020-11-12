using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IItemStockBusiness
    {
        IEnumerable<ItemStockDTO> GetItemStocks(long branchId, long orgId,long? modelId,long? warehouseId, long? itemTypeId, long? itemId, long? colorId, string status, string imei,string fromDate, string toDate);
        int RunProcess(long orgId,long userId,long branchId); 
    }
}
