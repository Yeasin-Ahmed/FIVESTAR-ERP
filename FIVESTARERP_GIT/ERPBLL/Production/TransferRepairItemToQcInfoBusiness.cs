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
        // Repository
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferRepairItemToQcInfoRepository _transferRepairItemToQcInfoRepository;
        private readonly TransferRepairItemToQcDetailRepository _transferRepairItemToQcDetailRepository;
        private readonly QRCodeTransferToRepairInfoRepository _qRCodeTransferToRepairInfoRepository;

        // Business
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
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;

        public TransferRepairItemToQcInfoBusiness(IProductionUnitOfWork productionDb, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IItemBusiness itemBusiness, ITransferRepairItemToQcDetailBusiness transferRepairItemToQcDetailBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, IRepairItemStockDetailBusiness repairItemStockDetailBusiness, IQCItemStockDetailBusiness qcItemStockDetailBusiness, IQRCodeTransferToRepairInfoBusiness qRCodeTransferToRepairInfoBusiness, IRepairItemStockInfoBusiness repairItemStockInfoBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, TransferRepairItemToQcDetailRepository transferRepairItemToQcDetailRepository, QRCodeTransferToRepairInfoRepository qRCodeTransferToRepairInfoRepository, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness)
        {
            // Database
            this._productionDb = productionDb;

            // Repository
            this._transferRepairItemToQcInfoRepository = new TransferRepairItemToQcInfoRepository(this._productionDb);
            this._transferRepairItemToQcDetailRepository = new TransferRepairItemToQcDetailRepository(this._productionDb);
            this._qRCodeTransferToRepairInfoRepository = new QRCodeTransferToRepairInfoRepository(this._productionDb);

            // Business class
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
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
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
                    if (_qCLineStockDetailBusiness.SaveQCLineStockIn(stockDetails, userId, orgId))
                    {
                        IsSuccess = _qcItemStockDetailBusiness.SaveQCItemStockIn(qcItemStocks, userId, orgId);
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
            var QrCodeInfoDto = _qRCodeTransferToRepairInfoBusiness.GetQRCodeWiseItemInfo(dto.QRCode, string.Format(@"'Received'"), orgId);
            if (QrCodeInfoDto != null)
            {
                // Preivous Transfer Information
                var transferInfo = await GetTransferRepairItemToQcInfoByIdAsync(dto.AssemblyLineId, dto.RepairLineId, dto.QCLineId, dto.ModelId, dto.ItemId, RequisitionStatus.Approved, orgId);

                // Item Preparation Info //
                var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Production, dto.ModelId, dto.ItemId, orgId);

                // Item Preparation Detail //
                var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

                // All items in Db
                var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);

                List<TransferRepairItemToQcDetail> transferRepairItemDetails = new List<TransferRepairItemToQcDetail>() {
                    new  TransferRepairItemToQcDetail () {
                    OrganizationId= orgId,
                    EUserId = user,
                    Quantity=1,
                    EntryDate = DateTime.Now,
                    QRCode = QrCodeInfoDto.QRCode,
                    IncomingTransferId = QrCodeInfoDto.TransferId,
                    IncomingTransferCode = QrCodeInfoDto.TransferCode}};

                // If there is Pending Transfer 
                if (transferInfo != null && transferInfo.TRQInfoId > 0)
                {
                    transferInfo.ForQty += 1;
                    transferInfo.UpUserId = user;
                    transferInfo.UpdateDate = DateTime.Now;
                    code = transferInfo.TransferCode;
                    transferId = transferInfo.TRQInfoId;

                    foreach (var item in transferRepairItemDetails)
                    {
                        item.TRQInfoId = transferInfo.TRQInfoId;
                    }
                }
                else
                {
                    // If there is no Pending Transfer 
                    code = "TRQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                    transferInfo = new TransferRepairItemToQcInfo()
                    {
                        LineId = QrCodeInfoDto.FloorId,
                        AssemblyLineId = QrCodeInfoDto.AssemblyLineId,
                        QCLineId = QrCodeInfoDto.QCLineId,
                        RepairLineId = QrCodeInfoDto.RepairLineId,
                        ForQty = 1,
                        StateStatus = RequisitionStatus.Approved,
                        DescriptionId = dto.ModelId,
                        ItemId = dto.ItemId,
                        ItemTypeId = QrCodeInfoDto.ItemTypeId,
                        WarehouseId = QrCodeInfoDto.WarehouseId,
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
                        ProductionLineId = QrCodeInfoDto.FloorId,
                        AssemblyLineId = QrCodeInfoDto.AssemblyLineId,
                        RepairLineId = QrCodeInfoDto.RepairLineId,
                        QCLineId = QrCodeInfoDto.QCLineId,
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
                        Remarks = "Stock Out By Repair Done-" + QrCodeInfoDto.QRCode,
                        UnitId = itemInfo.UnitId
                    };
                    repairLineStocks.Add(repairLineStock);
                }

                // Repair Item Stocks
                List<RepairItemStockDetailDTO> repairItemStocks = new List<RepairItemStockDetailDTO>() {
                    new RepairItemStockDetailDTO()
                    {
                        ProductionFloorId = QrCodeInfoDto.FloorId,
                        AssemblyLineId= QrCodeInfoDto.AssemblyLineId,
                        RepairLineId = QrCodeInfoDto.RepairLineId,
                        QCId = QrCodeInfoDto.QCLineId,
                        DescriptionId = QrCodeInfoDto.DescriptionId,
                        ItemId = QrCodeInfoDto.ItemId,
                        ItemTypeId = QrCodeInfoDto.ItemTypeId,
                        WarehouseId = QrCodeInfoDto.WarehouseId,
                        Quantity = 1,
                        OrganizationId = orgId,
                        EUserId = user,
                        EntryDate = DateTime.Now,
                        StockStatus = StockStatus.StockOut,
                        ReferenceNumber = code,
                        Remarks ="Stock Out By Repair Done-"+QrCodeInfoDto.QRCode
                    }
                };

                if (await _repairItemStockDetailBusiness.SaveRepairItemStockOutAsync(repairItemStocks, user, orgId))
                {

                    if (await _repairLineStockDetailBusiness.SaveRepairLineStockOutAsync(repairLineStocks, user, orgId, string.Empty))
                    {
                        if (transferInfo.TRQInfoId > 0)
                        {
                            _transferRepairItemToQcInfoRepository.Update(transferInfo);
                            _transferRepairItemToQcDetailRepository.InsertAll(transferRepairItemDetails);
                        }
                        else
                        {
                            _transferRepairItemToQcInfoRepository.Insert(transferInfo);
                        }

                        var qrCodeInfoInDb = await _qRCodeTransferToRepairInfoBusiness.GetQRCodeTransferToRepairInfoByIdAsync(QrCodeInfoDto.QRTRInfoId, orgId);
                        qrCodeInfoInDb.StateStatus = "Repair-Done";
                        qrCodeInfoInDb.UpdateDate = DateTime.Now;
                        qrCodeInfoInDb.UpUserId = user;
                        _qRCodeTransferToRepairInfoRepository.Update(qrCodeInfoInDb);

                        return await _transferRepairItemToQcInfoRepository.SaveAsync();
                    }
                }
            }
            return false;
        }

        public async Task<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfoByIdAsync(long assemblyLineId, long repairLineId, long qcLineId, long modelId, long itemId, string status, long orgId)
        {
            return await _transferRepairItemToQcInfoRepository.GetOneByOrgAsync(s => s.AssemblyLineId == assemblyLineId && s.RepairLineId == repairLineId && s.QCLineId == qcLineId && s.DescriptionId == modelId && s.ItemId == itemId && s.StateStatus == status && s.OrganizationId == orgId);
        }

        public IEnumerable<TransferRepairItemToQcInfoDTO> GetTransferRepairItemToQcInfosByQuery(long? floorId, long? assemblyId, long? repairLineId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferRepairItemToQcInfoDTO>(QueryForTransferRepairItemToQcInfos(floorId, assemblyId, repairLineId, qcLineId, modelId, warehouseId, itemTypeId, itemId, status, transferCode, fromDate, toDate, orgId)).ToList();
        }

        private string QueryForTransferRepairItemToQcInfos(long? floorId, long? assemblyId, long? repairLineId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param = string.Format(@" and ti.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and ti.LineId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and ti.AssemblyLineId={0}", assemblyId);
            }
            if (repairLineId != null && repairLineId > 0)
            {
                param += string.Format(@" and ti.RepairLineId={0}", repairLineId);
            }
            if (qcLineId != null && qcLineId > 0)
            {
                param += string.Format(@" and ti.QCLineId={0}", qcLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and ti.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and ti.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and ti.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and ti.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status) && status != "")
            {
                param += string.Format(@" and ti.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode != "")
            {
                param += string.Format(@" and ti.TransferCode Like '%{0}%'", transferCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ti.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ti.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ti.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select ti.TRQInfoId,ti.TransferCode,ti.LineId,pl.LineNumber 'LineName',ti.QCLineId, qc.QCName 'QCLineName',ti.RepairLineId,rl.RepairLineName,ti.DescriptionId,de.DescriptionName 'ModelName',
 ti.WarehouseId ,w.WarehouseName,ti.ItemTypeId, it.ItemName 'ItemTypeName',ti.ItemId,i.ItemName,
 ti.StateStatus,ti.ForQty,app.UserName 'EntryUser',ti.EntryDate,
(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId=ti.UpUserId) 'UpdateUser',
ti.UpdateDate
From [Production].dbo.tblTransferRepairItemToQcInfo ti
Inner Join [Inventory].dbo.tblDescriptions de on ti.DescriptionId = de.DescriptionId
Inner Join [Production].dbo.tblProductionLines pl on ti.LineId= pl.LineId
Inner Join [Production].dbo.tblQualityControl qc on ti.QCLineId = qc.QCId
Inner Join [Production].dbo.tblRepairLine rl on ti.RepairLineId = rl.RepairLineId
Inner Join [Inventory].dbo.tblWarehouses w on ti.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on ti.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on ti.ItemId = i.ItemId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on ti.EUserId = app.UserId
Where 1= 1 {0}", param);

            return query;
        }

        public async Task<bool> SaveTransferInfoStateStatusAsync(long transferId, string status, long userId, long orgId)
        {
            //Transfer Info
            var transferInDb = await GetTransferRepairItemToQcInfoByIdAsync(transferId, orgId);

            if(transferInDb != null && transferInDb.StateStatus == RequisitionStatus.Approved && status == RequisitionStatus.Accepted)
            {
                // Transfer Info
                transferInDb.StateStatus = RequisitionStatus.Accepted;
                transferInDb.UpdateDate = DateTime.Now;
                transferInDb.UpUserId = userId;

                // Transfer Detail
                var transferDetail = await _transferRepairItemToQcDetailBusiness.GetTransferRepairItemToQcDetailByInfoAsync(transferInDb.TRQInfoId, orgId);

                // Item Preparation Info //
                var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Production, transferInDb.DescriptionId.Value, transferInDb.ItemId.Value, orgId);

                // Item Preparation Detail //
                var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

                List<QCItemStockDetailDTO> qCItemStocks = new List<QCItemStockDetailDTO>() {
                new QCItemStockDetailDTO{
                    ProductionFloorId= transferInDb.LineId,
                    AssemblyLineId = transferInDb.AssemblyLineId,
                    QCId = transferInDb.QCLineId,
                    RepairLineId = transferInDb.RepairLineId,
                    DescriptionId = transferInDb.DescriptionId,
                    WarehouseId = transferInDb.WarehouseId,
                    ItemTypeId = transferInDb.ItemTypeId,
                    ItemId = transferInDb.ItemId,
                    Quantity = transferInDb.ForQty.Value,
                    StockStatus = StockStatus.StockIn,
                    EntryDate= DateTime.Now,
                    EUserId = userId,
                    Flag="Repair",
                    OrganizationId = orgId,
                    ReferenceNumber = transferInDb.TransferCode,
                    Remarks ="Stock In By Repair Item Return"
                }};

                List<AssemblyLineStockDetailDTO> assemblyLineStocks = new List<AssemblyLineStockDetailDTO>();
                foreach (var item in itemPreparationDetail)
                {
                    AssemblyLineStockDetailDTO assemblyLineStock = new AssemblyLineStockDetailDTO()
                    {
                        ProductionLineId = transferInDb.LineId,
                        AssemblyLineId = transferInDb.AssemblyLineId,
                        DescriptionId = transferInDb.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = (transferInDb.ForQty * item.Quantity).Value,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        StockStatus = StockStatus.StockIn,
                        RefferenceNumber = transferInDb.TransferCode,
                        Remarks = "Stock In By Repair Item Return",
                        UnitId = item.UnitId
                    };
                    assemblyLineStocks.Add(assemblyLineStock);
                }

                // QRCode //
                var qrCodes = transferDetail.Select(s => s.QRCode).ToList();
                var getQRCodeForUpdate =await _tempQRCodeTraceBusiness.GetTempQRCodeTracesByQRCodesAsync(qrCodes, orgId);
                foreach (var item in getQRCodeForUpdate)
                {
                    item.StateStatus = QRCodeStatus.Assembly;
                    item.UpdateDate = DateTime.Now;
                    item.UpUserId = userId;
                }
                _transferRepairItemToQcInfoRepository.Update(transferInDb);
                if (await _transferRepairItemToQcInfoRepository.SaveAsync())
                {
                    if(await _assemblyLineStockDetailBusiness.SaveAssemblyLineStockInAsync(assemblyLineStocks, userId, orgId))
                    {
                        if(await _qcItemStockDetailBusiness.SaveQCItemStockInAsync(qCItemStocks, userId, orgId))
                        {
                            return await _tempQRCodeTraceBusiness.UpdateQRCodeBatchAsync(getQRCodeForUpdate.ToList(), orgId);
                        }
                    }
                }
            }
            return false;
        }

        public async Task<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfoByIdAsync(long transferId, long orgId)
        {
            return await _transferRepairItemToQcInfoRepository.GetOneByOrgAsync(t => t.TRQInfoId == transferId && t.OrganizationId == orgId);
        }
    }
}
