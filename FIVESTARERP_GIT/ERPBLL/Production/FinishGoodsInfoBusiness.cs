using ERPBLL.Common;
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
    public class FinishGoodsInfoBusiness : IFinishGoodsInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly FinishGoodsInfoRepository _finishGoodsInfoRepository; // repo
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IProductionStockDetailBusiness _productionStockDetailBusiness;
                    
        public FinishGoodsInfoBusiness(IProductionUnitOfWork productionDb, IUnitBusiness unitBusiness, IItemTypeBusiness itemTypeBusiness, IItemBusiness itemBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness, IProductionStockDetailBusiness productionStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._finishGoodsInfoRepository = new FinishGoodsInfoRepository(this._productionDb);
            this._unitBusiness = unitBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._itemBusiness = itemBusiness;
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
        }

        public IEnumerable<FinishGoodsInfo> GetFinishGoodsByOrg(long orgId)
        {
            return _finishGoodsInfoRepository.GetAll(f => f.OrganizationId == orgId).ToList();
        }

        public bool SaveFinishGoods(FinishGoodsInfoDTO infos, List<FinishGoodsRowMaterialDTO> details, long userId, long orgId)
        {
            bool IsSucess = false;
            var itemsInDb = _itemBusiness.GetAllItemByOrgId(orgId).ToList();

            FinishGoodsInfo finishGoodsInfo = new FinishGoodsInfo()
            {
                ProductionLineId = infos.ProductionLineId,
                ItemId = infos.ItemId,
                WarehouseId = infos.WarehouseId,
                DescriptionId = infos.DescriptionId,
                ItemTypeId = _itemBusiness.GetItemOneByOrgId(infos.ItemId,orgId).ItemTypeId,
                UnitId = _itemBusiness.GetItemOneByOrgId(infos.ItemId, orgId).UnitId,
                Quanity = infos.Quanity,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
            };
            List<FinishGoodsRowMaterial> listFinishGoodsRowMaterial = new List<FinishGoodsRowMaterial>();
            List<FinishGoodsStockDetailDTO> listFinishGoodsStockDetailDTO = new List<FinishGoodsStockDetailDTO>();
            List<ProductionStockDetailDTO> listproductionStockDetailDTOs = new List<ProductionStockDetailDTO>();

            FinishGoodsStockDetailDTO finishGoodsStockDetailDTO = new FinishGoodsStockDetailDTO()
            {
                WarehouseId = infos.WarehouseId,
                ItemTypeId = finishGoodsInfo.ItemTypeId,
                ItemId = infos.ItemId,
                LineId = infos.ProductionLineId,
                UnitId = finishGoodsInfo.UnitId,
                DescriptionId = infos.DescriptionId,
                StockStatus = StockStatus.StockIn,
                Quantity = infos.Quanity,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                Remarks = "Stock In By Finish Goods"
            };
            listFinishGoodsStockDetailDTO.Add(finishGoodsStockDetailDTO);

            foreach (var item in details)
            {
                FinishGoodsRowMaterial finishGoodsRowMaterial = new FinishGoodsRowMaterial()
                {
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    WarehouseId = item.WarehouseId,
                    UnitId = _itemBusiness.GetItemOneByOrgId(item.ItemId, orgId).UnitId,
                    OrganizationId = orgId,
                    Quanity = item.Quanity,
                    EUserId = userId,
                    EntryDate = DateTime.Now
                };
                ProductionStockDetailDTO stockDetailDTO = new ProductionStockDetailDTO
                {
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    LineId = infos.ProductionLineId,
                    UnitId = finishGoodsRowMaterial.UnitId,
                    DescriptionId = infos.DescriptionId,
                    StockStatus = StockStatus.StockOut,
                    Quantity = item.Quanity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = "Stock Out For Producing Goods",
                    RefferenceNumber = "Used By Production"
                };

                listFinishGoodsRowMaterial.Add(finishGoodsRowMaterial);
                listproductionStockDetailDTOs.Add(stockDetailDTO);
            }
            // Details //
            finishGoodsInfo.FinishGoodsRowMaterials = listFinishGoodsRowMaterial;
            _finishGoodsInfoRepository.Insert(finishGoodsInfo);

            if (_finishGoodsInfoRepository.Save() == true)
            {
                // Finish Goods Stock-In//
                if (_finishGoodsStockDetailBusiness.SaveFinshGoodsStockIn(listFinishGoodsStockDetailDTO, userId, orgId) == true)
                {
                    // Production Stock-Out // 
                    IsSucess = _productionStockDetailBusiness.SaveProductionStockOut(listproductionStockDetailDTOs, userId, orgId, StockOutReason.StockOutByProductionForProduceGoods);
                }
            }
            return IsSucess;
        }
    }
}
