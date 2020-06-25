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
    public class QCItemStockDetailBusiness : IQCItemStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QCItemStockDetailRepository _qCItemStockDetailRepository;
        private readonly QCItemStockInfoRepository _qCItemStockInfoRepository;
        private readonly IQCItemStockInfoBusiness _qCItemStockInfoBusiness;

        public QCItemStockDetailBusiness(IProductionUnitOfWork productionDb, IQCItemStockInfoBusiness qCItemStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._qCItemStockInfoRepository = new QCItemStockInfoRepository(this._productionDb);
            this._qCItemStockDetailRepository = new QCItemStockDetailRepository(this._productionDb);
            this._qCItemStockInfoBusiness = qCItemStockInfoBusiness;
        }

        public IEnumerable<QCItemStockDetail> GetQCItemStockDetails(long orgId)
        {
            return _qCItemStockDetailRepository.GetAll(d => d.OrganizationId == orgId);
        }

        public bool SaveQCItemStockIn(List<QCItemStockDetailDTO> items, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<QCItemStockDetail> stockDetails = new List<QCItemStockDetail>();
            foreach (var item in items)
            {
                QCItemStockDetail stock = new QCItemStockDetail
                {
                    ProductionFloorId = item.ProductionFloorId,
                    QCId = item.QCId,
                    DescriptionId = item.DescriptionId,
                    AssemblyLineId = item.AssemblyLineId,
                    RepairLineId = item.RepairLineId,
                    LabId = item.LabId,
                    PackagingLineId = item.PackagingLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate= DateTime.Now,
                    Remarks =item.Remarks,
                    StockStatus = StockStatus.StockIn
                };

                var stockInfoInDb = _qCItemStockInfoBusiness.GetQCItemStockInfById(item.QCId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);
                if (stockInfoInDb != null)
                {
                    stockInfoInDb.Quantity += item.Quantity;
                    if(item.RepairLineId != null && item.RepairLineId > 0)
                    {
                        stockInfoInDb.RepairQty -= item.Quantity;
                    }
                    else if(item.LabId != null && item.LabId > 0)
                    {
                        stockInfoInDb.LabQty -= item.Quantity;
                    }
                    stockInfoInDb.UpUserId = userId;
                    this._qCItemStockInfoRepository.Update(stockInfoInDb);
                }
                else
                {
                    QCItemStockInfo info = new QCItemStockInfo()
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        DescriptionId = item.DescriptionId,
                        QCId = item.QCId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        RepairQty = 0,
                        LabQty = 0,
                        PackagingQty = 0,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = ""
                    };
                    this._qCItemStockInfoRepository.Insert(info);
                }

                stockDetails.Add(stock);
            }
            if(stockDetails.Count > 0)
            {
                this._qCItemStockDetailRepository.InsertAll(stockDetails);
                IsSuccess= this._qCItemStockDetailRepository.Save();
            }
            
            return IsSuccess;
        }

        public bool SaveQCItemStockOut(List<QCItemStockDetailDTO> items, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<QCItemStockDetail> stockDetails = new List<QCItemStockDetail>();
            foreach (var item in items)
            {
                QCItemStockDetail stock = new QCItemStockDetail
                {
                    ProductionFloorId = item.ProductionFloorId,
                    QCId = item.QCId,
                    DescriptionId = item.DescriptionId,
                    AssemblyLineId = item.AssemblyLineId,
                    RepairLineId = item.RepairLineId,
                    LabId = item.LabId,
                    PackagingLineId = item.PackagingLineId,
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

                var stockInfoInDb = _qCItemStockInfoBusiness.GetQCItemStockInfById(item.QCId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);

                stockInfoInDb.Quantity -= item.Quantity;
                if (item.RepairLineId != null && item.RepairLineId > 0)
                {
                    stockInfoInDb.RepairQty += item.Quantity;
                }
                else if (item.LabId != null && item.LabId > 0)
                {
                    stockInfoInDb.LabQty += item.Quantity;
                }
                else if (item.PackagingLineId != null && item.PackagingLineId > 0)
                {
                    stockInfoInDb.PackagingQty += item.Quantity;
                }
                stockInfoInDb.UpUserId = userId;
                _qCItemStockInfoRepository.Update(stockInfoInDb);
                stockDetails.Add(stock);
            }
            if (stockDetails.Count > 0)
            {
                _qCItemStockDetailRepository.InsertAll(stockDetails);
                IsSuccess = _qCItemStockDetailRepository.Save();
            }

            return IsSuccess;
        }
    }
}
