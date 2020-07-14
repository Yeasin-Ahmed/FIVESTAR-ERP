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
    public class QCLineStockDetailBusiness : IQCLineStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;

        private readonly QualityControlLineStockDetailRepository _qualityControlLineStockDetailRepository;
        private readonly QualityControlLineStockInfoRepository _qualityControlLineStockInfoRepository;

        private readonly ITransferStockToQCDetailBusiness _transferStockToQCDetailBusiness;
        private readonly ITransferStockToQCInfoBusiness _transferStockToQCInfoBusiness;

        private readonly IQCLineStockInfoBusiness _qCLineStockInfoBusiness;
        private readonly IQCItemStockDetailBusiness _qCItemStockDetailBusiness;
        

        public QCLineStockDetailBusiness(IProductionUnitOfWork productionDb, ITransferStockToQCDetailBusiness transferStockToQCDetailBusiness, ITransferStockToQCInfoBusiness transferStockToQCInfoBusiness, IQCLineStockInfoBusiness qCLineStockInfoBusiness, IQCItemStockDetailBusiness qCItemStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._qualityControlLineStockDetailRepository = new QualityControlLineStockDetailRepository(this._productionDb);
            this._qualityControlLineStockInfoRepository = new QualityControlLineStockInfoRepository(this._productionDb);
            this._transferStockToQCDetailBusiness = transferStockToQCDetailBusiness;
            this._transferStockToQCInfoBusiness = transferStockToQCInfoBusiness;
            this._qCLineStockInfoBusiness = qCLineStockInfoBusiness;
            this._qCItemStockDetailBusiness = qCItemStockDetailBusiness;
        }

        public IEnumerable<QualityControlLineStockDetail> GetQCLineStockDetails(long orgId)
        {
            throw new NotImplementedException();
        }

        public bool SaveQCLineStockIn(List<QualityControlLineStockDetailDTO> qcLineStockDetailDTO, long userId, long orgId)
        {
            List<QualityControlLineStockDetail> qcLineStockDetail = new List<QualityControlLineStockDetail>();
            foreach (var item in qcLineStockDetailDTO)
            {
                QualityControlLineStockDetail stockDetail = new QualityControlLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.QCLineId = item.QCLineId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var qcStockInfo = _qCLineStockInfoBusiness.GetQCLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.QCLineId == item.QCLineId).FirstOrDefault();
                if (qcStockInfo != null)
                {
                    qcStockInfo.StockInQty += item.Quantity;
                    _qualityControlLineStockInfoRepository.Update(qcStockInfo);
                }
                else
                {
                   QualityControlLineStockInfo info = new QualityControlLineStockInfo();
                    info.AssemblyLineId = item.AssemblyLineId;
                    info.ProductionLineId = item.ProductionLineId;
                    info.WarehouseId = item.WarehouseId;
                    info.DescriptionId = item.DescriptionId;
                    info.QCLineId = item.QCLineId;

                    info.ItemTypeId = item.ItemTypeId;
                    info.ItemId = item.ItemId;
                    info.UnitId = stockDetail.UnitId;
                    info.StockInQty = item.Quantity;
                    info.StockOutQty = 0;
                    info.OrganizationId = orgId;
                    info.EUserId = userId;
                    info.EntryDate = DateTime.Now;

                    _qualityControlLineStockInfoRepository.Insert(info);
                }
                qcLineStockDetail.Add(stockDetail);
            }
            _qualityControlLineStockDetailRepository.InsertAll(qcLineStockDetail);
            return _qualityControlLineStockDetailRepository.Save();
        }
        // Async Method
        public async Task<bool> SaveQCLineStockInAsync(List<QualityControlLineStockDetailDTO> qualityControlStockDetailDto, long userId, long orgId)
        {
            List<QualityControlLineStockDetail> qcLineStockDetail = new List<QualityControlLineStockDetail>();
            foreach (var item in qualityControlStockDetailDto)
            {
                QualityControlLineStockDetail stockDetail = new QualityControlLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.QCLineId = item.QCLineId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var qcStockInfo = await _qCLineStockInfoBusiness.GetQCLineStockInfoByQCAndItemAndModelIdAsync(item.QCLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);
                //_qCLineStockInfoBusiness.GetQCLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.QCLineId == item.QCLineId).FirstOrDefault();
                if (qcStockInfo != null)
                {
                    qcStockInfo.StockInQty += item.Quantity;
                    _qualityControlLineStockInfoRepository.Update(qcStockInfo);
                }
                else
                {
                    QualityControlLineStockInfo info = new QualityControlLineStockInfo();
                    info.AssemblyLineId = item.AssemblyLineId;
                    info.ProductionLineId = item.ProductionLineId;
                    info.WarehouseId = item.WarehouseId;
                    info.DescriptionId = item.DescriptionId;
                    info.QCLineId = item.QCLineId;

                    info.ItemTypeId = item.ItemTypeId;
                    info.ItemId = item.ItemId;
                    info.UnitId = stockDetail.UnitId;
                    info.StockInQty = item.Quantity;
                    info.StockOutQty = 0;
                    info.OrganizationId = orgId;
                    info.EUserId = userId;
                    info.EntryDate = DateTime.Now;

                    _qualityControlLineStockInfoRepository.Insert(info);
                }
                qcLineStockDetail.Add(stockDetail);
            }
            _qualityControlLineStockDetailRepository.InsertAll(qcLineStockDetail);
            return await _qualityControlLineStockDetailRepository.SaveAsync();
        }

        public bool SaveQCLineStockOut(List<QualityControlLineStockDetailDTO> qcLineStockDetailDTO, long userId, long orgId, string flag)
        {
            List<QualityControlLineStockDetail> qcLineStockDetail = new List<QualityControlLineStockDetail>();
            foreach (var item in qcLineStockDetailDTO)
            {
                QualityControlLineStockDetail stockDetail = new QualityControlLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.QCLineId = item.QCLineId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockOut;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var qcStockInfo = _qCLineStockInfoBusiness.GetQCLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.QCLineId == item.QCLineId).FirstOrDefault();
               
                qcStockInfo.StockOutQty += item.Quantity;
                _qualityControlLineStockInfoRepository.Update(qcStockInfo);
                qcLineStockDetail.Add(stockDetail);
            }
            _qualityControlLineStockDetailRepository.InsertAll(qcLineStockDetail);
            return _qualityControlLineStockDetailRepository.Save();
        }

        // Async Method
        public async Task<bool> SaveQCLineStockOutAsync(List<QualityControlLineStockDetailDTO> qualityControlStockDetailDto, long userId, long orgId, string flag)
        {
            List<QualityControlLineStockDetail> qcLineStockDetail = new List<QualityControlLineStockDetail>();
            foreach (var item in qualityControlStockDetailDto)
            {
                QualityControlLineStockDetail stockDetail = new QualityControlLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.QCLineId = item.QCLineId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockOut;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var qcStockInfo = await _qCLineStockInfoBusiness.GetQCLineStockInfoByQCAndItemAndModelIdAsync(item.QCLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                //_qCLineStockInfoBusiness.GetQCLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.QCLineId == item.QCLineId).FirstOrDefault();

                qcStockInfo.StockOutQty += item.Quantity;
                _qualityControlLineStockInfoRepository.Update(qcStockInfo);
                qcLineStockDetail.Add(stockDetail);
            }
            _qualityControlLineStockDetailRepository.InsertAll(qcLineStockDetail);
            return await _qualityControlLineStockDetailRepository.SaveAsync();
        }

        public bool SaveQCStockInByAssemblyLine(long transferId, string status, long orgId, long userId)
        {
            bool IsSuccess = false;
            if(transferId > 0)
            {
                var info = _transferStockToQCInfoBusiness.GetStockToQCInfoById(transferId, orgId);
                if(info != null && info.StateStatus == RequisitionStatus.Approved)
                {
                    var details = _transferStockToQCDetailBusiness.GetTransferStockToQCDetailByInfo(transferId, orgId);
                    if (details.Count() > 0)
                    {
                        List<QualityControlLineStockDetailDTO> stockDetails = new List<QualityControlLineStockDetailDTO>();
                        List<QCItemStockDetailDTO> qcItems = new List<QCItemStockDetailDTO>();
                        foreach (var item in details)
                        {
                            QualityControlLineStockDetailDTO detailItem = new QualityControlLineStockDetailDTO()
                            {
                                AssemblyLineId = info.AssemblyId,
                                QCLineId = info.QCLineId,
                                ProductionLineId = info.LineId,
                                DescriptionId = info.DescriptionId,
                                WarehouseId = item.WarehouseId,
                                ItemTypeId= item.ItemTypeId,
                                ItemId = item.ItemId,
                                UnitId = item.UnitId,
                                Quantity = item.Quantity,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                OrganizationId = orgId,
                                StockStatus = StockStatus.StockIn,
                                RefferenceNumber = info.TransferCode,
                                Remarks = "Stock In by Assembly Line",
                            };
                            stockDetails.Add(detailItem);
                        }

                        QCItemStockDetailDTO qcItem = new QCItemStockDetailDTO
                        {
                            ProductionFloorId = info.LineId,
                            AssemblyLineId = info.AssemblyId,
                            QCId = info.QCLineId,
                            WarehouseId = info.WarehouseId,
                            ItemTypeId = info.ItemTypeId,
                            ItemId = info.ItemId,
                            Quantity = info.ForQty.Value,
                            ReferenceNumber = info.TransferCode,
                            DescriptionId= info.DescriptionId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            OrganizationId = orgId,
                            StockStatus  = StockStatus.StockIn,
                        };
                        qcItems.Add(qcItem);

                        if (_transferStockToQCInfoBusiness.SaveTransferInfoStateStatus(transferId, status, userId, orgId))
                        {
                            if (SaveQCLineStockIn(stockDetails, userId, orgId)) {
                                IsSuccess= _qCItemStockDetailBusiness.SaveQCItemStockIn(qcItems, userId, orgId);
                            }
                            // QCItem Stockss
                        }
                    }
                    // details
                }// info
            }
            return IsSuccess;
        }
    }
}
