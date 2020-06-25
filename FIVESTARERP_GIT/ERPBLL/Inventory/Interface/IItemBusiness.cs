using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.Common;

namespace ERPBLL.Inventory.Interface
{
   public interface IItemBusiness
    {
        IEnumerable<Item> GetAllItemByOrgId(long orgId);
        bool SaveItem(ItemDomainDTO itemDomain, long userId, long orgId);
        bool IsDuplicateItemName(string itemName, long id, long orgId);
        ItemDomainDTO GetItemById(long itemId, long orgId);
        Item GetItemOneByOrgId(long id, long orgId);
        IEnumerable<ItemDomainDTO> GetAllItemsInProductionStockByLineId(long lineId, long orgId);
        IEnumerable<Dropdown> GetItemsByWarehouseId(long warehouseId, long orgId);

        IEnumerable<ItemDetailDTO> GetItemDetails(long orgId);
    }
}
