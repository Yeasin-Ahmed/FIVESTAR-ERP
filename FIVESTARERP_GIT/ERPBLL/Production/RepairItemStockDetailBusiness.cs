using ERPBLL.Common;
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
    public class RepairItemStockDetailBusiness : IRepairItemStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IRepairItemStockInfoBusiness _repairItemStockInfoBusiness;
        private readonly RepairItemStockDetailRepository _repairItemStockDetailRepository;
        private readonly RepairItemStockInfoRepository _repairItemStockInfoRepository;

        public RepairItemStockDetailBusiness(IProductionUnitOfWork productionDb, IRepairItemStockInfoBusiness repairItemStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._repairItemStockInfoBusiness = repairItemStockInfoBusiness;
            this._repairItemStockDetailRepository = new RepairItemStockDetailRepository(this._productionDb);
            this._repairItemStockInfoRepository = new RepairItemStockInfoRepository(this._productionDb);
        }
        public IEnumerable<RepairItemStockDetail> GetQCItemStockDetails(long orgId)
        {
            return _repairItemStockDetailRepository.GetAll(d => d.OrganizationId == orgId);
        }
        public bool SaveRepairItemStockIn(List<RepairItemStockDetailDTO> items, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<RepairItemStockDetail> stockDetails = new List<RepairItemStockDetail>();
            foreach (var item in items)
            {
                RepairItemStockDetail stock = new RepairItemStockDetail
                {
                    ProductionFloorId = item.ProductionFloorId,
                    AssemblyLineId = item.AssemblyLineId,
                    QCId = item.QCId,
                    DescriptionId = item.DescriptionId,
                    RepairLineId = item.RepairLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockIn
                };
                var stockInfoInDb = _repairItemStockInfoBusiness.GetRepairItem(item.AssemblyLineId.Value,item.QCId.Value, item.RepairLineId.Value,item.DescriptionId.Value, item.ItemId.Value, orgId);
                if (stockInfoInDb != null)
                {
                    stockInfoInDb.Quantity += item.Quantity;
                    stockInfoInDb.UpUserId = userId;
                    this._repairItemStockInfoRepository.Update(stockInfoInDb);
                }
                else
                {
                    RepairItemStockInfo info = new RepairItemStockInfo()
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        RepairLineId = item.RepairLineId,
                        DescriptionId = item.DescriptionId,
                        QCId = item.QCId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        QCQty = 0,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = ""
                    };
                    this._repairItemStockInfoRepository.Insert(info);
                }
                stockDetails.Add(stock);
            }
            if (stockDetails.Count > 0)
            {
                this._repairItemStockDetailRepository.InsertAll(stockDetails);
                IsSuccess = this._repairItemStockDetailRepository.Save();
            }
            return IsSuccess;
        }
        public bool SaveRepairItemStockOut(List<RepairItemStockDetailDTO> items, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<RepairItemStockDetail> stockDetails = new List<RepairItemStockDetail>();
            foreach (var item in items)
            {
                RepairItemStockDetail stock = new RepairItemStockDetail() {
                    ProductionFloorId = item.ProductionFloorId,
                    QCId = item.QCId,
                    DescriptionId = item.DescriptionId,
                    RepairLineId = item.RepairLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockOut
                };

                var stockInfoInDb = _repairItemStockInfoBusiness.GetRepairItem(item.AssemblyLineId.Value,item.QCId.Value, item.RepairLineId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);
                stockInfoInDb.Quantity -= item.Quantity;
                stockInfoInDb.QCQty += item.Quantity;
                stockInfoInDb.UpUserId = userId;
                _repairItemStockInfoRepository.Update(stockInfoInDb);
                stockDetails.Add(stock);
            }
            if(stockDetails.Count > 0)
            {
                this._repairItemStockDetailRepository.InsertAll(stockDetails);
                IsSuccess = this._repairItemStockDetailRepository.Save();
            }
            return IsSuccess;
        }
        public async Task<bool> SaveRepairItemStockOutAsync(List<RepairItemStockDetailDTO> items, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<RepairItemStockDetail> stockDetails = new List<RepairItemStockDetail>();
            foreach (var item in items)
            {
                RepairItemStockDetail stock = new RepairItemStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    QCId = item.QCId,
                    DescriptionId = item.DescriptionId,
                    RepairLineId = item.RepairLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockOut
                };

                var stockInfoInDb = await _repairItemStockInfoBusiness.GetRepairItemAsync(item.AssemblyLineId.Value,item.QCId.Value, item.RepairLineId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);
                //stockInfoInDb.Quantity -= item.Quantity;
                stockInfoInDb.QCQty += item.Quantity;
                stockInfoInDb.UpUserId = userId;
                _repairItemStockInfoRepository.Update(stockInfoInDb);
                stockDetails.Add(stock);
            }
            if (stockDetails.Count > 0)
            {
                this._repairItemStockDetailRepository.InsertAll(stockDetails);
                IsSuccess = await this._repairItemStockDetailRepository.SaveAsync();
            }
            return IsSuccess;
            
        }
    }
}
