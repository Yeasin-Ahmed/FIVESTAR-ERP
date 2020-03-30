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
    public class ItemReturnInfoBusiness : IItemReturnInfoBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for       - Repository
        /// </summary>
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly IProductionStockDetailBusiness _productionStockDetailBusiness; // BC
        private readonly IItemBusiness _itemBusiness; // BC
        private readonly IWarehouseBusiness _warehouseBusiness; //BC
        private readonly IProductionLineBusiness _productionLineBusiness; //BC
        private readonly ItemReturnInfoRepository itemReturnInfoRepository; //repo

        public ItemReturnInfoBusiness(IProductionUnitOfWork productionDb, IProductionStockDetailBusiness productionStockDetailBusiness, IItemBusiness itemBusiness, IWarehouseBusiness warehouseBusiness,IProductionLineBusiness productionLineBusiness)
        {
            this._productionDb = productionDb;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._warehouseBusiness = warehouseBusiness;
            this._productionLineBusiness = productionLineBusiness;
            itemReturnInfoRepository = new ItemReturnInfoRepository(this._productionDb);
        }

        public ItemReturnInfo GetItemReturnInfo(long OrgId, long infoId)
        {
            return itemReturnInfoRepository.GetAll(i => i.OrganizationId == OrgId && i.IRInfoId == infoId).FirstOrDefault();
        }

        public IEnumerable<ItemReturnInfo> GetItemReturnInfos(long OrgId)
        {
            return itemReturnInfoRepository.GetAll(i => i.OrganizationId == OrgId).ToList();
        }

        public bool SaveFaultyItemOrGoodsReturn(ItemReturnInfoDTO info, List<ItemReturnDetailDTO> details)
        {
            var executionStatus = false;
            ItemReturnInfo itemReturnInfo = new ItemReturnInfo();
            itemReturnInfo.IRCode = GetReturnCode(info.ReturnType);
            itemReturnInfo.ReturnType = info.ReturnType;
            itemReturnInfo.FaultyCase = info.FaultyCase;
            itemReturnInfo.LineId = info.LineId;
            itemReturnInfo.WarehouseId = info.WarehouseId;
            itemReturnInfo.OrganizationId = info.OrganizationId;
            itemReturnInfo.StateStatus = RequisitionStatus.Approved;
            itemReturnInfo.EUserId = info.EUserId;
            itemReturnInfo.EntryDate = DateTime.Now;
            itemReturnInfo.Remarks = info.Remarks;
            List<ItemReturnDetail> itemReturnDetails = new List<ItemReturnDetail>();
            List<ProductionStockDetailDTO> productionStockDetailDTOs = new List<ProductionStockDetailDTO>();
            foreach (var item in details)
            {
                ItemReturnDetail itemReturnDetail = new ItemReturnDetail();
                itemReturnDetail.IRCode = itemReturnInfo.IRCode;
                itemReturnDetail.ItemTypeId = item.ItemTypeId;
                itemReturnDetail.ItemId = item.ItemId;
                itemReturnDetail.UnitId = _itemBusiness.GetItemOneByOrgId(item.ItemId, info.OrganizationId).UnitId;
                itemReturnDetail.Quantity = item.Quantity;
                itemReturnDetail.OrganizationId = info.OrganizationId;
                itemReturnDetail.EUserId = info.EUserId;
                itemReturnDetail.EntryDate = DateTime.Now;
                itemReturnDetail.Remarks = item.Remarks;

                ProductionStockDetailDTO productionStockDetailDTO = new ProductionStockDetailDTO()
                {
                    WarehouseId = info.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    OrganizationId = info.OrganizationId,
                    LineId = info.LineId,
                    UnitId = itemReturnDetail.UnitId,
                    RefferenceNumber = itemReturnInfo.IRCode,
                    EUserId = itemReturnInfo.EUserId,
                    Remarks = info.ReturnType + (!string.IsNullOrEmpty(info.FaultyCase) ? " (" + info.FaultyCase + ")" : ""),
                    EntryDate = itemReturnDetail.EntryDate
                    
                };
                itemReturnDetails.Add(itemReturnDetail);
                productionStockDetailDTOs.Add(productionStockDetailDTO);
            }
            itemReturnInfo.ItemReturnDetails = itemReturnDetails;
            itemReturnInfoRepository.Insert(itemReturnInfo);
            if (itemReturnInfoRepository.Save() == true)
            {
                executionStatus= _productionStockDetailBusiness.SaveProductionStockOut(productionStockDetailDTOs, itemReturnInfo.EUserId.Value, itemReturnInfo.OrganizationId, itemReturnInfo.ReturnType);
            }
            return executionStatus;
        }

        public bool SaveItemReturnStatus(long irInfoId, string status, long orgId)
        {
            var irInfo = itemReturnInfoRepository.GetOneByOrg(ir => ir.IRInfoId== irInfoId && ir.OrganizationId == orgId);
            if (irInfo != null)
            {
                irInfo.StateStatus = status;
                itemReturnInfoRepository.Update(irInfo);
            }
            return itemReturnInfoRepository.Save();
        }

        private string GetReturnCode(string returnType)
        {
            string code = string.Empty;
            string val = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
            if (returnType == ReturnType.ProductionGoodsReturn)
            {
                code = "PGR-" + val;
            }
            else if (returnType == ReturnType.ProductionFaultyReturn)
            {
                code = "PFR-" + val;
            }
            if (returnType == ReturnType.RepairGoodsReturn)
            {
                code = "RGR-" + val;
            }
            else if (returnType == ReturnType.RepairFaultyReturn)
            {
                code = "RFR-" + val;
            }
            return code;
        }
    }
}
