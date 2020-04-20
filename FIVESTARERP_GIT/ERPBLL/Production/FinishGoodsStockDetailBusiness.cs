using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class FinishGoodsStockDetailBusiness : IFinishGoodsStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly FinishGoodsStockDetailRepository _finishGoodsStockDetailRepository; // repo
        private readonly FinishGoodsStockInfoRepository _finishGoodsStockInfoRepository; // repo
        private readonly FinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IItemBusiness _itemBusiness;  // BC

        public FinishGoodsStockDetailBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, FinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._finishGoodsStockDetailRepository = new FinishGoodsStockDetailRepository(this._productionDb);
            this._finishGoodsStockInfoRepository = new FinishGoodsStockInfoRepository(this._productionDb);
            this._itemBusiness = itemBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
        }

        public IEnumerable<FinishGoodsStockDetail> GelAllFinishGoodsStockDetailByOrgId(long orgId)
        {
            return _finishGoodsStockDetailRepository.GetAll(fd => fd.OrganizationId == orgId).ToList();
        }

        public bool SaveFinshGoodsStockIn(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId)
        {
            List<FinishGoodsStockDetail> finishGoodsStockDetail = new List<FinishGoodsStockDetail>();
            foreach (var item in finishGoodsStockDetailDTOs)
            {
                FinishGoodsStockDetail stockDetail = new FinishGoodsStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.LineId = item.LineId;
                stockDetail.DescriptionId = item.DescriptionId;

                var finishGoodsStockInfoInDb = _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.LineId == item.LineId && o.DescriptionId == item.DescriptionId).FirstOrDefault();
                if (finishGoodsStockInfoInDb != null)
                {
                    finishGoodsStockInfoInDb.StockInQty += item.Quantity;
                    _finishGoodsStockInfoRepository.Update(finishGoodsStockInfoInDb);
                }
                else
                {
                    FinishGoodsStockInfo finishGoodsStockInfo= new FinishGoodsStockInfo();
                    finishGoodsStockInfo.LineId = item.LineId;
                    finishGoodsStockInfo.WarehouseId = item.WarehouseId;
                    finishGoodsStockInfo.ItemTypeId = item.ItemTypeId;
                    finishGoodsStockInfo.ItemId = item.ItemId;
                    finishGoodsStockInfo.UnitId = stockDetail.UnitId;
                    finishGoodsStockInfo.StockInQty = item.Quantity;
                    finishGoodsStockInfo.StockOutQty = 0;
                    finishGoodsStockInfo.OrganizationId = orgId;
                    finishGoodsStockInfo.EUserId = userId;
                    finishGoodsStockInfo.EntryDate = DateTime.Now;
                    finishGoodsStockInfo.DescriptionId = item.DescriptionId;
                    _finishGoodsStockInfoRepository.Insert(finishGoodsStockInfo);
                }
                finishGoodsStockDetail.Add(stockDetail);
            }
            _finishGoodsStockDetailRepository.InsertAll(finishGoodsStockDetail);
            return _finishGoodsStockDetailRepository.Save();
        }

        public IEnumerable<FinishGoodsStockDetailInfoListDTO> GetFinishGoodsStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            IEnumerable<FinishGoodsStockDetailInfoListDTO> finishGoodsStockDetailInfoListDTOs = new List<FinishGoodsStockDetailInfoListDTO>();
            finishGoodsStockDetailInfoListDTOs = this._productionDb.Db.Database.SqlQuery<FinishGoodsStockDetailInfoListDTO>(QueryForFinishGoodsStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum)).ToList();
            return finishGoodsStockDetailInfoListDTOs;
        }

        private string QueryForFinishGoodsStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (lineId != null && lineId > 0)
            {
                param += string.Format(@" and pl.LineId={0}", lineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(stockStatus) && stockStatus.Trim() != "")
            {
                param += string.Format(@" and fsd.StockStatus='{0}'", stockStatus);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and fsd.RefferenceNumber Like'%{0}%'", refNum);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select fsd.FinishGoodsStockDetailId 'StockDetailId',ISNULL(pl.LineNumber,'') 'LineNumber', ISNULL(de.DescriptionName,'') 'ModelName', 
ISNULL(wh.WarehouseName,'') 'WarehouseName', ISNULL(it.ItemName,'') 'ItemTypeName', ISNULL(i.ItemName,'') 'ItemName',
ISNULL(u.UnitSymbol,'') 'UnitName', fsd.Quantity,fsd.StockStatus,CONVERT(nvarchar(30),fsd.EntryDate,100) 'EntryDate',
fsd.RefferenceNumber
From tblFinishGoodsStockDetail fsd
Left Join tblProductionLines pl on fsd.LineId= pl.LineId
Left Join tblDescriptions de on fsd.DescriptionId= de.DescriptionId
Left Join [Inventory].dbo.[tblWarehouses] wh on fsd.WarehouseId = wh.Id
Left Join [Inventory].dbo.[tblItemTypes] it on fsd.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.[tblItems] i on fsd.ItemId = i.ItemId
Left Join [Inventory].dbo.[tblUnits] u on fsd.UnitId = u.UnitId
Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }

        public IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseDailyProduction(long orgId)
        {
                return this._productionDb.Db.Database.SqlQuery<DashboardLineWiseProductionDTO>(
                string.Format(@"select  l.LineNumber,sum(d.Quantity) as Total from tblFinishGoodsStockDetail d
                inner join tblProductionLines l on d.LineId=l.LineId
                Where d.StockStatus ='Stock-In' And Cast(GETDATE() as date) = Cast(d.EntryDate as date) and d.OrganizationId={0}
                group by l.LineId,l.LineNumber", orgId)).ToList();
        }

        public IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseOverAllProduction(long orgId)
        {
                return this._productionDb.Db.Database.SqlQuery<DashboardLineWiseProductionDTO>(
                string.Format(@"select  l.LineNumber,sum(d.Quantity) as Total from tblFinishGoodsStockDetail d
                inner join tblProductionLines l on d.LineId=l.LineId
                Where d.StockStatus ='Stock-In' and d.OrganizationId={0}
                group by l.LineId,l.LineNumber", orgId)).ToList();
        }
    }
}
