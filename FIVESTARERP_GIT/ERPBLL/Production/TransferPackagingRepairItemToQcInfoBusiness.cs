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
    public class TransferPackagingRepairItemToQcInfoBusiness : ITransferPackagingRepairItemToQcInfoBusiness
    {
        // Database
        private readonly IProductionUnitOfWork _productionDb;
        // Business //
        private readonly IItemBusiness _itemBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly IIMEITransferToRepairInfoBusiness _iMEITransferToRepairInfoBusiness;
        private readonly IPackagingRepairItemStockDetailBusiness _packagingRepairItemStockDetailBusiness;
        private readonly IPackagingRepairRawStockDetailBusiness _packagingRepairRawStockDetailBusiness;
        private readonly ITransferPackagingRepairItemToQcDetailBusiness _transferPackagingRepairItemToQcDetailBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly IPackagingItemStockDetailBusiness _packagingItemStockDetailBusiness;

        // Repository
        private readonly TransferPackagingRepairItemToQcInfoRepository _transferPackagingRepairItemToQcInfoRepository;
        private readonly TransferPackagingRepairItemToQcDetailRepository _transferPackagingRepairItemToQcDetailRepository;
        private readonly IMEITransferToRepairInfoRepository _iMEITransferToRepairInfoRepository;

        public TransferPackagingRepairItemToQcInfoBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IIMEITransferToRepairInfoBusiness iMEITransferToRepairInfoBusiness, IPackagingRepairItemStockDetailBusiness packagingRepairItemStockDetailBusiness, IPackagingRepairRawStockDetailBusiness packagingRepairRawStockDetailBusiness, ITransferPackagingRepairItemToQcDetailBusiness transferPackagingRepairItemToQcDetailBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, IPackagingItemStockDetailBusiness packagingItemStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._itemBusiness = itemBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._iMEITransferToRepairInfoBusiness = iMEITransferToRepairInfoBusiness;
            this._packagingRepairItemStockDetailBusiness = packagingRepairItemStockDetailBusiness;
            this._packagingRepairRawStockDetailBusiness = packagingRepairRawStockDetailBusiness;
            this._transferPackagingRepairItemToQcDetailBusiness = transferPackagingRepairItemToQcDetailBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._packagingItemStockDetailBusiness = packagingItemStockDetailBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            // Repository
            this._transferPackagingRepairItemToQcInfoRepository = new TransferPackagingRepairItemToQcInfoRepository(this._productionDb);
            this._transferPackagingRepairItemToQcDetailRepository = new TransferPackagingRepairItemToQcDetailRepository(this._productionDb);
            this._iMEITransferToRepairInfoRepository = new IMEITransferToRepairInfoRepository(this._productionDb);
        }

        public async Task<TransferPackagingRepairItemToQcInfo> GetTransferPackagingRepairItemToQcInfoByIdAsync(long floorId, long packagingLineId, long modelId, long itemId, string status, long orgId)
        {
            return await this._transferPackagingRepairItemToQcInfoRepository.GetOneByOrgAsync(s => s.FloorId == floorId && s.PackagingLineId == packagingLineId && s.DescriptionId == modelId && s.ItemId == itemId && s.StateStatus == status && s.OrganizationId == orgId);
        }

        public async Task<TransferPackagingRepairItemToQcInfo> GetTransferPackagingRepairItemToQcInfoByIdAsync(long transferId, long orgId)
        {
            return await _transferPackagingRepairItemToQcInfoRepository.GetOneByOrgAsync(s => s.TPRQInfoId == transferId && s.OrganizationId == orgId);
        }

        // Transfer Receive //
        public async Task<bool> SavePackagingRapairToQCTransferInfoStateStatusAsync(long transferId, string status, long userId, long orgId)
        {
            //Transfer Info
            var transferInDb = await GetTransferPackagingRepairItemToQcInfoByIdAsync(transferId, orgId);
            var allItemInDb = _itemBusiness.GetAllItemByOrgId(orgId);
            if(transferInDb != null && transferInDb.StateStatus == RequisitionStatus.Approved && status == RequisitionStatus.Accepted)
            {
                transferInDb.StateStatus = RequisitionStatus.Accepted;
                transferInDb.UpdateDate = DateTime.Now;
                transferInDb.UpUserId = userId;

                //Transfer Detail
                var transferDetail = await _transferPackagingRepairItemToQcDetailBusiness.GetPackagingRepairItemToQcDetailsByTransferIdAsync(transferInDb.TPRQInfoId, orgId);

                // Item Preparation Info //
                var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Packaging, transferInDb.DescriptionId, transferInDb.ItemId, orgId);

                // Item Preparation Detail //
                var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

                // Packaging Line Stock //
                List<PackagingLineStockDetailDTO> packagingRawStocks = new List<PackagingLineStockDetailDTO>();
                foreach (var item in itemPreparationDetail)
                {
                    PackagingLineStockDetailDTO packagingRawStock = new PackagingLineStockDetailDTO()
                    {
                        ProductionLineId = transferInDb.FloorId,
                        PackagingLineId = transferInDb.PackagingLineId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity  = item.Quantity,
                        DescriptionId = transferInDb.DescriptionId,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        StockStatus = StockStatus.StockIn,
                        RefferenceNumber = transferInDb.TransferCode,
                        Remarks = "Stock In By Packaging Repair Transfer"
                    };
                    packagingRawStocks.Add(packagingRawStock);
                }

                // Packaging Item Stock //
                List<PackagingItemStockDetailDTO> packagingItemStocks = new List<PackagingItemStockDetailDTO>() {
                    new PackagingItemStockDetailDTO (){
                        ProductionFloorId = transferInDb.FloorId,
                        PackagingLineId = transferInDb.PackagingLineId,
                        WarehouseId = transferInDb.WarehouseId,
                        ItemTypeId = transferInDb.ItemTypeId,
                        ItemId = transferInDb.ItemId,
                        Quantity  = transferInDb.Quantity,
                        DescriptionId = transferInDb.DescriptionId,
                        UnitId = allItemInDb.FirstOrDefault(s=> s.ItemId == transferInDb.ItemId).UnitId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        StockStatus = StockStatus.StockIn,
                        ReferenceNumber = transferInDb.TransferCode,
                        Remarks = "Stock In By Packaging Repair Transfer"
                    }
                };

                // QRCode //
                var qrCodes = transferDetail.Select(s => s.QRCode).ToList();
                var getQRCodeForUpdate = await _tempQRCodeTraceBusiness.GetTempQRCodeTracesByQRCodesAsync(qrCodes, orgId);
                foreach (var item in getQRCodeForUpdate)
                {
                    item.StateStatus = QRCodeStatus.Packaging;
                    item.UpdateDate = DateTime.Now;
                    item.UpUserId = userId;
                }

                _transferPackagingRepairItemToQcInfoRepository.Update(transferInDb);
                if (await _transferPackagingRepairItemToQcInfoRepository.SaveAsync())
                {
                    if (await _packagingLineStockDetailBusiness.SavePackagingLineStockInAsync(packagingRawStocks, userId, orgId))
                    {
                        if (await _packagingItemStockDetailBusiness.SavePackagingItemStockInAsync(packagingItemStocks, userId, orgId))
                        {
                            return await _tempQRCodeTraceBusiness.UpdateQRCodeBatchAsync(getQRCodeForUpdate.ToList(), orgId);
                        }
                    }
                }
            }

            return false;
        }

        // Transfer //
        public async Task<bool> SaveTransferByIMEIScanningAsync(TransferPackagigRepairItemByIMEIScanningDTO dto, long user, long orgId)
        {
            string code = string.Empty;
            long transferId = 0;
            // Checking the IMEI is exist with the Receive Status
            var imeiInfoDto = _iMEITransferToRepairInfoBusiness.GetIMEIWiseItemInfo(string.Empty,dto.QRCode, string.Format(@"'Received'"), orgId);
            if(imeiInfoDto != null)
            {
                // Preivous Transfer Information
                var transferInfo = await GetTransferPackagingRepairItemToQcInfoByIdAsync(dto.FloorId, dto.PackagingLineId, dto.ModelId, dto.ItemId, RequisitionStatus.Approved, orgId);

                // Item Preparation Info //
                var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Packaging, dto.ModelId, dto.ItemId, orgId);

                // Item Preparation Detail //
                var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

                // All items in Db
                var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);

                List<TransferPackagingRepairItemToQcDetail> transferPackagingRepairItemDetails = new List<TransferPackagingRepairItemToQcDetail>() {
                    new  TransferPackagingRepairItemToQcDetail () {
                    OrganizationId= orgId,
                    EUserId = user,
                    EntryDate = DateTime.Now,
                    QRCode = imeiInfoDto.QRCode,
                    IMEI = imeiInfoDto.IMEI,
                    IncomingTransferId = imeiInfoDto.TransferId,
                    IncomingTransferCode = imeiInfoDto.TransferCode}};

                // If there is Pending Transfer 
                if (transferInfo != null && transferInfo.TPRQInfoId > 0)
                {
                    transferInfo.Quantity += 1;
                    transferInfo.UpUserId = user;
                    transferInfo.UpdateDate = DateTime.Now;
                    code = transferInfo.TransferCode;
                    transferId = transferInfo.TPRQInfoId;

                    foreach (var item in transferPackagingRepairItemDetails)
                    {
                        item.TPRQInfoId = transferInfo.TPRQInfoId;
                    }
                }
                else
                {
                    // If there is no Pending Transfer 
                    code = "TPRQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                    transferInfo = new TransferPackagingRepairItemToQcInfo()
                    {
                        FloorId = imeiInfoDto.ProductionFloorId,
                        PackagingLineId = imeiInfoDto.PackagingLineId,
                        Quantity = 1,
                        StateStatus = RequisitionStatus.Approved,
                        DescriptionId = dto.ModelId,
                        ItemId = dto.ItemId,
                        ItemTypeId = imeiInfoDto.ItemTypeId.Value,
                        WarehouseId = imeiInfoDto.WarehouseId,
                        EUserId = user,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        TransferCode = code
                    };
                    transferInfo.TransferPackagingRepairItemToQcDetails = transferPackagingRepairItemDetails;
                }

                // Packaging Repair Item Stock //
                List<PackagingRepairItemStockDetailDTO> itemStocks = new List<PackagingRepairItemStockDetailDTO>() {
                    new PackagingRepairItemStockDetailDTO()
                    {
                        FloorId = imeiInfoDto.ProductionFloorId,
                        PackagingLineId = imeiInfoDto.PackagingLineId,
                        DescriptionId = imeiInfoDto.DescriptionId,
                        WarehouseId = imeiInfoDto.WarehouseId,
                        ItemTypeId = imeiInfoDto.ItemTypeId,
                        ItemId = imeiInfoDto.ItemId,
                        Quantity = 1,
                        UnitId = allItemsInDb.FirstOrDefault(s=> s.ItemId == imeiInfoDto.ItemId).UnitId,
                        OrganizationId = orgId,
                        EUserId = user,
                        EntryDate = DateTime.Now,
                        RefferenceNumber = code,
                        StockStatus = StockStatus.StockOut,
                        Remarks ="Stock Out By Packaging Repair For QC Transfer"
                    }
                };
                // Packaging Repair Raw Stock //

                List<PackagingRepairRawStockDetailDTO> rawStocks = new List<PackagingRepairRawStockDetailDTO>();

                foreach (var item in itemPreparationDetail)
                {
                    PackagingRepairRawStockDetailDTO rawStock = new PackagingRepairRawStockDetailDTO()
                    {
                        FloorId = imeiInfoDto.ProductionFloorId,
                        PackagingLineId = imeiInfoDto.PackagingLineId,
                        DescriptionId = imeiInfoDto.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitId = allItemsInDb.FirstOrDefault(s => s.ItemId == item.ItemId).UnitId,
                        OrganizationId = orgId,
                        EUserId = user,
                        EntryDate = DateTime.Now,
                        RefferenceNumber = code,
                        StockStatus = StockStatus.StockOut,
                        Remarks = "Stock Out By Packaging Repair For QC Transfer"
                    };
                    rawStocks.Add(rawStock);
                }

                var imeiInfoInDb = await _iMEITransferToRepairInfoBusiness.GetIMEITransferToRepairInfosByTransferIdAsync(imeiInfoDto.IMEITRInfoId, orgId);
                imeiInfoInDb.StateStatus = "Repair-Done";
                imeiInfoInDb.UpdateDate = DateTime.Now;
                imeiInfoInDb.UpUserId = user;
                _iMEITransferToRepairInfoRepository.Update(imeiInfoInDb);

                if (transferInfo.TPRQInfoId > 0)
                {
                    _transferPackagingRepairItemToQcInfoRepository.Update(transferInfo);
                    _transferPackagingRepairItemToQcDetailRepository.InsertAll(transferPackagingRepairItemDetails);
                }
                else
                {
                    _transferPackagingRepairItemToQcInfoRepository.Insert(transferInfo);
                }

                if(await _transferPackagingRepairItemToQcInfoRepository.SaveAsync())
                {
                    if (await _packagingRepairRawStockDetailBusiness.SavePackagingRepairRawStockOutAsync(rawStocks, user, orgId))
                    {
                       return await _packagingRepairItemStockDetailBusiness.SavePackagingRepairItemStockOutAsync(itemStocks, user, orgId);
                    }
                }
            }
            return false;
        }

        // Transfer LIst //
        public IEnumerable<TransferPackagingRepairItemToQcInfoDTO> GetTransferPackagingRepairItemToQcInfosByQuery(long? floorId, long? packagingLine, long?  modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferPackagingRepairItemToQcInfoDTO>(QueryForTransferPackagingRepairItemToQcInfos(floorId, packagingLine, modelId, warehouseId, itemTypeId, itemId, status, transferCode, fromDate, toDate, orgId)).ToList();
        }

        private string QueryForTransferPackagingRepairItemToQcInfos(long? floorId, long? packagingLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param = string.Format(@" and ti.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and ti.FloorId={0}", floorId);
            }
            if (packagingLine != null && packagingLine > 0)
            {
                param += string.Format(@" and ti.PackagingLineId={0}", packagingLine);
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

            query = string.Format(@"Select ti.TPRQInfoId,ti.TransferCode,ti.FloorId,pl.LineNumber 'ProductionFloorName',ti.PackagingLineId, pac.PackagingLineName 'PackagingLineName',ti.DescriptionId,de.DescriptionName 'ModelName',
 ti.WarehouseId ,w.WarehouseName,ti.ItemTypeId, it.ItemName 'ItemTypeName',ti.ItemId,i.ItemName,
 ti.StateStatus,ti.Quantity,app.UserName 'EntryUser',ti.EntryDate,
(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId=ti.UpUserId) 'UpdateUser',
ti.UpdateDate
From [Production].dbo.tblTransferPackagingRepairItemToQcInfo ti
Inner Join [Inventory].dbo.tblDescriptions de on ti.DescriptionId = de.DescriptionId
Inner Join [Production].dbo.tblProductionLines pl on ti.FloorId= pl.LineId
Inner Join [Production].dbo.tblPackagingLine pac on ti.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblWarehouses w on ti.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on ti.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on ti.ItemId = i.ItemId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on ti.EUserId = app.UserId
Where 1= 1 {0}", param);

            return query;
        }
    }
}
