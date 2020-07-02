using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERPBLL.Inventory
{
    public class ItemBusiness:IItemBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for        - Repository
        /// </summary>
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly ItemRepository itemRepository; // repo
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        public ItemBusiness(IInventoryUnitOfWork inventoryDb, IProductionStockInfoBusiness productionStockInfoBusiness, IItemTypeBusiness itemTypeBusiness)
        {
            this._inventoryDb = inventoryDb;
            itemRepository = new ItemRepository(this._inventoryDb);
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
        }

        public IEnumerable<Item> GetAllItemByOrgId(long orgId)
        {
            return itemRepository.GetAll(item => item.OrganizationId == orgId).ToList();
        }

        public IEnumerable<ItemDomainDTO> GetAllItemsInProductionStockByLineId(long lineId, long orgId)
        {
           var items= _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(orgId).Where(l => l.LineId == lineId).Select(s => s.ItemId).Distinct().ToList();

         return GetAllItemByOrgId(orgId).Where(it => items.Contains(it.ItemId)).Select(it => new ItemDomainDTO
            {
                ItemId = it.ItemId,
                ItemName = it.ItemName,
                ItemTypeId = it.ItemTypeId,
                UnitId = it.UnitId,
                IsActive = it.IsActive,
                Remarks = it.Remarks
            }).ToList();
        }

        public ItemDomainDTO GetItemById(long itemId, long orgId)
        {
            return itemRepository.GetAll(item => item.ItemId == itemId && item.OrganizationId == orgId).Select(it=> new ItemDomainDTO{
                ItemId = it.ItemId,
                ItemName = it.ItemName,
                ItemTypeId = it.ItemTypeId,
                UnitId = it.UnitId,
                IsActive = it.IsActive,
                Remarks = it.Remarks
            }).FirstOrDefault();
        }

        public IEnumerable<ItemDetailDTO> GetItemDetailByRepairFaultySection(long floorId, long repairLineId,long modelId, long orgId)
        {
            IEnumerable<ItemDetailDTO> details = new List<ItemDetailDTO>();
            details = this._inventoryDb.Db.Database.SqlQuery<ItemDetailDTO>(string.Format(@"Select (Cast(i.ItemId as nvarchar(100))+'#'+Cast(it.ItemId as nvarchar(100))+'#'+Cast(w.Id as nvarchar(100))) 'ItemId',(i.ItemName+' ['+it.ItemName+'-'+w.WarehouseName+']') as 'ItemName' From [Production].dbo.tblFaultyItemStockInfo  fis
Inner Join [Inventory].dbo.tblItems i on fis.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblItemTypes it on fis.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblWarehouses w on fis.WarehouseId = w.Id
Where 1= 1 and fis.OrganizationId={0} and fis.ProductionFloorId ={1} and fis.RepairLineId = {2} and fis.DescriptionId={3}
Order By w.WarehouseName,i.ItemName", orgId,floorId,repairLineId, modelId)).ToList();
            return details;
        }

        public IEnumerable<ItemDetailDTO> GetItemDetails(long orgId)
        {
            IEnumerable<ItemDetailDTO> details = new List<ItemDetailDTO>();
            details= this._inventoryDb.Db.Database.SqlQuery<ItemDetailDTO>(string.Format(@"Select (Cast(i.ItemId as nvarchar(100))+'#'+Cast(it.ItemId as nvarchar(100))+'#'+Cast(w.Id as nvarchar(100))) 'ItemId',(i.ItemName+' ['+it.ItemName+'-'+w.WarehouseName+']') as 'ItemName' From tblItems i
Inner Join tblItemTypes it on i.ItemTypeId = it.ItemId
Inner Join tblWarehouses w on it.WarehouseId = w.Id
Where 1=1 and i.OrganizationId={0}
Order By w.WarehouseName,i.ItemName", orgId)).ToList();
            return details;
        }

        public Item GetItemOneByOrgId(long id, long orgId)
        {
            return itemRepository.GetOneByOrg(item => item.ItemId == id && item.OrganizationId == orgId);
        }

        public IEnumerable<ItemDetailDTO> GetItemPreparationItems(long modelId, long itemId,long orgId)
        {
            IEnumerable<ItemDetailDTO> details = new List<ItemDetailDTO>();
            details = this._inventoryDb.Db.Database.SqlQuery<ItemDetailDTO>(string.Format(@"Select (Cast(i.ItemId as nvarchar(100))+'#'+Cast(it.ItemId as nvarchar(100))+'#'+Cast(w.Id as nvarchar(100))) 'ItemId',(i.ItemName+' ['+it.ItemName+'-'+w.WarehouseName+']'+' (Qty-'+Cast(ip.Quantity as nvarchar(30))+')') as 'ItemName' From tblItemPreparationDetail ip
Inner Join tblItemPreparationInfo ipi on ipi.PreparationInfoId = ip.PreparationInfoId
Inner Join tblItems i on ip.ItemId = i.ItemId
Inner Join tblItemTypes it on i.ItemTypeId = it.ItemId
Inner Join tblWarehouses w on it.WarehouseId = w.Id
Where 1=1 and i.OrganizationId={0} and ipi.DescriptionId={1} and ipi.ItemId={2}
Order By w.WarehouseName,i.ItemName", orgId, modelId, itemId)).ToList();
            return details;
        }

        public IEnumerable<Dropdown> GetItemsByWarehouseId(long warehouseId, long orgId)
        {
           return this._inventoryDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select (i.ItemName+' ['+it.ItemName+']') 'text', (Cast(i.ItemId as nvarchar(50))+'#'+Cast(it.ItemId as nvarchar(50))) 'value' From tblItems i
Inner Join tblItemTypes it on i.ItemTypeId = it.ItemId
Inner Join tblWarehouses w on it.WarehouseId = w.Id
Where 1=1 and w.Id={0} and w.OrganizationId={1}", warehouseId, orgId)).ToList();
        }

        public bool IsDuplicateItemName(string itemName, long id, long orgId)
        {
            return itemRepository.GetOneByOrg(item => item.ItemName == itemName && item.OrganizationId == orgId && item.ItemId != id) != null ? true : false;
        }

        public bool SaveItem(ItemDomainDTO itemDomain, long userId, long orgId)
        {
            Item items = new Item();
            if (itemDomain.ItemId == 0)
            {
                items.ItemName = itemDomain.ItemName;
                items.Remarks = itemDomain.Remarks;
                items.IsActive = itemDomain.IsActive;
                items.EUserId = userId;
                items.EntryDate = DateTime.Now;
                items.OrganizationId = orgId;
                items.ItemTypeId = itemDomain.ItemTypeId;
                items.UnitId = itemDomain.UnitId;
                items.ItemCode = GenerateItemCode(orgId, itemDomain.ItemTypeId);
                itemRepository.Insert(items);
            }
            else
            {
                items = GetItemOneByOrgId(itemDomain.ItemId, orgId);
                items.ItemName = itemDomain.ItemName;
                items.Remarks = itemDomain.Remarks;
                items.IsActive = itemDomain.IsActive;
                items.UpUserId = userId;
                items.UpdateDate = DateTime.Now;
                items.ItemTypeId = itemDomain.ItemTypeId;
                items.UnitId = itemDomain.UnitId;
                //items.ItemCode = GenerateItemCode(orgId, itemDomain.ItemTypeId);
                itemRepository.Update(items);
            }
            return itemRepository.Save();
        }

        private string GenerateItemCode(long OrgId, long itemTypeId)
        {
            string code = string.Empty;
            string newCode = string.Empty;
            string shortName = _itemTypeBusiness.GetItemType(itemTypeId, OrgId).ShortName;

            var lastItem = itemRepository.GetAll(i => i.ItemTypeId == itemTypeId && i.OrganizationId == OrgId).OrderByDescending(i => i.ItemId).FirstOrDefault();
            if (lastItem == null)
            {
                newCode = shortName + "00001";
            }
            else
            {
                code =lastItem.ItemCode.Substring(3);
                code = (Convert.ToInt32(code) + 1).ToString();
                newCode = shortName + code.PadLeft(5, '0');
            }
            return newCode;
        }
    }
}
