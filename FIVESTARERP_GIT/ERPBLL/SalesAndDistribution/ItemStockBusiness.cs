using ERPBLL.Common;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DomainModels;
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

        public IEnumerable<ItemStockDTO> GetItemStocks(long branchId, long orgId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? colorId, string status, string imei, string fromDate, string toDate)
        {
            return this._salesAndDistributionDb.Db.Database.SqlQuery<ItemStockDTO>(QueryForItemStocks(branchId,orgId,modelId,warehouseId,itemTypeId,itemId,colorId,status,imei,fromDate,toDate)).ToList();
        }

        public int RunProcess(long orgId, long userId, long branchId)
        {
            return this._salesAndDistributionDb.Db.Database.SqlQuery<int>(string.Format(@"Exec [SalesAndDistribution].dbo.spIMEIProcessForFinishGoodsItem {0},{1},{2}", orgId,userId,branchId)).Single();
        }

        private string QueryForItemStocks(long branchId, long orgId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? colorId, string status, string imei, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (branchId > 0)
            {

            }
            param += string.Format(@" and stock.OrganizationId ={0}", orgId);

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
            if (status != null && status.Trim() !="")
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
i.ItemName,stock.AllIMEI,stock.StockStatus,stock.Remarks,app.UserName 'EntryUser',stock.EntryDate
From [SalesAndDistribution].dbo.tblItemStock stock
Left Join [ControlPanel].dbo.tblApplicationUsers app on stock.EUserId =app.UserId
Inner Join [Inventory].dbo.tblDescriptions de on stock.ModelId = de.DescriptionId
Inner Join [Inventory].dbo.tblItemTypes it on stock.ItemTypeId =it.ItemId
Inner Join [Inventory].dbo.tblItems i on stock.ItemId = i.ItemId
Where 1=1 {0}",Utility.ParamChecker(param));
            return query;
        }

    }
}
