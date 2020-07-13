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
    public class ProductionToPackagingStockTransferInfoBusiness : IProductionToPackagingStockTransferInfoBusiness
    {
        private readonly ProductionToPackagingStockTransferInfoRepository _productionToPackagingStockTransferInfoRepository;
        private readonly ProductionStockDetailBusiness _productionStockDetailBusiness;
        private readonly ProductionAssembleStockDetailBusiness _productionAssembleStockDetailBusiness;
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IItemBusiness _itemBusiness;
        private readonly IPackagingItemStockDetailBusiness _packagingItemStockDetailBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly IProductionToPackagingStockTransferDetailBusiness _productionToPackagingStockTransferDetailBusiness;
        public ProductionToPackagingStockTransferInfoBusiness(IProductionUnitOfWork productionDb, ProductionStockDetailBusiness productionStockDetailBusiness, ProductionAssembleStockDetailBusiness productionAssembleStockDetailBusiness, IItemBusiness itemBusiness, IPackagingItemStockDetailBusiness packagingItemStockDetailBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, IProductionToPackagingStockTransferDetailBusiness productionToPackagingStockTransferDetailBusiness)
        {
            this._productionDb = productionDb;
            this._productionToPackagingStockTransferInfoRepository = new ProductionToPackagingStockTransferInfoRepository(this._productionDb);
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._productionAssembleStockDetailBusiness = productionAssembleStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._packagingItemStockDetailBusiness = packagingItemStockDetailBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._productionToPackagingStockTransferDetailBusiness = productionToPackagingStockTransferDetailBusiness;
        }

        public ProductionToPackagingStockTransferInfo GetProductionToPackagingStockTransferInfoById(long transferInfoId, long orgId)
        {
            return _productionToPackagingStockTransferInfoRepository.GetOneByOrg(s => s.PTPSTInfoId == transferInfoId && s.OrganizationId == orgId);
        }

        public IEnumerable<ProductionToPackagingStockTransferInfo> GetProductionToPackagingStockTransferInfos(long orgId)
        {
            return _productionToPackagingStockTransferInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<ProductionToPackagingStockTransferInfoDTO> GetProductionToPackagingStockTransferInfosByQuery(long? floorId, long? packagingId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status,long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<ProductionToPackagingStockTransferInfoDTO>(QueryProductionToPackagingStockTransferInfos(floorId,packagingId,modelId,warehouseId,itemTypeId,itemId,status,orgId)).ToList();
        }

        private string QueryProductionToPackagingStockTransferInfos(long? floorId, long? packagingId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param = string.Format(@" and trans.OrganizationId={0}", orgId);
            if(floorId !=null && floorId > 0)
            {
                param = string.Format(@" and trans.FloorId={0}", floorId);
            }
            if (packagingId != null && packagingId > 0)
            {
                param = string.Format(@" and trans.PackagingLineId={0}", packagingId);
            }
            if (modelId != null && modelId > 0)
            {
                param = string.Format(@" and trans.ModelId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param = string.Format(@" and trans.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param = string.Format(@" and trans.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param = string.Format(@" and trans.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                param = string.Format(@" and trans.StateStatus='{0}'", status);
            }

            query = string.Format(@"Select trans.PTPSTInfoId,trans.TransferCode,pl.LineNumber 'FloorName',pack.PackagingLineName,de.DescriptionName 'ModelName',
w.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName,trans.Quantity,trans.StateStatus,
trans.EntryDate,app.UserName 'EntryUser',trans.UpdateDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = trans.UpUserId) 'UpdateUser'
From tblProductionToPackagingStockTransferInfo trans
Inner Join tblProductionLines pl on trans.FloorId = pl.LineId
Inner Join tblPackagingLine pack on trans.PackagingLineId =pack.PackagingLineId
Inner Join [Inventory].dbo.tblWarehouses w on trans.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItems i on trans.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblItemTypes it on trans.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblDescriptions de on trans.ModelId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on trans.EUserId = app.UserId and app.OrganizationId={1}
Where 1=1 {0} Order By trans.EntryDate desc", Utility.ParamChecker(param),orgId);
            return query;
        }

        public bool SaveProductionToPackagingStockTransfer(ProductionToPackagingStockTransferInfoDTO transferInfoDto, long userId, long orgId)
        {
            string code = (DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));

            var items = _itemBusiness.GetAllItemByOrgId(orgId);
            // Transfer Info
            ProductionToPackagingStockTransferInfo info = new ProductionToPackagingStockTransferInfo()
            {
                FloorId = transferInfoDto.FloorId,
                PackagingLineId = transferInfoDto.PackagingLineId,
                ModelId = transferInfoDto.ModelId,
                WarehouseId = transferInfoDto.WarehouseId,
                ItemTypeId = transferInfoDto.ItemTypeId,
                ItemId = transferInfoDto.ItemId,
                Quantity = transferInfoDto.Quantity,
                Remarks = transferInfoDto.Remarks,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                TransferCode = code,
                StateStatus = RequisitionStatus.Approved
            };

            long infoUnitId = items.FirstOrDefault(s => s.ItemId == transferInfoDto.ItemId) != null ? items.FirstOrDefault(s => s.ItemId == transferInfoDto.ItemId).UnitId : 0;

            List <ProductionAssembleStockDetailDTO> productionAssembleStocks = new List<ProductionAssembleStockDetailDTO>()
            {
                new ProductionAssembleStockDetailDTO()
                {
                    ProductionFloorId = transferInfoDto.FloorId,
                    PackagingLineId = transferInfoDto.PackagingLineId,
                    DescriptionId = transferInfoDto.ModelId,
                    WarehouseId = transferInfoDto.WarehouseId,
                    ItemTypeId = transferInfoDto.ItemTypeId,
                    ItemId = transferInfoDto.ItemId,
                    Quantity = transferInfoDto.Quantity,
                    Remarks = "Stock By Packaging Transfer",
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    UnitId = infoUnitId,
                    RefferenceNumber= code
                }
            };

            List<ProductionToPackagingStockTransferDetail> transferDetails = new List<ProductionToPackagingStockTransferDetail>();
            List<ProductionStockDetailDTO> productionStocks = new List<ProductionStockDetailDTO>();

            foreach (var item in transferInfoDto.ProductionToPackagingStockTransferDetails)
            {
                // Transfer Details
                ProductionToPackagingStockTransferDetail transferDetail = new ProductionToPackagingStockTransferDetail()
                {
                    FloorId = info.FloorId,
                    PackagingLineId = info.PackagingLineId,
                    ModelId = info.ModelId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    TransferCode = code,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks
                };
                transferDetails.Add(transferDetail);

                // Production Line Stock
                var unitId = items.FirstOrDefault(s => s.ItemId == item.ItemId) != null ? items.FirstOrDefault(s => s.ItemId == item.ItemId).UnitId : 0;
                ProductionStockDetailDTO productionStock = new ProductionStockDetailDTO()
                {
                    LineId = info.FloorId,
                    DescriptionId = info.ModelId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    RefferenceNumber = code,
                    Quantity = item.Quantity,
                    Remarks = "Stock By Packaging Transfer",
                    StockFor = ItemPreparationType.Packaging,
                    StockStatus = StockStatus.StockIn,
                    UnitId = unitId
                };
                productionStocks.Add(productionStock);
            }
            info.ProductionToPackagingStockTransferDetails = transferDetails;

            if (_productionStockDetailBusiness.SaveProductionStockOut(productionStocks, userId, orgId, string.Empty))
            {
                if (_productionAssembleStockDetailBusiness.SaveProductionAssembleStockDetailStockOut(productionAssembleStocks, userId, orgId))
                {
                    _productionToPackagingStockTransferInfoRepository.Insert(info);
                    return _productionToPackagingStockTransferInfoRepository.Save();
                }
            }
            return false;
        }

        public bool SaveProductionToPackagingStockTransferState(long transferInfoId, string status, long userId, long orgId)
        {
            var transferInfo = GetProductionToPackagingStockTransferInfoById(transferInfoId, orgId);
            if (transferInfo != null)
            {
                if (transferInfo.StateStatus == RequisitionStatus.Approved && status == RequisitionStatus.Accepted)
                {
                    transferInfo.StateStatus = RequisitionStatus.Accepted;
                    transferInfo.UpdateDate = DateTime.Now;
                    transferInfo.UpUserId = userId;
                    _productionToPackagingStockTransferInfoRepository.Update(transferInfo);

                    var transferDetails = _productionToPackagingStockTransferDetailBusiness.GetProductionToPackagingStockTransferDetailsByInfoId(transferInfoId, orgId);

                    // Packaging Line Stock
                    List<PackagingLineStockDetailDTO> lineStocks = new List<PackagingLineStockDetailDTO>();

                    // Packaging Item Stock
                    List<PackagingItemStockDetailDTO> itemStocks = new List<PackagingItemStockDetailDTO>() {
                        new PackagingItemStockDetailDTO(){
                            ProductionFloorId = transferInfo.FloorId,
                            PackagingLineId = transferInfo.PackagingLineId,
                            DescriptionId = transferInfo.ModelId,
                            WarehouseId = transferInfo.WarehouseId,
                            ItemTypeId = transferInfo.ItemTypeId,
                            ItemId = transferInfo.ItemId,
                            OrganizationId= orgId,
                            StockStatus = StockStatus.StockIn,
                            ReferenceNumber = transferInfo.TransferCode,
                            Quantity = transferInfo.Quantity,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            Remarks = "Stock In By Production Assemble Stock"
                        }
                    };

                    var items = _itemBusiness.GetAllItemByOrgId(orgId);
                    foreach (var item in transferDetails)
                    {
                        PackagingLineStockDetailDTO lineStock = new PackagingLineStockDetailDTO()
                        {
                            ProductionLineId = item.FloorId,
                            PackagingLineId = item.PackagingLineId,
                            DescriptionId = item.ModelId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            Quantity = item.Quantity,
                            StockStatus = StockStatus.StockIn,
                            OrganizationId = orgId,
                            EUserId = userId,
                            UnitId = items.FirstOrDefault(s=> s.ItemId == item.ItemId).UnitId,
                            RefferenceNumber = transferInfo.TransferCode,
                            Remarks = "Stock In By Production Assemble Stock"
                        };
                        lineStocks.Add(lineStock);
                    }

                    if (_productionToPackagingStockTransferInfoRepository.Save())
                    {
                        if (_packagingLineStockDetailBusiness.SavePackagingLineStockIn(lineStocks, userId, orgId))
                        {
                            return _packagingItemStockDetailBusiness.SavePackagingItemStockIn(itemStocks, userId, orgId);

                        }
                    }
                }
            }
            return false;
        }
    }
}
