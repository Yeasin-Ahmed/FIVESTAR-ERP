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
    public class IMEITransferToRepairInfoBusiness : IIMEITransferToRepairInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly IPackagingItemStockDetailBusiness _packagingItemStockDetailBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly ITransferToPackagingRepairInfoBusiness _transferToPackagingRepairInfoBusiness;
        private readonly IItemBusiness _itemBusiness;

        // Repository //
        private readonly IMEITransferToRepairInfoRepository _iMEITransferToRepairInfoRepository;
        private readonly TransferToPackagingRepairInfoRepository _transferToPackagingRepairInfoRepository;
        public IMEITransferToRepairInfoBusiness(IProductionUnitOfWork productionDb, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IPackagingItemStockDetailBusiness packagingItemStockDetailBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, ITransferToPackagingRepairInfoBusiness transferToPackagingRepairInfoBusiness, IItemBusiness itemBusiness)
        {
            // Database //
            this._productionDb = productionDb;
            // Business
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._packagingItemStockDetailBusiness = packagingItemStockDetailBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._transferToPackagingRepairInfoBusiness = transferToPackagingRepairInfoBusiness;
            this._itemBusiness = itemBusiness;
            // Repository //
            this._iMEITransferToRepairInfoRepository = new IMEITransferToRepairInfoRepository(this._productionDb);
            this._transferToPackagingRepairInfoRepository = new TransferToPackagingRepairInfoRepository(this._productionDb);
        }

        public IEnumerable<IMEITransferToRepairInfoDTO> GetIMEITransferToRepairInfosByQuery(long? transferId, string qrCode, string imei, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, long orgId)
        {

            return this._productionDb.Db.Database.SqlQuery<IMEITransferToRepairInfoDTO>(IMEITransferToRepairInfosQuery(transferId, qrCode, imei, floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, transferCode, orgId)).ToList();
        }

        private string IMEITransferToRepairInfosQuery(long? transferId, string qrCode, string imei, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            if (transferId != null && transferId > 0)
            {
                param += string.Format(@" and imei.TransferId={0}", transferId);
            }
            if (!string.IsNullOrEmpty(qrCode) && qrCode.Trim() !="")
            {
                param += string.Format(@" and imei.QRCode='{0}'", qrCode);
            }
            if (!string.IsNullOrEmpty(imei) && imei.Trim() != "")
            {
                param += string.Format(@" and imei.IMEI='{0}'", imei);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and imei.ProductionFloorId={0}", floorId);
            }
            if (packagingLineId != null && packagingLineId > 0)
            {
                param += string.Format(@" and imei.PackagingLineId={0}", packagingLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and imei.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and imei.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and imei.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and imei.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and imei.TransferCode LIKE'%{0}%'", transferCode);
            }

            query = string.Format(@"Select imei.IMEITRInfoId,imei.TransferId,imei.TransferCode,imei.ProductionFloorId,pl.LineNumber 'ProductionFloorName',imei.PackagingLineId,pac.PackagingLineName,imei.DescriptionId,de.DescriptionName 'ModelName',
imei.WarehouseId,wa.WarehouseName,imei.ItemTypeId,it.ItemName 'ItemTypeName',imei.ItemId,i.ItemName,imei.QRCode,imei.IMEI From [Production].dbo.tblIMEITransferToRepairInfo imei
Left Join [Production].dbo.tblProductionLines pl on imei.ProductionFloorId = pl.LineId
Left Join [Production].dbo.tblPackagingLine  pac on imei.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblDescriptions de on imei.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses wa on imei.WarehouseId = wa.Id
Left Join [Inventory].dbo.tblItemTypes it on imei.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on imei.ItemId = i.ItemId
Where 1= 1 and imei.OrganizationId={0} {1}", orgId,Utility.ParamChecker(param));
            return query;
        }

        public async Task<bool> SaveIMEITransferToRepairInfoAsync(string imei, List<QRCodeProblemDTO> problems, long userId, long orgId)
        {
            // Checking Transfer //
            bool IsSuccess = false;
            string code = string.Empty;
            long transferId = 0;

            var imeiItem = _tempQRCodeTraceBusiness.GetTempQRCodeTraceByIMEI(imei, orgId);

            // Previous transfer info
            var transferInfo = await _transferToPackagingRepairInfoBusiness.GetNonReceivedTransferToPackagingRepairInfoAsync(imeiItem.ProductionFloorId.Value, imeiItem.PackagingLineId.Value, imeiItem.DescriptionId.Value, imeiItem.ItemId.Value, orgId);

            var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);

            // Item Preparation Info //
            var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Packaging, imeiItem.DescriptionId.Value, imeiItem.ItemId.Value, orgId);

            // Item Preparation Detail //
            var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

            List<PackagingLineStockDetailDTO> packagingLineStocks = new List<PackagingLineStockDetailDTO>();
            List<TransferToPackagingRepairDetail> transferDetails = new List<TransferToPackagingRepairDetail>();

            if (transferInfo != null)
            {
                transferInfo.Quantity++;
                //transferInfo.UpUserId = userId;
                //transferInfo.UpdateDate = DateTime.Now;
                code = transferInfo.TransferCode;
                foreach (var item in itemPreparationDetail)
                {
                    PackagingLineStockDetailDTO packagingLineStock = new PackagingLineStockDetailDTO
                    {
                        ProductionLineId = imeiItem.ProductionFloorId,
                        PackagingLineId = imeiItem.PackagingLineId,
                        DescriptionId = imeiItem.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = orgId,
                        Quantity = item.Quantity,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId,
                        RefferenceNumber = code,
                        Remarks= "Stock Out By Pakaging Line QC",
                        StockStatus = StockStatus.StockOut
                    };
                    packagingLineStocks.Add(packagingLineStock);
                }
            }
            else
            {
                code = "TPR-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

                transferInfo = new TransferToPackagingRepairInfo()
                {
                    TransferCode = code,
                    ProductionFloorId = imeiItem.ProductionFloorId.Value,
                    PackagingLineId = imeiItem.PackagingLineId.Value,
                    DescriptionId = imeiItem.DescriptionId.Value,
                    WarehouseId = imeiItem.WarehouseId.Value,
                    ItemTypeId = imeiItem.ItemTypeId.Value,
                    ItemId = imeiItem.ItemId.Value,
                    UnitId = allItemsInDb.FirstOrDefault(s => s.ItemId == imeiItem.ItemId).UnitId,
                    StateStatus = RequisitionStatus.Approved,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Quantity = 1,
                    Remarks = "",
                    OrganizationId = orgId
                };
                
                foreach (var item in itemPreparationDetail)
                {
                    PackagingLineStockDetailDTO packagingLineStock = new PackagingLineStockDetailDTO
                    {
                        ProductionLineId = imeiItem.ProductionFloorId,
                        PackagingLineId = imeiItem.PackagingLineId,
                        DescriptionId = imeiItem.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = orgId,
                        Quantity = item.Quantity,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId,
                        RefferenceNumber = code,
                        StockStatus = StockStatus.StockOut,
                        Remarks = "Stock Out By Pakaging Line QC"
                    };
                    packagingLineStocks.Add(packagingLineStock);

                    TransferToPackagingRepairDetail transferDetail = new TransferToPackagingRepairDetail()
                    {
                        ProductionFloorId = imeiItem.ProductionFloorId.Value,
                        PackagingLineId = imeiItem.PackagingLineId.Value,
                        DescriptionId = imeiItem.DescriptionId.Value,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = orgId,
                        Quantity = item.Quantity,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId,
                        TransferCode = code,
                        Remarks = "Stock Out By Pakaging Line QC"
                    };
                    transferDetails.Add(transferDetail);
                }
                transferInfo.TransferToPackagingRepairDetails = transferDetails;
            }
            List<PackagingItemStockDetailDTO> packagingItemStocks = new List<PackagingItemStockDetailDTO>() {
                new PackagingItemStockDetailDTO(){
                    ProductionFloorId= imeiItem.ProductionFloorId,
                    PackagingLineId = imeiItem.PackagingLineId,
                    DescriptionId = imeiItem.DescriptionId,
                    WarehouseId = imeiItem.WarehouseId,
                    ItemTypeId = imeiItem.ItemTypeId,
                    ItemId = imeiItem.ItemId,
                    Quantity = 1,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    UnitId = allItemsInDb.FirstOrDefault(s=> s.ItemId == imeiItem.ItemId).UnitId,
                    ReferenceNumber =code,
                    StockStatus = StockStatus.StockIn,
                    Remarks= "Stock Out By Pakaging Line QC"
                }
            };

            IMEITransferToRepairInfo iMEITransferToRepairInfo = new IMEITransferToRepairInfo()
            {
                ProductionFloorId = imeiItem.ProductionFloorId.Value,
                PackagingLineId = imeiItem.PackagingLineId.Value,
                DescriptionId = imeiItem.DescriptionId.Value,
                WarehouseId = imeiItem.WarehouseId.Value,
                ItemTypeId = imeiItem.ItemTypeId.Value,
                ItemId = imeiItem.ItemId.Value,
                QRCode = imeiItem.CodeNo,
                IMEI = imeiItem.IMEI,
                EUserId = userId,
                EntryDate = DateTime.Now,
                OrganizationId = orgId,
                StateStatus = "Send",
                TransferCode = code
            };
            List<IMEITransferToRepairDetail> iMEITransferToRepairDetails = new List<IMEITransferToRepairDetail>();

            foreach (var item in problems)
            {
                IMEITransferToRepairDetail iMEITransferToRepairDetail = new IMEITransferToRepairDetail()
                {
                    ProductionFloorId = imeiItem.ProductionFloorId.Value,
                    PackagingLineId = imeiItem.PackagingLineId.Value,
                    DescriptionId = imeiItem.DescriptionId.Value,
                    WarehouseId = imeiItem.WarehouseId.Value,
                    ItemTypeId = imeiItem.ItemTypeId.Value,
                    ItemId= imeiItem.ItemId.Value,
                    UnitId = allItemsInDb.FirstOrDefault(s => s.ItemId == imeiItem.ItemId).UnitId,
                    IMEI = imeiItem.IMEI,
                    QRCode = imeiItem.CodeNo,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Quantity = 1,
                    Remarks="",
                    ProblemId = item.ProblemId,
                    ProblemName = item.ProblemName
                };
                iMEITransferToRepairDetails.Add(iMEITransferToRepairDetail);
            }
            iMEITransferToRepairInfo.IMEITransferToRepairDetails = iMEITransferToRepairDetails;

            if(transferInfo.TPRInfoId == 0)
            {
                _transferToPackagingRepairInfoRepository.Insert(transferInfo);
            }
            else
            {
                _transferToPackagingRepairInfoRepository.Update(transferInfo);
            }

            if(await _transferToPackagingRepairInfoRepository.SaveAsync())
            {
                if (await _packagingItemStockDetailBusiness.SavePackagingItemStockOutAsync(packagingItemStocks, userId, orgId))
                {
                    if (await _packagingLineStockDetailBusiness.SavePackagingLineStockOutAsync(packagingLineStocks, userId, orgId, string.Empty))
                    {
                        if (await _tempQRCodeTraceBusiness.UpdateQRCodeStatusAsync(imeiItem.CodeNo, QRCodeStatus.PackagingRepair, orgId))
                        {
                            transferId = transferInfo.TPRInfoId;
                            iMEITransferToRepairInfo.TransferId = transferId;
                            _iMEITransferToRepairInfoRepository.Insert(iMEITransferToRepairInfo);
                            await _iMEITransferToRepairInfoRepository.SaveAsync();
                        }
                    }
                }
            }
            return IsSuccess;
        }

        public IEnumerable<IMEITransferToRepairInfo> GetIMEITransferToRepairInfosByTransferId(long transferId, long orgId)
        {
            return _iMEITransferToRepairInfoRepository.GetAll(s => s.TransferId == transferId && s.OrganizationId == orgId);
        }

        public bool SaveIMEIStatusByTransferInfoId(long transferId, string status, long userId, long orgId)
        {
            if (_transferToPackagingRepairInfoBusiness.SavePakagingQCTransferStateStatus(transferId, status, userId, orgId))
            {
                var imeiByTransferInfoInDb = GetIMEITransferToRepairInfosByTransferId(transferId, orgId);
                foreach (var item in imeiByTransferInfoInDb)
                {
                    item.StateStatus = FinishGoodsSendStatus.Received;
                    item.UpdateDate = DateTime.Now;
                    item.UpUserId = userId;
                    _iMEITransferToRepairInfoRepository.Update(item);
                }
            }
            return _iMEITransferToRepairInfoRepository.Save();
        }
    }
}
