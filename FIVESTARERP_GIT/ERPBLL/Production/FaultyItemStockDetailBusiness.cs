using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DTOModel;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class FaultyItemStockDetailBusiness : IFaultyItemStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly FaultyItemStockInfoRepository _faultyItemStockInfoRepository;
        private readonly FaultyItemStockDetailRepository _faultyItemStockDetailRepository;
        private readonly IFaultyItemStockInfoBusiness _faultyItemStockInfoBusiness;
        public FaultyItemStockDetailBusiness(IProductionUnitOfWork productionDb, IFaultyItemStockInfoBusiness faultyItemStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._faultyItemStockInfoRepository = new FaultyItemStockInfoRepository(this._productionDb);
            this._faultyItemStockDetailRepository = new FaultyItemStockDetailRepository(this._productionDb);
            this._faultyItemStockInfoBusiness = faultyItemStockInfoBusiness;
        }
        public IEnumerable<FaultyItemStockDetail> GetFaultyItemStocks(long orgId)
        {
            return _faultyItemStockDetailRepository.GetAll(f => f.OrganizationId == orgId);
        }
        public bool SaveFaultyItemStockIn(List<FaultyItemStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<FaultyItemStockDetail> faultyItemStocks = new List<FaultyItemStockDetail>();
            foreach (var item in stockDetails)
            {
                FaultyItemStockDetail faultyItem = new FaultyItemStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    QCId = item.QCId,
                    RepairLineId = item.RepairLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId =  item.UnitId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = "Stock In By Repair Item Stock",
                    StockStatus = StockStatus.StockIn,
                    EntryDate = DateTime.Now
                };
                var stockInfoInDb = this._faultyItemStockInfoBusiness.GetFaultyItemStockInfoByQCandRepairandItem(item.QCId.Value, item.RepairLineId.Value, item.ItemId.Value, orgId);

                if(stockInfoInDb != null)
                {
                    stockInfoInDb.StockInQty += item.Quantity;
                    stockInfoInDb.UpdateDate = DateTime.Now;
                    stockInfoInDb.UpUserId = userId;
                    _faultyItemStockInfoRepository.Update(stockInfoInDb);
                }
                else
                {
                    FaultyItemStockInfo stockInfo = new FaultyItemStockInfo()
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        DescriptionId = item.DescriptionId,
                        QCId = item.QCId,
                        RepairLineId = item.RepairLineId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                        Remarks = item.Remarks,
                        EntryDate = DateTime.Now
                    };
                    _faultyItemStockInfoRepository.Insert(stockInfo);
                }

                faultyItemStocks.Add(faultyItem);
            }
            if(faultyItemStocks.Count > 0)
            {
                this._faultyItemStockDetailRepository.InsertAll(faultyItemStocks);
                IsSuccess = _faultyItemStockDetailRepository.Save();
            }
            return  IsSuccess;
        }
        public bool SaveFaultyItemStockOut(List<FaultyItemStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<FaultyItemStockDetail> faultyItemStocks = new List<FaultyItemStockDetail>();
            foreach (var item in stockDetails)
            {
                FaultyItemStockDetail faultyItem = new FaultyItemStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    QCId = item.QCId,
                    RepairLineId = item.RepairLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockOut,
                    EntryDate = DateTime.Now
                };
                var stockInfoInDb = this._faultyItemStockInfoBusiness.GetFaultyItemStockInfoByQCandRepairandItem(item.QCId.Value, item.RepairLineId.Value, item.ItemId.Value, orgId);
                    stockInfoInDb.StockOutQty += item.Quantity;
                    stockInfoInDb.UpdateDate = DateTime.Now;
                    stockInfoInDb.UpUserId = userId;
                    _faultyItemStockInfoRepository.Update(stockInfoInDb);
                faultyItemStocks.Add(faultyItem);
            }
            if (faultyItemStocks.Count > 0)
            {
                this._faultyItemStockDetailRepository.InsertAll(faultyItemStocks);
                IsSuccess=_faultyItemStockDetailRepository.Save();
            }
            return IsSuccess; 
        }
    }
}
