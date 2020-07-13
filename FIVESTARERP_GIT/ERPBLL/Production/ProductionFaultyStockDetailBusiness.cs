using ERPBLL.Common;
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
    public class ProductionFaultyStockDetailBusiness : IProductionFaultyStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly ProductionFaultyStockDetailRepository _productionFaultyStockDetailRepository;
        private readonly ProductionFaultyStockInfoRepository _productionFaultyStockInfoRepository;
        private readonly IProductionFaultyStockInfoBusiness _productionFaultyStockInfoBusiness;
        private readonly IRepairSectionFaultyItemTransferInfoBusiness _repairSectionFaultyItemTransferInfoBusiness;
        private readonly IRepairSectionFaultyItemTransferDetailBusiness _repairSectionFaultyItemTransferDetailBusiness;
        private readonly RepairSectionFaultyItemTransferInfoRepository _repairSectionFaultyItemTransferInfoRepository;

        public ProductionFaultyStockDetailBusiness(IProductionUnitOfWork productionDb, IProductionFaultyStockInfoBusiness productionFaultyStockInfoBusiness, IRepairSectionFaultyItemTransferInfoBusiness repairSectionFaultyItemTransferInfoBusiness, IRepairSectionFaultyItemTransferDetailBusiness repairSectionFaultyItemTransferDetailBusiness)
        {
            this._productionDb = productionDb;
            this._productionFaultyStockDetailRepository = new ProductionFaultyStockDetailRepository(this._productionDb);
            this._productionFaultyStockInfoRepository = new ProductionFaultyStockInfoRepository(this._productionDb);
            this._repairSectionFaultyItemTransferInfoRepository = new RepairSectionFaultyItemTransferInfoRepository(this._productionDb);

            this._productionFaultyStockInfoBusiness = productionFaultyStockInfoBusiness;
            this._repairSectionFaultyItemTransferInfoBusiness = repairSectionFaultyItemTransferInfoBusiness;
            this._repairSectionFaultyItemTransferDetailBusiness = repairSectionFaultyItemTransferDetailBusiness;
        }

        public IEnumerable<ProductionFaultyStockDetail> GetProductionFaultyInfoStocks(long orgId)
        {
            return this._productionFaultyStockDetailRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public bool SaveProductionFaultyStockIn(List<ProductionFaultyStockDetailDTO> stockDetailsDTO, long userId, long orgId)
        {
            List<ProductionFaultyStockDetail> stockDetails = new List<ProductionFaultyStockDetail>();
            foreach (var item in stockDetailsDTO)
            {
                ProductionFaultyStockDetail stockDetail = new ProductionFaultyStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    RepairLineId = item.RepairLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    StockStatus = StockStatus.StockIn,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = "Faulty Stock In By Repair Section"
                };
                stockDetails.Add(stockDetail);

                var info = _productionFaultyStockInfoBusiness.GetProductionFaultyStockInfoByFloorAndModelAndItem(item.ProductionFloorId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);

                if (info != null)
                {
                    info.StockInQty += item.Quantity;
                    info.UpUserId = userId;
                    info.UpdateDate = DateTime.Now;
                    this._productionFaultyStockInfoRepository.Update(info);
                }
                else
                {
                    ProductionFaultyStockInfo newInfo = new ProductionFaultyStockInfo()
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        DescriptionId = item.DescriptionId,
                        RepairLineId = item.RepairLineId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        StockInQty = item.Quantity,
                        Remarks = "Faulty Stock In By Repair Section"
                    };
                    this._productionFaultyStockInfoRepository.Insert(newInfo);
                }
            }
            this._productionFaultyStockDetailRepository.InsertAll(stockDetails);
            return this._productionFaultyStockDetailRepository.Save();
        }

        public bool SaveProductionFaultyStockOut(List<ProductionFaultyStockDetailDTO> stockDetailsDTO, long userId, long orgId)
        {
            List<ProductionFaultyStockDetail> stockDetails = new List<ProductionFaultyStockDetail>();
            foreach (var item in stockDetailsDTO)
            {
                ProductionFaultyStockDetail stockDetail = new ProductionFaultyStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    RepairLineId = item.RepairLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = "Faulty Stock In By Repair Section"
                };

                stockDetails.Add(stockDetail);
                var info = _productionFaultyStockInfoBusiness.GetProductionFaultyStockInfoByFloorAndModelAndItem(item.ProductionFloorId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);

                info.StockInQty += item.Quantity;
                info.UpUserId = userId;
                info.UpdateDate = DateTime.Now;
                this._productionFaultyStockInfoRepository.Update(info);
            }

            this._productionFaultyStockDetailRepository.InsertAll(stockDetails);
            return this._productionFaultyStockDetailRepository.Save();
        }

        public bool StockInByRepairSection(long transferId, string status, long orgId, long userId)
        {
            var transferInfo = _repairSectionFaultyItemTransferInfoBusiness.GetRepairSectionFaultyTransferInfoById(transferId,orgId);
            if(transferInfo.StateStatus == RequisitionStatus.Approved)
            {
                transferInfo.StateStatus = status;
                transferInfo.UpdateDate = DateTime.Now;
                transferInfo.UpUserId = userId;
                var transferDetail = _repairSectionFaultyItemTransferDetailBusiness.GetRepairSectionFaultyItemTransferDetailByInfo(transferId, orgId);
                List<ProductionFaultyStockDetailDTO> stockDetailDTOs = new List<ProductionFaultyStockDetailDTO>();
                foreach (var item in transferDetail)
                {
                    ProductionFaultyStockDetailDTO stockDetailDTO = new ProductionFaultyStockDetailDTO
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        RepairLineId = item.RepairLineId,
                        DescriptionId = item.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.FaultyQty,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        ReferenceNumber = transferInfo.TransferCode,
                        StockStatus = StockStatus.StockIn,
                        Remarks ="Faulty Stock In By Repair Line"
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                _repairSectionFaultyItemTransferInfoRepository.Update(transferInfo);

                if (_repairSectionFaultyItemTransferInfoRepository.Save()) {

                    return this.SaveProductionFaultyStockIn(stockDetailDTOs, userId, orgId);
                }
            }
            return false;
        }
    }
}

