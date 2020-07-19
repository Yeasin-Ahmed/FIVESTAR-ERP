using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
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
        private readonly TransferRepairItemToQcDetailRepository _transferRepairItemToQcDetailRepository;

        private readonly ITransferRepairItemToQcDetailBusiness _transferRepairItemToQcDetailBusiness;
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly IRepairItemStockDetailBusiness _repairItemStockDetailBusiness;
        private readonly IQCItemStockDetailBusiness _qcItemStockDetailBusiness;
        private readonly IRepairItemStockInfoBusiness _repairItemStockInfoBusiness;
        private readonly IQRCodeTransferToRepairInfoBusiness _qRCodeTransferToRepairInfoBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;

       public TransferRepairItemToQcInfoBusiness(IProductionUnitOfWork productionDb, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IItemBusiness itemBusiness, ITransferRepairItemToQcDetailBusiness transferRepairItemToQcDetailBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, IRepairItemStockDetailBusiness repairItemStockDetailBusiness, IQCItemStockDetailBusiness qcItemStockDetailBusiness, IQRCodeTransferToRepairInfoBusiness qRCodeTransferToRepairInfoBusiness, IRepairItemStockInfoBusiness repairItemStockInfoBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, TransferRepairItemToQcDetailRepository transferRepairItemToQcDetailRepository)
        {
            this._productionDb = productionDb;
            this._transferRepairItemToQcInfoRepository = new TransferRepairItemToQcInfoRepository(this._productionDb);
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._transferRepairItemToQcDetailBusiness = transferRepairItemToQcDetailBusiness;
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
            this._repairItemStockDetailBusiness = repairItemStockDetailBusiness;
            this._qcItemStockDetailBusiness = qcItemStockDetailBusiness;
            this._qRCodeTransferToRepairInfoBusiness = qRCodeTransferToRepairInfoBusiness;
            this._repairItemStockInfoBusiness = repairItemStockInfoBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._transferRepairItemToQcDetailRepository = new TransferRepairItemToQcDetailRepository(this._productionDb);
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

        public async Task<bool> SaveTransferByQRCodeScanningAsync(TransferRepairItemByQRCodeScanningDTO dto, long user, long orgId)
        {

            string code = string.Empty;
            long transferId = 0;

            // Checking the QRCode is exist with the Receive Status
            var QrCodeInfo = _qRCodeTransferToRepairInfoBusiness.GetQRCodeWiseItemInfo(dto.QRCode, FinishGoodsSendStatus.Received, orgId);
            if(QrCodeInfo != null)
            {
                // Preivous Transfer Information
                var transferInfo = await GetTransferRepairItemToQcInfoByIdAsync(dto.RepairLineId, dto.QCLineId, dto.ModelId, dto.ItemId, RequisitionStatus.Approved, orgId);

                // Item Preparation Info //
                var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Production, dto.ModelId, dto.ItemId, orgId);

                // Item Preparation Detail //
                var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

                // All items in Db
                var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);

                List<TransferRepairItemToQcDetail> transferRepairItemDetails = new List<TransferRepairItemToQcDetail>() {
                    new  TransferRepairItemToQcDetail () {
                    QRCode = QrCodeInfo.QRCode,
                    IncomingTransferId = QrCodeInfo.TransferId,
                    IncomingTransferCode = QrCodeInfo.TransferCode
                }
            };

                // If there is Pending Transfer 
                if (transferInfo != null && transferInfo.TRQInfoId > 0 )
                {
                    transferInfo.ForQty += 1;
                    transferInfo.UpUserId = user;
                    transferInfo.UpdateDate = DateTime.Now;
                    code = transferInfo.TransferCode;
                }
                else
                {
                    // If there is no Pending Transfer 
                    code = "TRQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                    transferInfo = new TransferRepairItemToQcInfo()
                    {
                        LineId = QrCodeInfo.FloorId,
                        QCLineId = QrCodeInfo.QCLineId,
                        RepairLineId= QrCodeInfo.RepairLineId,
                        ForQty = 1,
                        StateStatus = RequisitionStatus.Approved,
                        DescriptionId = dto.ModelId,
                        ItemId = dto.ItemId,
                        ItemTypeId = QrCodeInfo.ItemTypeId,
                        WarehouseId = QrCodeInfo.WarehouseId,
                        EUserId = user,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        TransferCode = code
                    };
                    transferInfo.TransferRepairItemToQcDetails = transferRepairItemDetails;

                }

                // Repair Speare Parts Stock
                List<RepairLineStockDetailDTO> repairLineStocks = new List<RepairLineStockDetailDTO>();
                foreach (var item in itemPreparationDetail)
                {
                    var itemInfo = allItemsInDb.FirstOrDefault(s => s.ItemId == item.ItemId);
                    RepairLineStockDetailDTO repairLineStock = new RepairLineStockDetailDTO()
                    {
                        ProductionLineId = QrCodeInfo.FloorId,
                        RepairLineId = QrCodeInfo.RepairLineId,
                        QCLineId = QrCodeInfo.QCLineId,
                        DescriptionId = itemPreparationInfo.DescriptionId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        WarehouseId = item.WarehouseId,
                        Quantity = item.Quantity,
                        StockStatus = StockStatus.StockOut,
                        EntryDate = DateTime.Now,
                        EUserId = user,
                        OrganizationId = orgId,
                        RefferenceNumber = code,
                        Remarks = "Stock Out By Repair Done-" + QrCodeInfo.QRCode,
                        UnitId = itemInfo.UnitId
                    };
                    repairLineStocks.Add(repairLineStock);
                }


                // Repair Item Stocks
                List<RepairItemStockDetailDTO> repairItemStocks = new List<RepairItemStockDetailDTO>() {
                    new RepairItemStockDetailDTO()
                    {
                        ProductionFloorId = QrCodeInfo.FloorId,
                        RepairLineId = QrCodeInfo.RepairLineId,
                        QCId = QrCodeInfo.QCLineId,
                        DescriptionId = QrCodeInfo.DescriptionId,
                        ItemId = QrCodeInfo.ItemId,
                        ItemTypeId = QrCodeInfo.ItemTypeId,
                        WarehouseId = QrCodeInfo.WarehouseId,
                        Quantity = 1,
                        OrganizationId = orgId,
                        EUserId = user,
                        EntryDate = DateTime.Now,
                        StockStatus = StockStatus.StockOut,
                        ReferenceNumber = code,
                        Remarks ="Stock Out By Repair Done-"+QrCodeInfo.QRCode
                    }
                };


                if (await _repairItemStockDetailBusiness.SaveRepairItemStockOutAsync(repairItemStocks, user, orgId)) {

                    if(await _repairLineStockDetailBusiness.SaveRepairLineStockOutAsync(repairLineStocks, user, orgId,string.Empty))
                    {
                        if(transferInfo.TRQInfoId > 0)
                        {
                            _transferRepairItemToQcInfoRepository.Update(transferInfo);
                            _transferRepairItemToQcDetailRepository.InsertAll(transferRepairItemDetails);
                        }
                        else
                        {
                            _transferRepairItemToQcInfoRepository.Insert(transferInfo);
                        }
                    }
                }

                
                //code = transferInfo.TransferCode;
                //QrCodeInfo.StateStatus = "Repair-Done";
                //QrCodeInfo.UpdateDate = DateTime.Now;
                //QrCodeInfo.UpUserId = user;
                //transferId = transferInfo.TRQInfoId;

                // Repair Item Stock
                //_repairItemStockDetailBusiness

            }
            return false;
        }

        public async Task<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfoByIdAsync(long repairLineId, long qcLineId, long modelId, long itemId, string status, long orgId)
        {
            return await _transferRepairItemToQcInfoRepository.GetOneByOrgAsync(s => s.RepairLineId == repairLineId && s.QCLineId == qcLineId && s.DescriptionId == modelId && s.ItemId == itemId && s.StateStatus == status && s.OrganizationId == orgId);
        }
    }
}
