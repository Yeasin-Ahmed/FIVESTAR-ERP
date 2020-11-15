using ERPBLL.Common;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution
{
    public class ItemStockBusiness : IItemStockBusiness
    {
        // Db
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        // Repo
        private readonly ItemStockRepository _itemStockRepository;

        public ItemStockBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._itemStockRepository = new ItemStockRepository(this._salesAndDistributionDb);
        }

        public IEnumerable<ItemStockDTO> GetItemStocks(long branchId, long orgId, long? categoryId,long? brandId,long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? colorId, string status, string imei, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (categoryId != null && categoryId > 0)
            {
                param += string.Format(@" and stock.CategoryId ={0}", categoryId);
            }
            if (brandId != null && brandId > 0)
            {
                param += string.Format(@" and stock.BrandId ={0}", brandId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and stock.ModelId ={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and stock.WarehouseId ={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and stock.itemTypeId ={0}", warehouseId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and stock.ItemId ={0}", itemId);
            }
            if (colorId != null && colorId > 0)
            {
                param += string.Format(@" and stock.ColorId ={0}", colorId);
            }
            if (status != null && status.Trim() != "")
            {
                param += string.Format(@" and stock.StockStatus ='{0}'", status);
            }
            if (imei != null && imei.Trim() != "")
            {
                param += string.Format(@" and stock.AllIMEI LIKE'%{0}%'", imei);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(stock.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(stock.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(stock.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select stock.StockId,de.DescriptionName 'ModelName',it.ItemName 'ItemTypeName', 
i.ItemName,stock.AllIMEI,stock.StockStatus,stock.Remarks,app.UserName 'EntryUser',stock.EntryDate,cat.CategoryName,b.BrandName,c.ColorName
From [SalesAndDistribution].dbo.tblItemStock stock
Left Join [ControlPanel].dbo.tblApplicationUsers app on stock.EUserId =app.UserId
Left Join [Inventory].dbo.tblDescriptions de on stock.ModelId = de.DescriptionId
Left Join [Inventory].dbo.tblItemTypes it on stock.ItemTypeId =it.ItemId
Left Join [Inventory].dbo.tblItems i on stock.ItemId = i.ItemId
Left Join [Inventory].dbo.tblCategory cat on cat.CategoryId = stock.CategoryId
Left Join [Inventory].dbo.tblBrand b on b.BrandId=stock.BrandId
Left Join [Inventory].dbo.tblColors c on c.ColorId=stock.ColorId
Where 1=1 and stock.OrganizationId ={0} {1}", orgId, Utility.ParamChecker(param));
            return this._salesAndDistributionDb.Db.Database.SqlQuery<ItemStockDTO>(query).ToList();
        }

        public IEnumerable<ModelAndColorWiseItemStockDTO> GetModelAndColorWiseItemStocks(long orgId, long? categoryId, long? brandId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? colorId)
        {
            return this._salesAndDistributionDb.Db.Database.SqlQuery<ModelAndColorWiseItemStockDTO>(string.Format(@"Exec spGetModelAndColorWiseItemStock {0}",orgId)).ToList();
        }

        public int RunProcess(long orgId, long userId, long branchId)
        {
            return this._salesAndDistributionDb.Db.Database.SqlQuery<int>(string.Format(@"Exec dbo.spIMEIProcessForFinishGoodsItem {0},{1},{2}", orgId,userId,branchId)).Single();
        }


    }
}
