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
    public class TransferStockToPackagingLine2InfoBusiness : ITransferStockToPackagingLine2InfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IItemBusiness _itemBusiness;
        private readonly TransferStockToPackagingLine2InfoRepository _transferStockToPackagingLine2InfoRepository;
        private readonly ITransferStockToPackagingLine2DetailBusiness _transferStockToPackagingLine2DetailBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;

        public TransferStockToPackagingLine2InfoBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, ITransferStockToPackagingLine2DetailBusiness transferStockToPackagingLine2DetailBusiness)
        {
            this._productionDb = productionDb;
            this._itemBusiness = itemBusiness;
            this._transferStockToPackagingLine2InfoRepository = new TransferStockToPackagingLine2InfoRepository(this._productionDb);
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._transferStockToPackagingLine2DetailBusiness = transferStockToPackagingLine2DetailBusiness;
        }
        public TransferStockToPackagingLine2Info GetStockToPL2InfoById(long id, long orgId)
        {
            return _transferStockToPackagingLine2InfoRepository.GetOneByOrg(t => t.TP2InfoId == id && t.OrganizationId == orgId);
        }
        public IEnumerable<TransferStockToPackagingLine2Info> GetStockToPL2Infos(long orgId)
        {
            return _transferStockToPackagingLine2InfoRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
        public bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId)
        {
            bool IsSuccess = false;
            var transferInDb = GetStockToPL2InfoById(transferId, orgId);
            if(transferInDb != null && transferInDb.StateStatus == RequisitionStatus.Approved)
            {
                transferInDb.StateStatus = RequisitionStatus.Accepted;
                transferInDb.UpdateDate = DateTime.Now;
                transferInDb.UpUserId = userId;
                _transferStockToPackagingLine2InfoRepository.Update(transferInDb);

                var transferDetails = _transferStockToPackagingLine2DetailBusiness.GetTransferFromPLDetailByInfo(transferId, orgId);
                List<PackagingLineStockDetailDTO> stockDetails = new List<PackagingLineStockDetailDTO>();
                foreach (var item in transferDetails)
                {
                    PackagingLineStockDetailDTO stock = new PackagingLineStockDetailDTO()
                    {
                        ProductionLineId = transferInDb.LineId,
                        DescriptionId = transferInDb.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = "Stock In By Packaging Line Transfer (" + transferInDb.TransferCode + ")",
                        PackagingLineId = transferInDb.PackagingLineToId,
                        RefferenceNumber = transferInDb.TransferCode,
                        StockStatus = StockStatus.StockIn
                    };
                    stockDetails.Add(stock);
                }

                if (_transferStockToPackagingLine2InfoRepository.Save())
                {
                    IsSuccess = _packagingLineStockDetailBusiness.SavePackagingLineStockIn(stockDetails, userId, orgId);
                }
            }
            return IsSuccess;
        }

        public bool SaveTransferStockToPackaging2(TransferStockToPackagingLine2InfoDTO infoDto, List<TransferStockToPackagingLine2DetailDTO> detailDto, long userId, long orgId)
        {
            bool IsSuccess = false;
            TransferStockToPackagingLine2Info info = new TransferStockToPackagingLine2Info()
            {
                LineId = infoDto.LineId,
                DescriptionId = infoDto.DescriptionId,
                PackagingLineFromId = infoDto.PackagingLineFromId,
                PackagingLineToId = infoDto.PackagingLineToId,
                WarehouseId = infoDto.WarehouseId,
                ItemTypeId = infoDto.ItemTypeId,
                ItemId = infoDto.ItemId,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                ForQty = infoDto.ForQty,
                StateStatus = RequisitionStatus.Approved,
                Remarks = infoDto.Remarks,
                TransferCode = ("TPL-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
            List<TransferStockToPackagingLine2Detail> pl2details = new List<TransferStockToPackagingLine2Detail>();
            List<PackagingLineStockDetailDTO> stockDetails = new List<PackagingLineStockDetailDTO>();

            foreach (var item in detailDto)
            {
                TransferStockToPackagingLine2Detail pl2detail = new TransferStockToPackagingLine2Detail()
                {
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitId = _itemBusiness.GetItemById(item.ItemId.Value,orgId).UnitId,
                    OrganizationId= orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = item.Remarks
                };
                pl2details.Add(pl2detail);

                // Stock
                PackagingLineStockDetailDTO stock = new PackagingLineStockDetailDTO()
                {
                    ProductionLineId = info.LineId,
                    DescriptionId = info.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitId = pl2detail.UnitId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = "Stock Out By Packaging Line Transfer ("+ info.TransferCode + ")",
                    PackagingLineId = infoDto.PackagingLineFromId,
                    RefferenceNumber = info.TransferCode,
                    StockStatus = StockStatus.StockOut
                };
                stockDetails.Add(stock);
            }   
            info.TransferStockToPackagingLine2Details = pl2details;
            _transferStockToPackagingLine2InfoRepository.Insert(info);

            if (_transferStockToPackagingLine2InfoRepository.Save()) {
                IsSuccess = _packagingLineStockDetailBusiness.SavePackagingLineStockOut(stockDetails, userId, orgId, "");
            }

            return IsSuccess;
        }
    }
}
