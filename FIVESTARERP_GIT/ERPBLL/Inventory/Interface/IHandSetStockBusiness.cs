using ERPBO.Inventory.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IHandSetStockBusiness
    {
        IEnumerable<HandSetStockDTO> GetHandSetStocks(long orgId,long? categoryId, long? brandId, long? modelId, long? colorId, long? warehouseId,long? itemTypeId, long? itemId,string status, string imei,string fromDate,string toDate,string cartoonNo );
        bool SaveHandSetItemStockIn(List<HandSetStockDTO> handSets,long userId,long branchId,long orgId);
    }
}
