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
    public class TransferFromQCInfoBusiness : ITransferFromQCInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferFromQCInfoRepository _transferFromQCInfoRepository;
        private readonly ITransferFromQCDetailBusiness _transferFromQCDetailBusiness;
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly IQCItemStockDetailBusiness _qCItemStockDetailBusiness;
        private readonly IRepairItemStockDetailBusiness _repairItemStockDetailBusiness;
        private readonly IPackagingItemStockDetailBusiness _packagingItemStockDetailBusiness;

        public TransferFromQCInfoBusiness(IProductionUnitOfWork productionDb, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IItemBusiness itemBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, ITransferFromQCDetailBusiness transferFromQCDetailBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, IQCItemStockDetailBusiness qCItemStockDetailBusiness, IRepairItemStockDetailBusiness repairItemStockDetailBusiness, IPackagingItemStockDetailBusiness packagingItemStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._transferFromQCInfoRepository = new TransferFromQCInfoRepository(this._productionDb);
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._transferFromQCDetailBusiness = transferFromQCDetailBusiness;
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
            this._qCItemStockDetailBusiness = qCItemStockDetailBusiness;
            this._repairItemStockDetailBusiness = repairItemStockDetailBusiness;
            this._packagingItemStockDetailBusiness = packagingItemStockDetailBusiness;
        }

        public TransferFromQCInfo GetTransferFromQCInfoById(long transferId, long orgId)
        {
            return _transferFromQCInfoRepository.GetOneByOrg(t => t.TFQInfoId == transferId && t.OrganizationId == orgId);
        }
        public IEnumerable<TransferFromQCInfo> GetTransferFromQCInfos(long orgId)
        {
            return _transferFromQCInfoRepository.GetAll(t => t.OrganizationId == orgId);
        }
        public bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId)
        {
            bool IsSuccess = false;
            var transferInDb = GetTransferFromQCInfoById(transferId, orgId);
            if (transferInDb != null && transferInDb.StateStatus == RequisitionStatus.Approved)
            {
                transferInDb.StateStatus = RequisitionStatus.Accepted;
                transferInDb.UpUserId = userId;
                transferInDb.UpdateDate = DateTime.Now;
                _transferFromQCInfoRepository.Update(transferInDb);
                var details = _transferFromQCDetailBusiness.GetTransferFromQCDetailByInfo(transferId, orgId);
                if (transferInDb.TransferFor == "Packaging Line")
                {
                    List<PackagingLineStockDetailDTO> stockDetails = new List<PackagingLineStockDetailDTO>();
                    List<PackagingItemStockDetailDTO> packagingItemStocks = new List<PackagingItemStockDetailDTO>() {
                        new PackagingItemStockDetailDTO(){

                            ProductionFloorId = transferInDb.LineId,
                            DescriptionId = transferInDb.DescriptionId,
                            PackagingLineId = transferInDb.PackagingLineId,
                            QCId = transferInDb.QCLineId,
                            WarehouseId = transferInDb.WarehouseId,
                            ItemTypeId = transferInDb.ItemTypeId,
                            ItemId = transferInDb.ItemId,
                            Quantity = transferInDb.ForQty.Value,
                            OrganizationId= orgId,
                            EUserId= userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockIn,
                            Remarks="Item transfer from QC",
                            ReferenceNumber = transferInDb.TransferCode
                        }
                    };

                    foreach (var item in details)
                    {
                        PackagingLineStockDetailDTO packaging = new PackagingLineStockDetailDTO()
                        {
                            QCLineId = transferInDb.QCLineId.Value,
                            PackagingLineId = transferInDb.PackagingLineId,
                            DescriptionId = transferInDb.DescriptionId,
                            WarehouseId = item.WarehouseId.Value,
                            ItemTypeId = item.ItemTypeId.Value,
                            ItemId = item.ItemId.Value,
                            UnitId = item.UnitId,
                            ProductionLineId = transferInDb.LineId.Value,
                            Quantity = item.Quantity,
                            Remarks = "Stock In By QC (" + transferInDb.TransferCode + ")",
                            OrganizationId = orgId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockIn,
                            RefferenceNumber = transferInDb.TransferCode
                        };
                        stockDetails.Add(packaging);
                    }
                    if (_transferFromQCInfoRepository.Save())
                    {
                        if (_packagingLineStockDetailBusiness.SavePackagingLineStockIn(stockDetails, userId, orgId))
                        {
                            IsSuccess = _packagingItemStockDetailBusiness.SavePackagingItemStockIn(packagingItemStocks, userId, orgId);
                        }
                    }
                }
                else
                {
                    if (transferInDb.TransferFor == "Repair Line")
                    {
                        List<RepairLineStockDetailDTO> stockDetails = new List<RepairLineStockDetailDTO>();

                        List<RepairItemStockDetailDTO> repairStocks = new List<RepairItemStockDetailDTO>()
                        {
                            new RepairItemStockDetailDTO()
                            {
                                ProductionFloorId = transferInDb.LineId,
                                DescriptionId = transferInDb.DescriptionId,
                                QCId = transferInDb.QCLineId,
                                RepairLineId = transferInDb.RepairLineId,
                                WarehouseId= transferInDb.WarehouseId,
                                ItemTypeId = transferInDb.ItemTypeId,
                                ItemId = transferInDb.ItemId,
                                OrganizationId= orgId,
                                EUserId = userId,
                                Quantity = transferInDb.ForQty.Value,
                                StockStatus = StockStatus.StockIn,
                                ReferenceNumber=transferInDb.TransferCode,
                                Remarks = transferInDb.RepairTransferReason
                            }
                        };
                        foreach (var item in details)
                        {
                            RepairLineStockDetailDTO repair = new RepairLineStockDetailDTO()
                            {
                                QCLineId = transferInDb.QCLineId.Value,
                                RepairLineId = transferInDb.RepairLineId,
                                DescriptionId = transferInDb.DescriptionId,
                                WarehouseId = item.WarehouseId.Value,
                                ItemTypeId = item.ItemTypeId.Value,
                                ItemId = item.ItemId.Value,
                                UnitId = item.UnitId,
                                ProductionLineId = transferInDb.LineId.Value,
                                Quantity = item.Quantity,
                                Remarks = transferInDb.RepairTransferReason,
                                OrganizationId = orgId,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                StockStatus = StockStatus.StockIn,
                                RefferenceNumber = transferInDb.TransferCode
                            };
                            stockDetails.Add(repair);
                        }
                        if (_transferFromQCInfoRepository.Save())
                        {
                            if (_repairLineStockDetailBusiness.SaveRepairLineStockIn(stockDetails, userId, orgId))
                            {
                                IsSuccess = _repairItemStockDetailBusiness.SaveRepairItemStockIn(repairStocks, userId, orgId);
                            }
                        }
                    }
                }
            }
            return IsSuccess;
        }
        public bool SaveTransfer(TransferFromQCInfoDTO infoDto, List<TransferFromQCDetailDTO> detailDto, long userId, long orgId)
        {
            bool IsSuccess = false;
            TransferFromQCInfo info = new TransferFromQCInfo
            {
                TransferCode = ("TFQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                LineId = infoDto.LineId,
                DescriptionId = infoDto.DescriptionId,
                WarehouseId = infoDto.WarehouseId,
                QCLineId = infoDto.QCLineId,
                RepairLineId = infoDto.RepairLineId,
                PackagingLineId = infoDto.PackagingLineId,
                TransferFor = infoDto.TransferFor,
                RepairTransferReason = infoDto.RepairTransferReason,
                StateStatus = RequisitionStatus.Approved,
                Remarks = infoDto.Remarks,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                ItemTypeId = infoDto.ItemTypeId,
                ItemId = infoDto.ItemId,
                ForQty = infoDto.ForQty
            };
            List<QCItemStockDetailDTO> qcItemStocks = new List<QCItemStockDetailDTO>()
            {
                new QCItemStockDetailDTO(){
                    ProductionFloorId =info.LineId,
                    QCId= info.QCLineId,
                    DescriptionId = info.DescriptionId,
                    RepairLineId = info.RepairLineId,
                    PackagingLineId = info.PackagingLineId,
                    WarehouseId = info.WarehouseId,
                    ItemTypeId = info.ItemTypeId,
                    ItemId =info.ItemId,
                    Quantity = info.ForQty.Value,
                    OrganizationId = orgId,
                    EUserId = userId,
                    StockStatus= StockStatus.StockOut,
                    EntryDate = DateTime.Now,
                    ReferenceNumber= info.TransferCode,
                }
            };
            List<TransferFromQCDetail> listOfDetail = new List<TransferFromQCDetail>();
            List<QualityControlLineStockDetailDTO> stockDetail = new List<QualityControlLineStockDetailDTO>();

            foreach (var item in detailDto)
            {
                TransferFromQCDetail detail = new TransferFromQCDetail
                {
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = _itemBusiness.GetItemOneByOrgId(item.ItemId.Value, orgId).UnitId,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now
                };
                listOfDetail.Add(detail);
                QualityControlLineStockDetailDTO stock = new QualityControlLineStockDetailDTO
                {
                    DescriptionId = info.DescriptionId,
                    ProductionLineId = info.LineId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = detail.UnitId,
                    WarehouseId = item.WarehouseId,
                    QCLineId = info.QCLineId,
                    RefferenceNumber = info.TransferCode,
                    Quantity = item.Quantity,
                    Remarks = "Stock Out For " + info.TransferFor + " (" + info.TransferCode + ")",
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    StockStatus = StockStatus.StockOut
                };
                stockDetail.Add(stock);
            }
            info.TransferFromQCDetails = listOfDetail;
            _transferFromQCInfoRepository.Insert(info);
            if (_transferFromQCInfoRepository.Save())
            {
                // QC Raw Material Stocks
                if (_qCLineStockDetailBusiness.SaveQCLineStockOut(stockDetail, userId, orgId, "Stock Out By QC Transfer"))
                {
                    //QC Item Stock
                    IsSuccess = _qCItemStockDetailBusiness.SaveQCItemStockOut(qcItemStocks, userId, orgId);
                }
            }
            return IsSuccess;
        }

        public IEnumerable<TransferFromQCInfo> GetTransferFromQCInfoByTransferFor(string transferFor, long orgId)
        {
            return _transferFromQCInfoRepository.GetAll(t => t.TransferFor == transferFor && t.OrganizationId == orgId);
        }
    }
}
