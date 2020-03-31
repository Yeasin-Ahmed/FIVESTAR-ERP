using ERPBLL.Common;
using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class ProductionStockDetailBusiness : IProductionStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database;
        private readonly IInventoryUnitOfWork _inventoryDb; // database

        private readonly ProductionStockDetailRepository _productionStockDetailRepository; //repo
        private readonly ProductionStockInfoRepository _productionStockInfoRepository; // repo

        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness; // BC
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness; // BC
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness; // BC
        private readonly IItemBusiness _itemBusiness;  // BC

        public ProductionStockDetailBusiness(IProductionUnitOfWork productionDb , IInventoryUnitOfWork inventoryDb, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionStockInfoBusiness productionStockInfoBusiness , IItemBusiness itemBusiness)
        {
            this._productionDb = productionDb;
            this._inventoryDb = inventoryDb;
            _productionStockDetailRepository = new ProductionStockDetailRepository(this._productionDb);
            _productionStockInfoRepository =  new ProductionStockInfoRepository(this._productionDb);

            _productionStockInfoBusiness = productionStockInfoBusiness;
            _requsitionInfoBusiness = requsitionInfoBusiness;
            _requsitionDetailBusiness = requsitionDetailBusiness;
            _itemBusiness = itemBusiness;
        }
        public IEnumerable<ProductionStockDetail> GelAllProductionStockDetailByOrgId(long orgId)
        {
            return _productionStockDetailRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public bool SaveProductionStockIn(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId)
        {
            List<ProductionStockDetail> productionStockDetails = new List<ProductionStockDetail>();
            foreach (var item in productionStockDetailDTOs)
            {
                ProductionStockDetail stockDetail = new ProductionStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.LineId = item.LineId;
                stockDetail.DescriptionId = item.DescriptionId;

                var productionInfo = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.LineId == item.LineId && o.DescriptionId == item.DescriptionId).FirstOrDefault();
                if (productionInfo != null)
                {
                    productionInfo.StockInQty += item.Quantity;
                    _productionStockInfoRepository.Update(productionInfo);
                }
                else
                {
                    ProductionStockInfo productionStockInfo = new ProductionStockInfo();
                    productionStockInfo.LineId = item.LineId;
                    productionStockInfo.WarehouseId = item.WarehouseId;
                    productionStockInfo.ItemTypeId = item.ItemTypeId;
                    productionStockInfo.ItemId = item.ItemId;
                    productionStockInfo.UnitId = stockDetail.UnitId;
                    productionStockInfo.StockInQty = item.Quantity;
                    productionStockInfo.StockOutQty = 0;
                    productionStockInfo.OrganizationId = orgId;
                    productionStockInfo.EUserId = userId;
                    productionStockInfo.EntryDate = DateTime.Now;
                    productionStockInfo.DescriptionId = item.DescriptionId;
                    _productionStockInfoRepository.Insert(productionStockInfo);
                }
                productionStockDetails.Add(stockDetail);
            }
            _productionStockDetailRepository.InsertAll(productionStockDetails);
            return _productionStockDetailRepository.Save();
        }

        public bool SaveProductionStockOut(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId, string flag)
        {
            var executionStatus = false;
            List<ProductionStockDetail> productionStockDetails = new List<ProductionStockDetail>();
            if (flag == ReturnType.ProductionGoodsReturn || flag == ReturnType.RepairGoodsReturn || flag == ReturnType.ProductionFaultyReturn || flag == ReturnType.RepairFaultyReturn)
            {
                foreach (var item in productionStockDetailDTOs)
                {
                    ProductionStockDetail stockDetail = new ProductionStockDetail();
                    stockDetail.WarehouseId = item.WarehouseId;
                    stockDetail.ItemTypeId = item.ItemTypeId;
                    stockDetail.ItemId = item.ItemId;
                    stockDetail.Quantity = item.Quantity;
                    stockDetail.OrganizationId = orgId;
                    stockDetail.EUserId = userId;
                    stockDetail.Remarks = item.Remarks;
                    stockDetail.UnitId = item.UnitId;
                    stockDetail.RefferenceNumber = item.RefferenceNumber;
                    stockDetail.LineId = item.LineId;
                    stockDetail.DescriptionId = item.DescriptionId;
                    stockDetail.EntryDate = item.EntryDate;
                    stockDetail.StockStatus = StockStatus.StockReturn;
                    productionStockDetails.Add(stockDetail);
                    var stockInfo = _productionStockInfoBusiness.GetAllProductionStockInfoByLineAndModelId(orgId, item.ItemId.Value, item.LineId.Value,item.DescriptionId.Value);
                    stockInfo.StockOutQty += stockDetail.Quantity;
                    _productionStockInfoRepository.Update(stockInfo);
                }
                _productionStockDetailRepository.InsertAll(productionStockDetails);
                executionStatus = _productionStockDetailRepository.Save();
            }
            return executionStatus;
        }
        public bool SaveProductionStockInByProductionRequistion(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqId, orgId);
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqId, orgId).ToList();
            if(reqInfo!= null && reqInfo.StateStatus == RequisitionStatus.Approved && reqDetail.Count > 0)
            {
                List<ProductionStockDetailDTO> productionStockDetailDTOs = new List<ProductionStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    ProductionStockDetailDTO productionStockDetailDTO = new ProductionStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        UnitId = item.UnitId,
                        StockStatus = StockStatus.StockIn,
                        Remarks = item.Remarks,
                        Quantity = (int)item.Quantity.Value,
                        RefferenceNumber = reqInfo.ReqInfoCode,
                        LineId = reqInfo.LineId,
                        DescriptionId = reqInfo.DescriptionId
                    };
                    productionStockDetailDTOs.Add(productionStockDetailDTO);
                }
                if(SaveProductionStockIn(productionStockDetailDTOs, userId, orgId) == true)
                {
                   return _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, orgId);
                }
            }
            return false;
        }
    }
}
