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
        private readonly IPackagingRepairItemStockInfoBusiness _packagingRepairItemStockInfoBusiness;
        // Repository
        private readonly TransferPackagingRepairItemToQcInfoRepository _transferPackagingRepairItemToQcInfoRepository;

        public TransferPackagingRepairItemToQcInfoBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IIMEITransferToRepairInfoBusiness iMEITransferToRepairInfoBusiness, IPackagingRepairItemStockDetailBusiness packagingRepairItemStockDetailBusiness, IPackagingRepairItemStockInfoBusiness packagingRepairItemStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._itemBusiness = itemBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._iMEITransferToRepairInfoBusiness = iMEITransferToRepairInfoBusiness;
            this._packagingRepairItemStockInfoBusiness = packagingRepairItemStockInfoBusiness;
            this._packagingRepairItemStockDetailBusiness = packagingRepairItemStockDetailBusiness;
            // Repository
            this._transferPackagingRepairItemToQcInfoRepository = new TransferPackagingRepairItemToQcInfoRepository(this._productionDb);
        }

        public async Task<TransferPackagingRepairItemToQcInfo> GetTransferPackagingRepairItemToQcInfoByIdAsync(long floorId, long packagingLineId, long modelId, long itemId, string status, long orgId)
        {
            return await this._transferPackagingRepairItemToQcInfoRepository.GetOneByOrgAsync(s => s.FloorId == floorId && s.PackagingLineId == packagingLineId && s.DescriptionId == modelId && s.ItemId == itemId && s.StateStatus == status && s.OrganizationId == orgId);
        }

        public async Task<bool> SaveTransferByQRCodeScanningAsync(TransferPackagigRepairItemByIMEIScanningDTO dto, long user, long orgId)
        {
            string code = string.Empty;
            long transferId = 0;
            // Checking the QRCode is exist with the Receive Status
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
            }
            throw new NotImplementedException();
        }
    }
}
