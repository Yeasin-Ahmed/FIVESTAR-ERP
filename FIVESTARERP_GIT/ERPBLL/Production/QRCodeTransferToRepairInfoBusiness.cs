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
    public class QRCodeTransferToRepairInfoBusiness : IQRCodeTransferToRepairInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QRCodeTransferToRepairInfoRepository _qRCodeTransferToRepairInfoRepository;
        private readonly ITransferFromQCInfoBusiness _transferFromQCInfoBusiness;
        private readonly ITransferFromQCDetailBusiness _transferFromQCDetailBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly IQCItemStockInfoBusiness _qCItemStockInfoBusiness;
        private readonly IQCItemStockDetailBusiness _qCItemStockDetailBusiness;
        private readonly IQCLineStockInfoBusiness _qCLineStockInfoBusiness;
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly TransferFromQCInfoRepository _transferFromQCInfoRepository;
        private readonly TransferFromQCDetailRepository _transferFromQCDetailRepository;
        public QRCodeTransferToRepairInfoBusiness(IProductionUnitOfWork productionDb, ITransferFromQCInfoBusiness transferFromQCInfoBusiness, ITransferFromQCDetailBusiness transferFromQCDetailBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IQCItemStockInfoBusiness qCItemStockInfoBusiness, IQCItemStockDetailBusiness qCItemStockDetailBusiness, IQCLineStockInfoBusiness qCLineStockInfoBusiness, IQCLineStockDetailBusiness qCLineStockDetailBusiness, TransferFromQCDetailRepository transferFromQCDetailRepository)
        {
            this._productionDb = productionDb;
            this._transferFromQCInfoBusiness = transferFromQCInfoBusiness;
            this._transferFromQCDetailBusiness = transferFromQCDetailBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._qCItemStockInfoBusiness = qCItemStockInfoBusiness;
            this._qCItemStockDetailBusiness = qCItemStockDetailBusiness;
            this._qCLineStockInfoBusiness = qCLineStockInfoBusiness;
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._qRCodeTransferToRepairInfoRepository = new QRCodeTransferToRepairInfoRepository(this._productionDb);
            this._transferFromQCInfoRepository = new TransferFromQCInfoRepository(this._productionDb);
            this._transferFromQCDetailRepository = new TransferFromQCDetailRepository(this._productionDb);
        }

        public IEnumerable<QRCodeTransferToRepairInfo> GetQRCodeTransferToRepairInfoByTransferId(long transferId, long orgId)
        {
            throw new NotImplementedException();
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
            var transferInfo = await _transferFromQCInfoBusiness.GetNonReceivedTransferFromQCInfoByQRCodeKeyAsync(dto.QCLineId, dto.RepairLineId, dto.DescriptionId, dto.WarehouseId.Value, dto.ItemTypeId.Value, dto.ItemId.Value, orgId);

            // Item Preparation Info //
            var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Production, dto.DescriptionId, dto.ItemId.Value, orgId);

            // Item Preparation Detail //
            var itemPreparationDetail =(List<ItemPreparationDetail>) await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

            // Quanlity Control Line Raw Material Stock
            List<QualityControlLineStockDetailDTO> stockDetailDTOs = new List<QualityControlLineStockDetailDTO>();

            List<TransferFromQCDetail> transferDetails;
            if(transferInfo != null) // If there is a pending transfer information
            {
                transferInfo.ForQty += 1;
                code = transferInfo.TransferCode;
                transferId = transferInfo.TFQInfoId;
                transferDetails = (List<TransferFromQCDetail>)(await _transferFromQCDetailBusiness.GetTransferFromQCDetailByInfoAsync(transferInfo.TFQInfoId, orgId));

                for (int i = 0; i < transferDetails.Count; i++)
                {
                    // Update Transfer Info//
                    for (int j = 0; j < itemPreparationDetail.Count; j++)
                    {
                        if(transferDetails[i].ItemId == itemPreparationDetail[j].ItemId)
                        {
                            transferDetails[i].Quantity += itemPreparationDetail[j].Quantity;
                            QualityControlLineStockDetailDTO qualityControlLineStock = new QualityControlLineStockDetailDTO
                            {
                                ProductionLineId = dto.FloorId,
                                QCLineId = dto.QCLineId,
                                DescriptionId = dto.DescriptionId,
                                WarehouseId = transferDetails[i].WarehouseId,
                                ItemTypeId = transferDetails[i].ItemTypeId,
                                ItemId = transferDetails[i].ItemId,
                                OrganizationId = orgId,
                                EUserId = orgId,
                                Quantity = transferDetails[i].Quantity,
                                EntryDate = DateTime.Now,
                                UnitId = transferDetails[i].UnitId,
                                RefferenceNumber = code,
                                StockStatus = StockStatus.StockIn
                            };
                            stockDetailDTOs.Add(qualityControlLineStock);
                            break;
                        }
                    }
                }

                transferInfo.TransferFromQCDetails = transferDetails;
            }
            else
            {
                /// New Transfer //
                
                code = "TFQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

                transferInfo = new TransferFromQCInfo {
                    LineId = dto.FloorId,
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
                    TransferFor ="Repair"
                };

                transferDetails = new List<TransferFromQCDetail>();

                foreach (var item in itemPreparationDetail)
                {
                    TransferFromQCDetail transferDetail = new TransferFromQCDetail
                    {
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId= item.ItemId,
                        OrganizationId = orgId,
                        EUserId = orgId,
                        Quantity = item.Quantity,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId
                    };
                    transferDetails.Add(transferDetail);

                    QualityControlLineStockDetailDTO qualityControlLineStock = new QualityControlLineStockDetailDTO
                    {
                        ProductionLineId = dto.FloorId,
                        QCLineId = dto.QCLineId,
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
                    stockDetailDTOs.Add(qualityControlLineStock);
                }
                
                transferInfo.TransferFromQCDetails = transferDetails;
            }

            List<QCItemStockDetailDTO> qCItemStockDetails = new List<QCItemStockDetailDTO>() {
                new QCItemStockDetailDTO
                {
                    ProductionFloorId = dto.FloorId,
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
            // QC Item Stock //
            if (await _qCItemStockDetailBusiness.SaveQCItemStockOutAsync(qCItemStockDetails, userId, orgId))
            {
                // QC Line Stock //
                if (await _qCLineStockDetailBusiness.SaveQCLineStockOutAsync(stockDetailDTOs, userId, orgId, string.Empty))
                {
                    if(transferInfo.TFQInfoId == 0)
                    {
                        _transferFromQCInfoRepository.Insert(transferInfo);
                    }
                    else
                    {
                        _transferFromQCInfoRepository.Update(transferInfo);
                        _transferFromQCDetailRepository.UpdateAll(transferDetails);
                    }
                    if (await _transferFromQCInfoRepository.SaveAsync()) {

                        // QR Code //
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

            //----------------------------//
            // QC Item Stock Info -- Update
            // QC Item Stock Detail -- Insert
            // QC Raw Stock Info -- Update
            // QC Raw Stock Detail -- Insert
            // Transfer Info -- Insert/ Update
            // Transfer Detail -- Insert / Update
            // QRCode Info -- Insert
            // QRCode Problem -- Insert
            // Item Preparation
            // Item PreparationDetails
            return IsSuccess;
        }
    }
}
