using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
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
    public class FinishGoodsSendToWarehouseInfoBusiness : IFinishGoodsSendToWarehouseInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        public readonly FinishGoodsSendToWarehouseInfoRepository _finishGoodsSendToWarehouseInfoRepository;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IFinishGoodsSendToWarehouseDetailBusiness _finishGoodsSendToWarehouseDetailBusiness;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        public FinishGoodsSendToWarehouseInfoBusiness(IProductionUnitOfWork productionDb, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness, IItemBusiness itemBusiness, IFinishGoodsSendToWarehouseDetailBusiness finishGoodsSendToWarehouseDetailBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._finishGoodsSendToWarehouseInfoRepository = new FinishGoodsSendToWarehouseInfoRepository(this._productionDb);
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._finishGoodsSendToWarehouseDetailBusiness = finishGoodsSendToWarehouseDetailBusiness;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
        }

        public IEnumerable<FinishGoodsSendToWarehouseInfo> GetFinishGoodsSendToWarehouseList(long orgId)
        {
            return _finishGoodsSendToWarehouseInfoRepository.GetAll(f => f.OrganizationId == orgId).ToList();
        }

        public bool SaveFinishGoodsSendToWarehouse(FinishGoodsSendToWarehouseInfoDTO info, List<FinishGoodsSendToWarehouseDetailDTO> detail, long userId, long orgId)
        {
            bool IsSuccess = false;
            FinishGoodsSendToWarehouseInfo domainInfo = new FinishGoodsSendToWarehouseInfo
            {
                LineId = info.LineId,
                WarehouseId = info.WarehouseId,
                DescriptionId = info.DescriptionId,
                PackagingLineId = info.PackagingLineId,
                Flag = ProductionTypes.SKD,
                Remarks = "",
                EUserId = userId,
                OrganizationId = orgId,
                EntryDate = DateTime.Now,
                StateStatus = FinishGoodsSendStatus.Send,
                RefferenceNumber = ("STW-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
            List<FinishGoodsSendToWarehouseDetail> domainDetails = new List<FinishGoodsSendToWarehouseDetail>();
            List<FinishGoodsStockDetailDTO> stockDetails = new List<FinishGoodsStockDetailDTO>();
            foreach (var item in detail)
            {
                FinishGoodsSendToWarehouseDetail dtl = new FinishGoodsSendToWarehouseDetail
                {
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitId = _itemBusiness.GetItemOneByOrgId(item.ItemId, orgId).UnitId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    Remarks = domainInfo.Flag,
                    Flag = domainInfo.Flag,
                    RefferenceNumber = domainInfo.RefferenceNumber
                };
                FinishGoodsStockDetailDTO stock = new FinishGoodsStockDetailDTO()
                {
                    LineId = info.LineId,
                    PackagingLineId = info.PackagingLineId,
                    WarehouseId = info.WarehouseId,
                    DescriptionId = info.DescriptionId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitId = dtl.UnitId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    Remarks = domainInfo.Flag,
                    RefferenceNumber = domainInfo.RefferenceNumber
                };
                domainDetails.Add(dtl);
                stockDetails.Add(stock);
            }
            domainInfo.FinishGoodsSendToWarehouseDetails = domainDetails;
            _finishGoodsSendToWarehouseInfoRepository.Insert(domainInfo);
            if (_finishGoodsSendToWarehouseInfoRepository.Save() == true)
            {
                IsSuccess = this._finishGoodsStockDetailBusiness.SaveFinishGoodsStockOut(stockDetails, userId, orgId);
            }
            return IsSuccess;
        }

        public FinishGoodsSendToWarehouseInfo GetFinishGoodsSendToWarehouseById(long id, long orgId)
        {
            return _finishGoodsSendToWarehouseInfoRepository.GetOneByOrg(f => f.SendId == id && f.OrganizationId == orgId);
        }

        public bool SaveFinishGoodsStatus(long sendId, long userId, long orgId)
        {
            bool IsSuccess = false;
            var info = GetFinishGoodsSendToWarehouseById(sendId, orgId);
            List<WarehouseStockDetailDTO> detailDTOs = new List<WarehouseStockDetailDTO>();
            if (info != null)
            {
                info.StateStatus = FinishGoodsSendStatus.Received;
                info.UpdateDate = DateTime.Now;
                info.UpUserId = userId;
                _finishGoodsSendToWarehouseInfoRepository.Update(info);
                var details = this._finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(info.SendId, orgId);
                foreach (var item in details)
                {
                    WarehouseStockDetailDTO warehouse = new WarehouseStockDetailDTO()
                    {
                        WarehouseId =info.WarehouseId,
                        DescriptionId = info.DescriptionId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        Quantity = item.Quantity,
                        EUserId = userId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        RefferenceNumber = info.RefferenceNumber,
                        Remarks = info.Remarks,
                        StockStatus = StockStatus.StockIn
                    };
                    detailDTOs.Add(warehouse);
                }
            }
            if (_finishGoodsSendToWarehouseInfoRepository.Save() == true)
            {
                IsSuccess=_warehouseStockDetailBusiness.SaveWarehouseStockIn(detailDTOs, userId, orgId);
            }
            return IsSuccess;
        }
    }
}
