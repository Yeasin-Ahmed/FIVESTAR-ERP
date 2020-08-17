using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
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
    public class QRCodeTransferToRepairInfoBusiness : IQRCodeTransferToRepairInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
       
        // Business Class
        private readonly ITransferFromQCInfoBusiness _transferFromQCInfoBusiness;
        private readonly ITransferFromQCDetailBusiness _transferFromQCDetailBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly IQCItemStockInfoBusiness _qCItemStockInfoBusiness;
        private readonly IQCItemStockDetailBusiness _qCItemStockDetailBusiness;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly IRepairLineStockInfoBusiness _repairLineStockInfoBusiness;
        private readonly IFaultyItemStockDetailBusiness _faultyItemStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;

        // Repository 
        private readonly TransferFromQCInfoRepository _transferFromQCInfoRepository;
        private readonly TransferFromQCDetailRepository _transferFromQCDetailRepository;
        private readonly QRCodeTransferToRepairInfoRepository _qRCodeTransferToRepairInfoRepository;
        public QRCodeTransferToRepairInfoBusiness(IProductionUnitOfWork productionDb, ITransferFromQCInfoBusiness transferFromQCInfoBusiness, ITransferFromQCDetailBusiness transferFromQCDetailBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IQCItemStockInfoBusiness qCItemStockInfoBusiness, IQCItemStockDetailBusiness qCItemStockDetailBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, IFaultyItemStockDetailBusiness faultyItemStockDetailBusiness, IItemBusiness itemBusiness, IRepairLineStockInfoBusiness repairLineStockInfoBusiness, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness)
        {
            this._productionDb = productionDb;
            this._transferFromQCInfoBusiness = transferFromQCInfoBusiness;
            this._transferFromQCDetailBusiness = transferFromQCDetailBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._qCItemStockInfoBusiness = qCItemStockInfoBusiness;
            this._qCItemStockDetailBusiness = qCItemStockDetailBusiness;
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
            this._faultyItemStockDetailBusiness = faultyItemStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._repairLineStockInfoBusiness = repairLineStockInfoBusiness;
            this._qRCodeTransferToRepairInfoRepository = new QRCodeTransferToRepairInfoRepository(this._productionDb);
            this._transferFromQCInfoRepository = new TransferFromQCInfoRepository(this._productionDb);
            this._transferFromQCDetailRepository = new TransferFromQCDetailRepository(this._productionDb);
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
        }


        public IEnumerable<QRCodeTransferToRepairInfo> GetQRCodeTransferToRepairInfoByTransferId(long transferId, long orgId)
        {
            return _qRCodeTransferToRepairInfoRepository.GetAll(s => s.TransferId == transferId && s.OrganizationId == orgId);
        }

        public IEnumerable<QRCodeTransferToRepairInfoDTO> GetQRCodeTransferToRepairInfoByTransferIdByQuery(long transferId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QRCodeTransferToRepairInfoDTO>(QueryQRCodeTransferToRepairInfoByTransferIdByQuery(transferId, orgId)).ToList();
        }

        private string QueryQRCodeTransferToRepairInfoByTransferIdByQuery(long transferId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and qr.OrganizationId={0}", orgId);
            if (transferId > 0)
            {
                param += string.Format(@" and qr.TransferId={0}", transferId);
            }

            query = string.Format(@"Select qr.TransferId,qr.QRCode,qr.TransferCode,qr.FloorId,qr.QCLineId,qr.RepairLineId,qr.AssemblyLineId,qr.DescriptionId,qr.WarehouseId,
qr.ItemTypeId,qr.ItemId,qr.StateStatus,qr.OrganizationId,pl.LineNumber 'FloorName',qc.QCName 'QCLineName',rl.RepairLineName,al.AssemblyLineName,de.DescriptionName 'ModelName',
w.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName From [Production].dbo.tblQRCodeTransferToRepairInfo qr
Inner Join [Production].dbo.tblProductionLines pl on qr.FloorId = pl.LineId
Inner Join [Production].dbo.tblQualityControl qc on qr.QCLineId = qc.QCId
Inner Join [Production].dbo.tblRepairLine rl on qr.RepairLineId = rl.RepairLineId
Inner Join [Production].dbo.tblAssemblyLines al on qr.AssemblyLineId = al.AssemblyLineId
Inner Join [Inventory].dbo.tblDescriptions de on qr.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on qr.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on qr.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on qr.ItemId = i.ItemId
Where 1= 1 {0}", Utility.ParamChecker(param));

            return query;
        }

        public IEnumerable<QRCodeTransferToRepairInfo> GetRCodeTransferToRepairInfos(long orgId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveQRCodeTransferToRepairAsync(QRCodeTransferToRepairInfoDTO dto, long userId, long orgId)
        {
            // Checking Transfer //
            bool IsSuccess = false;
            string code = string.Empty;
            long transferId = 0;

            // Previous transfer Information
            var transferInfo = await _transferFromQCInfoBusiness.GetNonReceivedTransferFromQCInfoByQRCodeKeyAsync(dto.AssemblyLineId, dto.QCLineId, dto.RepairLineId, dto.DescriptionId, dto.WarehouseId.Value, dto.ItemTypeId.Value, dto.ItemId.Value, orgId);

            // Item Preparation Info //
            var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Production, dto.DescriptionId, dto.ItemId.Value, orgId);

            // Item Preparation Detail //
            var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

            // Assembly Line Raw Material Stock
            List<AssemblyLineStockDetailDTO> stockDetailDTOs = new List<AssemblyLineStockDetailDTO>();

            List<TransferFromQCDetail> transferDetails = new List<TransferFromQCDetail>();
            if (transferInfo != null) // If there is a pending transfer information
            {
                transferInfo.ForQty += 1;
                code = transferInfo.TransferCode;
                transferId = transferInfo.TFQInfoId;
                //transferDetails = (List<TransferFromQCDetail>)(await _transferFromQCDetailBusiness.GetTransferFromQCDetailByInfoAsync(transferInfo.TFQInfoId, orgId));\

                foreach (var item in itemPreparationDetail)
                {
                    AssemblyLineStockDetailDTO assemblyStock = new AssemblyLineStockDetailDTO
                    {
                        ProductionLineId = dto.FloorId,
                        AssemblyLineId = dto.AssemblyLineId,
                        DescriptionId = dto.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = orgId,
                        Quantity = item.Quantity,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId,
                        RefferenceNumber = code,
                        StockStatus = StockStatus.StockOut
                    };
                    stockDetailDTOs.Add(assemblyStock);
                }
            }
            else
            {
                /// New Transfer //
                code = "TFQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

                transferInfo = new TransferFromQCInfo
                {
                    LineId = dto.FloorId,
                    AssemblyLineId = dto.AssemblyLineId,
                    QCLineId = dto.QCLineId,
                    DescriptionId = dto.DescriptionId,
                    RepairLineId = dto.RepairLineId,
                    WarehouseId = dto.WarehouseId,
                    ItemTypeId = dto.ItemTypeId,
                    ItemId = dto.ItemId,
                    ForQty = 1,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    TransferCode = code,
                    StateStatus = RequisitionStatus.Approved,
                    TransferFor = "Repair Line"
                };

                //transferDetails = new List<TransferFromQCDetail>();

                foreach (var item in itemPreparationDetail)
                {
                    TransferFromQCDetail transferDetail = new TransferFromQCDetail
                    {
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = orgId,
                        Quantity = item.Quantity,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId
                    };
                    transferDetails.Add(transferDetail);

                    AssemblyLineStockDetailDTO assemblyStock = new AssemblyLineStockDetailDTO
                    {
                        ProductionLineId = dto.FloorId,
                        AssemblyLineId = dto.AssemblyLineId,
                        DescriptionId = dto.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = orgId,
                        Quantity = item.Quantity,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId,
                        RefferenceNumber = code,
                        StockStatus = StockStatus.StockIn
                    };
                    stockDetailDTOs.Add(assemblyStock);
                }

                transferInfo.TransferFromQCDetails = transferDetails;
            }

            List<QCItemStockDetailDTO> qCItemStockDetails = new List<QCItemStockDetailDTO>() {
                new QCItemStockDetailDTO
                {
                    ProductionFloorId = dto.FloorId,
                    AssemblyLineId = dto.AssemblyLineId,
                    QCId = dto.QCLineId,
                    DescriptionId = dto.DescriptionId,
                    WarehouseId = dto.WarehouseId,
                    ItemTypeId = dto.ItemTypeId,
                    ItemId = dto.ItemId,
                    Flag ="Repair",
                    Quantity = 1,
                    EUserId= userId,
                    OrganizationId = orgId,
                    RepairLineId = dto.RepairLineId,
                    StockStatus = StockStatus.StockOut,
                    ReferenceNumber = code,
                    EntryDate = DateTime.Now
                }
            };
           
            if (transferInfo.TFQInfoId == 0)
            {
                _transferFromQCInfoRepository.Insert(transferInfo);
            }
            else
            {
                _transferFromQCInfoRepository.Update(transferInfo);
                //_transferFromQCDetailRepository.InsertAll(transferDetails);
            }
            if (await _transferFromQCInfoRepository.SaveAsync())
            {
                if(await _qCItemStockDetailBusiness.SaveQCItemStockOutAsync(qCItemStockDetails, userId, orgId))
                {
                    if (await _assemblyLineStockDetailBusiness.SaveAssemblyLineStockOutAsync(stockDetailDTOs, userId, orgId,string.Empty))
                    {
                        // QR Code //
                        if(_tempQRCodeTraceBusiness.UpdateQRCodeStatus(dto.QRCode, QRCodeStatus.AssemblyRepair, orgId)){
                            QRCodeTransferToRepairInfo qRCodeTransferToRepairInfo = new QRCodeTransferToRepairInfo
                            {
                                FloorId = dto.FloorId,
                                AssemblyLineId = dto.AssemblyLineId,
                                DescriptionId = dto.DescriptionId,
                                RepairLineId = dto.RepairLineId,
                                WarehouseId = dto.WarehouseId,
                                ItemTypeId = dto.ItemTypeId,
                                ItemId = dto.ItemId,
                                QCLineId = dto.QCLineId,
                                OrganizationId = orgId,
                                StateStatus = FinishGoodsSendStatus.Send,
                                EntryDate = DateTime.Now,
                                EUserId = userId,
                                QRCode = dto.QRCode,
                                TransferCode = code, // Came from if there is a pending transfer against the QC line 
                                TransferId = transferInfo.TFQInfoId
                            };
                            List<QRCodeProblem> qRCodeProblems = new List<QRCodeProblem>();
                            foreach (var item in dto.QRCodeProblems)
                            {
                                QRCodeProblem qRCodeProblem = new QRCodeProblem
                                {
                                    FloorId = dto.FloorId,
                                    AssemblyLineId = dto.AssemblyLineId,
                                    DescriptionId = dto.DescriptionId,
                                    RepairLineId = dto.RepairLineId,
                                    QCLineId = dto.QCLineId,
                                    OrganizationId = orgId,
                                    QRCode = dto.QRCode,
                                    ProblemId = item.ProblemId,
                                    ProblemName = item.ProblemName,
                                    EntryDate = DateTime.Now,
                                    EUserId = userId,
                                    TransferCode = transferInfo.TransferCode, // Came from if there is a pending transfer against the QC line 
                                    TransferId = transferInfo.TFQInfoId
                                };
                                qRCodeProblems.Add(qRCodeProblem);
                            }
                            qRCodeTransferToRepairInfo.QRCodeProblems = qRCodeProblems;
                            _qRCodeTransferToRepairInfoRepository.Insert(qRCodeTransferToRepairInfo);
                            IsSuccess = await _qRCodeTransferToRepairInfoRepository.SaveAsync();
                        }
                    }
                }
            }
           
            return IsSuccess;
        }

        public bool SaveQRCodeStatusByTrasnferInfoId(long transferId, string status, long userId, long orgId)
        {
            if (_transferFromQCInfoBusiness.SaveTransferInfoStateStatus(transferId, status, userId, orgId))
            {
                var qrCodesInDb = GetQRCodeTransferToRepairInfoByTransferId(transferId, orgId);

                foreach (var item in qrCodesInDb)
                {
                    item.StateStatus = FinishGoodsSendStatus.Received;
                    item.UpdateDate = DateTime.Now;
                    item.UpUserId = userId;
                    _qRCodeTransferToRepairInfoRepository.Update(item);
                }
            }
            return _qRCodeTransferToRepairInfoRepository.Save();
        }

        public QRCodeTransferToRepairInfoDTO GetQRCodeWiseItemInfo(string qrCode, string status, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and qr.OrganizationId={0}", orgId);
            if (!string.IsNullOrEmpty(qrCode))
            {
                param += string.Format(@" and qr.QRCode='{0}'", qrCode);
            }
            if (!string.IsNullOrEmpty(status))
            {
                param += string.Format(@" and qr.StateStatus IN({0})", status);
            }

            query = string.Format(@"Select qr.QRTRInfoId,qr.TransferId,qr.QRCode,qr.TransferCode,qr.FloorId,qr.QCLineId,qr.RepairLineId,qr.AssemblyLineId,qr.DescriptionId,qr.WarehouseId,qr.ItemTypeId,qr.ItemId,qr.StateStatus,qr.OrganizationId,pl.LineNumber 'FloorName',qc.QCName 'QCLineName',rl.RepairLineName,al.AssemblyLineName,de.DescriptionName 'ModelName',w.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName From [Production].dbo.tblQRCodeTransferToRepairInfo qr
                Inner Join [Production].dbo.tblProductionLines pl on qr.FloorId = pl.LineId
                Inner Join [Production].dbo.tblQualityControl qc on qr.QCLineId = qc.QCId
                Inner Join [Production].dbo.tblRepairLine rl on qr.RepairLineId = rl.RepairLineId
                Inner Join [Production].dbo.tblAssemblyLines al on qr.AssemblyLineId = al.AssemblyLineId
                Inner Join [Inventory].dbo.tblDescriptions de on qr.DescriptionId = de.DescriptionId
                Inner Join [Inventory].dbo.tblWarehouses w on qr.WarehouseId = w.Id
                Inner Join [Inventory].dbo.tblItemTypes it on qr.ItemTypeId = it.ItemId
                Inner Join [Inventory].dbo.tblItems i on qr.ItemId = i.ItemId
                Where 1= 1 {0} Order By QRTRInfoId desc", Utility.ParamChecker(param));

            var data = this._productionDb.Db.Database.SqlQuery<QRCodeTransferToRepairInfoDTO>(query).FirstOrDefault();

            return data;
        }

        //Adding Faulty Item By QRCode Scanning //
        public bool StockOutByAddingFaultyWithQRCode(FaultyInfoByQRCodeDTO model, long userId, long orgId)
        {
            //Check if the QRCode is exist with the status Received
           var qrCodeInfo = GetQRCodeWiseItemInfo(model.QRCode, string.Format(@"'Received'"), orgId);
            if (qrCodeInfo != null && qrCodeInfo.TransferId == model.TransferId && qrCodeInfo.DescriptionId == model.ModelId)
            {
                var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId).ToList();
                List<RepairLineStockDetailDTO> repairLineStocks = new List<RepairLineStockDetailDTO>();
                List<FaultyItemStockDetailDTO> faultyItemStocks = new List<FaultyItemStockDetailDTO>();
                foreach (var item in model.FaultyItems)
                {
                    var itemInfo = allItemsInDb.FirstOrDefault(i => i.ItemId == item.ItemId);
                    RepairLineStockDetailDTO repairLineStock = new RepairLineStockDetailDTO()
                    {
                        ProductionLineId = qrCodeInfo.FloorId,
                        AssemblyLineId = qrCodeInfo.AssemblyLineId,
                        QCLineId = qrCodeInfo.QCLineId,
                        RepairLineId = qrCodeInfo.RepairLineId,
                        DescriptionId = qrCodeInfo.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        StockStatus = StockStatus.StockOut,
                        OrganizationId = orgId,
                        EUserId = userId,
                        Remarks = "Stock Out By QRCode Scanning",
                        EntryDate = DateTime.Now,
                        RefferenceNumber = qrCodeInfo.QRCode + "#" + qrCodeInfo.TransferCode + "#" + qrCodeInfo.TransferId.ToString(),
                        UnitId = itemInfo.UnitId
                    };

                    repairLineStocks.Add(repairLineStock);

                    FaultyItemStockDetailDTO faultyItemStock = new FaultyItemStockDetailDTO()
                    {
                        ProductionFloorId = qrCodeInfo.FloorId,
                        AsseemblyLineId = qrCodeInfo.AssemblyLineId,
                        QCId = qrCodeInfo.QCLineId,
                        RepairLineId = qrCodeInfo.RepairLineId,
                        DescriptionId = qrCodeInfo.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        StockStatus = StockStatus.StockIn,
                        OrganizationId = orgId,
                        EUserId = userId,
                        Remarks = "Stock In By QRCode Scanning",
                        EntryDate = DateTime.Now,
                        ReferenceNumber = qrCodeInfo.QRCode,
                        TransferCode = qrCodeInfo.TransferCode,
                        TransferId = qrCodeInfo.TransferId,
                        UnitId = itemInfo.UnitId,
                        IsChinaFaulty =item.IsChinaFaulty
                    };

                    faultyItemStocks.Add(faultyItemStock);
                }

                if (repairLineStocks.Count > 0 && _repairLineStockDetailBusiness.SaveRepairLineStockOut(repairLineStocks, userId, orgId, string.Empty))
                {
                    return _faultyItemStockDetailBusiness.SaveFaultyItemStockIn(faultyItemStocks, userId, orgId);
                }
            }
            return false;
        }

        public ExecutionStateWithText CheckingAvailabilityOfSparepartsWithRepairLineStock(long modelId, long itemId, long repairLineId, long orgId)
        {
            var itemPreparationInfo = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndType(ItemPreparationType.Production, modelId, itemId, orgId);
            var itemPreparationDetail = _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoId(itemPreparationInfo.PreparationInfoId, orgId);
            var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);
            ExecutionStateWithText execution = new ExecutionStateWithText();
            execution.isSuccess = true;
            foreach (var item in itemPreparationDetail)
            {
                var repairLineStock = _repairLineStockInfoBusiness.GetRepairLineStockInfoByRepairAndItemAndModelId(repairLineId, item.ItemId, modelId, orgId);
                var itemName = allItemsInDb.FirstOrDefault(s => s.ItemId == item.ItemId).ItemName;
                if (repairLineStock != null)
                {
                    if (item.Quantity > (repairLineStock.StockInQty - repairLineStock.StockOutQty).Value)
                    {
                        execution.text += itemName + " does not have enough qty </br>";
                        execution.isSuccess = false;
                    }
                }
                else
                {
                    execution.text += itemName + " does not have enough qty </br>";
                    execution.isSuccess = false;
                }
            }

            return execution;
        }

        public async Task<QRCodeTransferToRepairInfo> GetQRCodeTransferToRepairInfoByIdAsync(long id, long orgId)
        {
            return await _qRCodeTransferToRepairInfoRepository.GetOneByOrgAsync(s => s.QRTRInfoId == id && s.OrganizationId == orgId);
        }

        public bool IsQRCodeExistInTransferWithStatus(string qrCode, string status, long orgId)
        {
            return this.GetQRCodeWiseItemInfo(qrCode, status, orgId) != null;
        }

        public IEnumerable<QRCodeTransferToRepairInfoDTO> GetQRCodeTransferToRepairInfosByQuery(long? floorId, long? assemblyId, long? qcLineId, long? repairLineId, string qrCode, string transferCode, string status, string date, long? userId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QRCodeTransferToRepairInfoDTO>(QueryForQRCodeTransferToRepairInfos(floorId, assemblyId, qcLineId, repairLineId, qrCode, transferCode, status, date, userId, orgId)).ToList();
        }

        private string QueryForQRCodeTransferToRepairInfos(long? floorId, long? assemblyId, long? qcLineId, long? repairLineId, string qrCode, string transferCode, string status,string date, long? userId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and qr.OrganizationId={0}",orgId);

            if (floorId != null  && floorId> 0)
            {
                param += string.Format(@" and qr.FloorId={0}", floorId);
            }
            if(assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and qr.AssemblyLineId={0}", assemblyId);
            }
            if(qcLineId != null && qcLineId > 0)
            {
                param += string.Format(@" and qr.QCLineId={0}", qcLineId);
            }
            if (repairLineId != null && repairLineId > 0)
            {
                param += string.Format(@" and qr.RepairLineId={0}", qcLineId);
            }
            if (!string.IsNullOrEmpty(qrCode) && qrCode !="")
            {
                param += string.Format(@" and qr.QRCode Like '%{0}%'", qrCode);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode != "")
            {
                param += string.Format(@" and qr.TransferCode Like '%{0}%'", transferCode);
            }
            if(!string.IsNullOrEmpty(date) && date != "")
            {
                string fDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(qr.EntryDate as date)='{0}'", fDate);
            }

            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and qr.StateStatus ='{0}'", status);
            }
            {
                param += string.Format(@" and qr.EUserId ={0}", userId);
            }
            if (userId != null && userId > 0)
            {
                param += string.Format(@" and qr.EUserId ={0}", userId);
            }

            query = string.Format(@"Select qr.QRTRInfoId,qr.FloorId,pl.LineNumber 'FloorName',qr.AssemblyLineId,al.AssemblyLineName, 
qr.QCLineId,qc.QCName 'QCLineName',qr.RepairLineId,rl.RepairLineName,qr.QRCode,qr.StateStatus,qr.TransferCode,qr.EntryDate,
qr.UpdateDate
From [Production].dbo.tblQRCodeTransferToRepairInfo qr
Inner Join [Production].dbo.tblProductionLines pl on qr.FloorId = pl.LineId
Inner Join [Production].dbo.tblAssemblyLines al on qr.AssemblyLineId=al.AssemblyLineId
Inner Join [Production].dbo.tblQualityControl qc on qr.QCLineId = qc.QCId
Inner Join [Production].dbo.tblRepairLine rl on qr.RepairLineId = rl.RepairLineId
Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }
    }
}
