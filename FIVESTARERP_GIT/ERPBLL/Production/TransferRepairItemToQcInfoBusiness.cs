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
    public class TransferRepairItemToQcInfoBusiness : ITransferRepairItemToQcInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferRepairItemToQcInfoRepository _transferRepairItemToQcInfoRepository;
        private readonly ITransferRepairItemToQcDetailBusiness _transferRepairItemToQcDetailBusiness;
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly IRepairItemStockDetailBusiness _repairItemStockDetailBusiness;
        private readonly IQCItemStockDetailBusiness _qcItemStockDetailBusiness;

        public TransferRepairItemToQcInfoBusiness(IProductionUnitOfWork productionDb, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IItemBusiness itemBusiness, ITransferRepairItemToQcDetailBusiness transferRepairItemToQcDetailBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, IRepairItemStockDetailBusiness repairItemStockDetailBusiness, IQCItemStockDetailBusiness qcItemStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._transferRepairItemToQcInfoRepository = new TransferRepairItemToQcInfoRepository(this._productionDb);
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._transferRepairItemToQcDetailBusiness = transferRepairItemToQcDetailBusiness;
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
            this._repairItemStockDetailBusiness = repairItemStockDetailBusiness;
            this._qcItemStockDetailBusiness = qcItemStockDetailBusiness;
        }

        public TransferRepairItemToQcInfo GetTransferRepairItemToQcInfoById(long transferId, long orgId)
        {
            return _transferRepairItemToQcInfoRepository.GetOneByOrg(t => t.TRQInfoId == transferId && t.OrganizationId == orgId);
        }
        public IEnumerable<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfos(long orgId)
        {
            var data = _transferRepairItemToQcInfoRepository.GetAll(t => t.OrganizationId == orgId).ToList();
            return data;
        }
        public bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId)
        {
            bool IsSuccess = false;
            var transferInDb = GetTransferRepairItemToQcInfoById(transferId, orgId);
            if (transferInDb != null && transferInDb.StateStatus == RequisitionStatus.Approved)
            {
                transferInDb.StateStatus = RequisitionStatus.Accepted;
                transferInDb.UpUserId = userId;
                transferInDb.UpdateDate = DateTime.Now;
                _transferRepairItemToQcInfoRepository.Update(transferInDb);
                string flag = (transferInDb.RepairLineId.HasValue && transferInDb.RepairLineId.Value > 0) ? "Repair" : "";
                var details = _transferRepairItemToQcDetailBusiness.GetTransferRepairItemToQcDetailByInfo(transferId, orgId);
                List<QualityControlLineStockDetailDTO> stockDetails = new List<QualityControlLineStockDetailDTO>();
                List<QCItemStockDetailDTO> qcItemStocks = new List<QCItemStockDetailDTO>()
                {
                    new QCItemStockDetailDTO()
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
                        StockStatus = StockStatus.StockOut,
                        ReferenceNumber=transferInDb.TransferCode,
                        Remarks = "Transfer Item To QC",
                        Flag = flag
                    }
                };
                foreach (var item in details)
                {
                    QualityControlLineStockDetailDTO stock = new QualityControlLineStockDetailDTO()
                    {
                        QCLineId = transferInDb.QCLineId.Value,
                        DescriptionId = transferInDb.DescriptionId,
                        WarehouseId = item.WarehouseId.Value,
                        ItemTypeId = item.ItemTypeId.Value,
                        ItemId = item.ItemId.Value,
                        UnitId = item.UnitId,
                        ProductionLineId = transferInDb.LineId.Value,
                        Quantity = item.Quantity,
                        Remarks = "Stock In By Repair (" + transferInDb.TransferCode + ")",
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        StockStatus = StockStatus.StockIn,
                        RefferenceNumber = transferInDb.TransferCode
                    };
                    stockDetails.Add(stock);
                }
                if (_transferRepairItemToQcInfoRepository.Save())
                {
                    if (_qCLineStockDetailBusiness.SaveQCLineStockIn(stockDetails, userId, orgId)) {
                        IsSuccess= _qcItemStockDetailBusiness.SaveQCItemStockIn(qcItemStocks, userId, orgId);
                    }
                }
            }
            return IsSuccess;
        }
        public bool SaveTransfer(TransferRepairItemToQcInfoDTO infoDto, List<TransferRepairItemToQcDetailDTO> detailDto, long userId, long orgId)
        {
            bool IsSuccess = false;
            TransferRepairItemToQcInfo info = new TransferRepairItemToQcInfo
            {
                TransferCode = ("TRQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                LineId = infoDto.LineId,
                DescriptionId = infoDto.DescriptionId,
                WarehouseId = infoDto.WarehouseId,
                QCLineId = infoDto.QCLineId,
                RepairLineId = infoDto.RepairLineId,
                StateStatus = RequisitionStatus.Approved,
                Remarks = infoDto.Remarks,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                ItemTypeId = infoDto.ItemTypeId,
                ItemId = infoDto.ItemId,
                ForQty = infoDto.ForQty
            };
            List<TransferRepairItemToQcDetail> listOfDetail = new List<TransferRepairItemToQcDetail>();
            List<RepairLineStockDetailDTO> stockDetail = new List<RepairLineStockDetailDTO>();
            List<RepairItemStockDetailDTO> repairStocks = new List<RepairItemStockDetailDTO>()
            {
                new RepairItemStockDetailDTO()
                {
                    ProductionFloorId = info.LineId,
                    DescriptionId = info.DescriptionId,
                    QCId = info.QCLineId,
                    RepairLineId = info.RepairLineId,
                    WarehouseId= info.WarehouseId,
                    ItemTypeId = info.ItemTypeId,
                    ItemId = info.ItemId,
                    OrganizationId= orgId,
                    EUserId = userId,
                    Quantity = info.ForQty.Value,
                    StockStatus = StockStatus.StockOut,
                    ReferenceNumber=info.TransferCode,
                    Remarks = "Transfer Item To QC"
                }
            };
            foreach (var item in detailDto)
            {
                TransferRepairItemToQcDetail detail = new TransferRepairItemToQcDetail
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
                RepairLineStockDetailDTO stock = new RepairLineStockDetailDTO
                {
                    DescriptionId = info.DescriptionId,
                    ProductionLineId = info.LineId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = detail.UnitId,
                    WarehouseId = item.WarehouseId,
                    RepairLineId = info.RepairLineId,
                    QCLineId = info.QCLineId,
                    RefferenceNumber = info.TransferCode,
                    Quantity = item.Quantity,
                    Remarks = "Stock Out For QC (" + info.TransferCode + ")",
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    StockStatus = StockStatus.StockOut
                };
                stockDetail.Add(stock);
            }
            info.TransferRepairItemToQcDetails = listOfDetail;
            _transferRepairItemToQcInfoRepository.Insert(info);

            if (_transferRepairItemToQcInfoRepository.Save())
            {
                if (_repairLineStockDetailBusiness.SaveRepairLineStockOut(stockDetail, userId, orgId, "Stock Out For QC Transfer"))
                {
                    IsSuccess = _repairItemStockDetailBusiness.SaveRepairItemStockOut(repairStocks, userId, orgId);
                }
            }
            return IsSuccess;
        }
    }
}
