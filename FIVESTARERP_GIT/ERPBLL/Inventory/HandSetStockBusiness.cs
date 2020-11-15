using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModels;
using ERPBO.SalesAndDistribution.CommonModels;
using ERPDAL.InventoryDAL;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class HandSetStockBusiness : IHandSetStockBusiness
    {
        //
        private readonly IInventoryUnitOfWork _inventoryDb;
        
        private readonly HandSetStockRepository _handSetStockRepository;

        public HandSetStockBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution, IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            this._handSetStockRepository = new HandSetStockRepository(this._inventoryDb);
        }

        public IEnumerable<HandSetStockDTO> GetHandSetStocks(long orgId, long? categoryId, long? brandId, long? modelId, long? colorId, long? warehouseId, long? itemTypeId, long? itemId, string status, string imei, string fromDate, string toDate, string cartoonNo)
        {
            List<HandSetStockDTO> handSetStocks = new List<HandSetStockDTO>();
            string param = string.Empty;
            string query = string.Empty;
            if(categoryId !=null && categoryId > 0)
            {
                param += string.Format(@" and h.CategoryId={0} ",categoryId);
            }
            if (brandId != null && brandId > 0)
            {
                param += string.Format(@" and h.BrandId={0} ",brandId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and h.ModelId={0} ", modelId);
            }
            if (colorId != null && colorId > 0)
            {
                param += string.Format(@" and h.ColorId={0}", colorId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and h.WarehouseId={0}",warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and h.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and h.ItemId={0}", itemId);
            }
            if(!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and h.StockStatus='{0}'", status.Trim());
            }
            if (!string.IsNullOrEmpty(imei) && imei.Trim() != "")
            {
                param += string.Format(@" and h.AllIMEI LIKE '%{0}%'", imei.Trim());
            }
            if (!string.IsNullOrEmpty(cartoonNo) && cartoonNo.Trim() != "")
            {
                param += string.Format(@" and h.CartoonNo LIKE '%{0}%'", cartoonNo.Trim());
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(h.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(h.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(h.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select h.StockId,h.CartoonId,h.CartoonNo,h.CategoryId,c.CategoryName,h.BrandId,
b.BrandName,h.ModelId,d.DescriptionName'ModelName',h.ColorId,clr.ColorName,h.WarehouseId,
w.WarehouseName,h.ItemTypeId,it.ItemName'ItemTypeName',h.ItemId,i.ItemName,
h.StockStatus,h.AllIMEI,h.IMEI,h.EntryDate,h.EUserId,app.UserName 'EntryUser',h.UpdateDate,(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId=ISNULL(h.UpUserId,0)) 'UpdateUser' 
From tblHandSetStock h
Inner Join [ControlPanel].dbo.tblApplicationUsers app on h.EUserId= app.UserId
Left Join tblCategory c on h.CategoryId = c.CategoryId
Left Join tblBrand b on h.BrandId= b.BrandId
Left Join tblDescriptions d on h.ModelId = d.DescriptionId
Left Join tblColors clr on h.ColorId = clr.ColorId
Left Join tblWarehouses w on h.WarehouseId =w.Id
Left Join tblItemTypes it on h.ItemTypeId =it.ItemId
Left Join tblItems i on h.ItemId =i.ItemId
Where h.OrganizationId = {0} {1}", orgId, Utility.ParamChecker(param));

            handSetStocks = _inventoryDb.Db.Database.SqlQuery<HandSetStockDTO>(query).ToList();
            return handSetStocks;
        }

        public bool SaveHandSetItemStockIn(List<HandSetStockDTO> handSets, long userId,long branchId, long orgId)
        {
            var items = string.Join("",handSets.Select(s => s.ItemId).Distinct().ToArray());
            var itemDetails = _inventoryDb.Db.Database.SqlQuery<HandSetItemInfo>(string.Format(@"Select it.ItemId, cat.CategoryId,b.BrandId,de.DescriptionId,clr.ColorId From 
[Inventory].dbo.tblItems it
Inner Join [Inventory].dbo.tblDescriptions de on it.DescriptionId=de.DescriptionId
Inner Join [Inventory].dbo.tblColors clr on it.ColorId =clr.ColorId
Inner Join [Inventory].dbo.tblCategory cat on de.CategoryId = cat.CategoryId
Inner Join [Inventory].dbo.tblBrand b on de.BrandId = b.BrandId
Where it.ItemId IN ({1}) and it.OrganizationId={0}", orgId, items)).ToList();
            List<HandSetStock> handSetStocks = new List<HandSetStock>();
            foreach (var item in handSets)
            {
                var i = itemDetails.FirstOrDefault(s => s.ItemId == item.ItemId);
                HandSetStock handSetStock = new HandSetStock()
                {
                    CartoonId= item.CartoonId,
                    CartoonNo= item.CartoonNo,
                    WarehouseId =item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    CategoryId = i.CategoryId,
                    BrandId = i.BrandId,
                    ModelId = item.ModelId,
                    ColorId = i.ColorId,
                    IMEI = item.IMEI,
                    AllIMEI=item.AllIMEI,
                    OrganizationId=orgId,
                    BranchId = branchId,
                    EntryDate = DateTime.Now,
                    EUserId=userId,
                    Remarks="Stock In By Finish Goods",
                    StockStatus=StockStatus.StockIn
                };
                handSetStocks.Add(handSetStock);
            }
            _handSetStockRepository.InsertAll(handSetStocks);
            return _handSetStockRepository.Save();
        }
    }
}
