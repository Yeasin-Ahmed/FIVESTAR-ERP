using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
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
            return _itemPreparationDetailRepository.GetOneByOrg(i => i.PreparationDetailId == id && i.OrganizationId == orgId);
        }

        public IEnumerable<ItemPreparationDetail> GetItemPreparationDetailsByInfoId(long infoId, long orgId)
        {
            return _itemPreparationDetailRepository.GetAll(i => i.OrganizationId == orgId && i.PreparationInfoId == infoId).ToList();
        }

        public async Task<IEnumerable<ItemPreparationDetail>> GetItemPreparationDetailsByInfoIdAsync(long infoId, long orgId)
        {
            return await _itemPreparationDetailRepository.GetAllAsync(i => i.OrganizationId == orgId && i.PreparationInfoId == infoId);
        }

        public IEnumerable<ItemPreparationDetail> GetItemPreparationDetailsByModelAndItem(long modelId, long itemId, long orgId)
        {
            IEnumerable<ItemPreparationDetail> details = new List<ItemPreparationDetail>();
            var info = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItem(modelId, itemId, orgId);
            if (info != null)
            {
                details = GetItemPreparationDetailsByInfoId(info.PreparationInfoId, orgId);
            }
            return details;
        }

        public IEnumerable<ItemPreparationDetailWithInfoDTO> GetItemPreparationDetailWithInfo(long infoId, long orgId)
        {
            return this._inventoryDb.Db.Database.SqlQuery<ItemPreparationDetailWithInfoDTO>(string.Format(@"Select ipi.PreparationInfoId,ISNULL(i.ItemId,0) 'ItemIdSrc',ISNULL(it.ItemId,0) 'ItemTypeIdSrc',ISNULL(w.Id,0) 'WarehouseIdSrc',i.ItemName 'ItemNameSrc',it.ItemName 'ItemTypeNameSrc',w.WarehouseName 'WarehouseNameSrc', 
ipd.Quantity, 
ISNULL(ipi.ItemId,0) 'ItemIdTgt',(Select ItemName From tblItems Where ItemId= ipi.ItemId) 'ItemNameTgt',
ISNULL(ipi.ItemTypeId,0) 'ItemTypeIdTgt',(Select ItemName From tblItemTypes Where ItemId= ipi.ItemTypeId) 'ItemTypeNameTgt',
ISNULL(ipi.WarehouseId,0) 'WarehouseIdTgt',(Select WarehouseName From tblWarehouses Where Id= ipi.WarehouseId) 'WarehouseNameTgt',ipd.UnitId,u.UnitSymbol 'UnitName'
From tblItemPreparationDetail ipd
Inner Join tblItemPreparationInfo ipi on ipd.PreparationInfoId = ipi.PreparationInfoId
Inner Join tblWarehouses w on ipd.WarehouseId = w.Id
Inner Join tblItemTypes it on ipd.ItemTypeId = it.ItemId
Inner Join tblItems i on ipd.ItemId = i.ItemId 
Inner Join tblUnits u on ipd.UnitId = u.UnitId
Where ipd.PreparationInfoId = {0} and ipd.OrganizationId={1}", infoId, orgId)).ToList();
        }
    }
}
