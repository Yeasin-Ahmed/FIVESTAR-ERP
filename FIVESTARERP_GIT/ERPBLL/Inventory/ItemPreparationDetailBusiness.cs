using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class ItemPreparationDetailBusiness : IItemPreparationDetailBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly ItemPreparationDetailRepository _itemPreparationDetailRepository; // repo
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        public ItemPreparationDetailBusiness(IInventoryUnitOfWork inventoryDb, IItemPreparationInfoBusiness itemPreparationInfoBusiness)
        {
            this._inventoryDb = inventoryDb;
            this._itemPreparationDetailRepository = new ItemPreparationDetailRepository(this._inventoryDb);
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
        }
        public IEnumerable<ItemPreparationDetail> GetItemPreparationDetails(long orgId)
        {
            return _itemPreparationDetailRepository.GetAll(i => i.OrganizationId == orgId).ToList();
        }

        public ItemPreparationDetail GetItemPreparationDetailsById(long id, long orgId)
        {
            return _itemPreparationDetailRepository.GetOneByOrg(i =>i.OrganizationId == id && i.OrganizationId == orgId);
        }

        public IEnumerable<ItemPreparationDetail> GetItemPreparationDetailsByInfoId(long infoId, long orgId)
        {
            return _itemPreparationDetailRepository.GetAll(i => i.OrganizationId == orgId && i.PreparationInfoId== infoId).ToList();
        }

        public IEnumerable<ItemPreparationDetail> GetItemPreparationDetailsByModelAndItem(long modelId, long itemId, long orgId)
        {
            IEnumerable<ItemPreparationDetail> details = new List<ItemPreparationDetail>();
            var info = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItem(modelId, itemId, orgId);
            if (info != null)
            {
                details = GetItemPreparationDetailsByInfoId(1, orgId);
            }
            return details;
        }
    }
}
