using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
   public class WarehouseStockInfoBusiness: IWarehouseStockInfoBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for       - Repository
        /// </summary>
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly WarehouseStockInfoRepository warehouseStockInfoRepository; // repo
        public WarehouseStockInfoBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            warehouseStockInfoRepository = new WarehouseStockInfoRepository(this._inventoryDb);
        }

        public IEnumerable<WarehouseStockInfo> GetAllWarehouseStockInfoByOrgId(long orgId)
        {
            return warehouseStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }

        public int? GetWarehouseStock(long? warehouseId, long? modelId, long? unitId, long? itemTypeId, long? itemId, long orgId)
        {
            var data = warehouseStockInfoRepository.GetOneByOrg(s => s.ItemTypeId == itemTypeId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
            int? qty = data != null ? (data.StockInQty - data.StockOutQty) : 0; // Current Stock
            return qty;
        }

        public IEnumerable<WarehouseStockInfoDTO> GetWarehouseStockInfoLists(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq,  long orgId)
        {
            IEnumerable<WarehouseStockInfoDTO> warehouseStockInfoLists = new List<WarehouseStockInfoDTO>();
            warehouseStockInfoLists = this._inventoryDb.Db.Database.SqlQuery<WarehouseStockInfoDTO>(QueryForWarehouseStockInfo(warehouseId, modelId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
            return warehouseStockInfoLists;
        }
        private string QueryForWarehouseStockInfo(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq,  long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and wsi.OrganizationId={0}", orgId);
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId={0}", modelId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                param += string.Format(@" and (wsi.StockInQty - wsi.StockOutQty)='{0}'", lessOrEq);
            }

            query = string.Format(@"Select wsi.StockInfoId,wh.WarehouseName 'Warehouse',de.DescriptionName 'ModelName',it.ItemName 'ItemType',i.ItemName 'Item',u.UnitSymbol 'Unit',wsi.StockInQty,wsi.StockOutQty,wsi.Remarks
From tblWarehouseStockInfo wsi
Left Join tblWarehouses wh on wsi.WarehouseId = wh.Id
Left Join tblDescriptions de on wsi.DescriptionId =de.DescriptionId
Left Join tblItemTypes it on wsi.ItemTypeId = it.ItemId
Left Join tblItems i on wsi.ItemId  = i.ItemId
Left Join tblUnits u on wsi.UnitId= u.UnitId      
Left Join [ControlPanel].dbo.tblApplicationUsers au on wsi.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
