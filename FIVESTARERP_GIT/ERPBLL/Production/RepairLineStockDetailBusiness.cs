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
    public class RepairLineStockDetailBusiness : IRepairLineStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairLineStockDetailRepository _repairLineStockDetailRepository;
        private readonly RepairLineStockInfoRepository _repairLineStockInfoRepository;
        private readonly IRepairLineStockInfoBusiness _repairLineStockInfoBusiness;
        private readonly IFaultyItemStockDetailBusiness _faultyItemStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;

        public RepairLineStockDetailBusiness(IProductionUnitOfWork productionDb, IRepairLineStockInfoBusiness repairLineStockInfoBusiness, IFaultyItemStockDetailBusiness faultyItemStockDetailBusiness, IItemBusiness itemBusiness)
        {
            this._productionDb = productionDb;
            this._repairLineStockInfoRepository = new RepairLineStockInfoRepository(this._productionDb);
            this._repairLineStockDetailRepository = new RepairLineStockDetailRepository(this._productionDb);
            this._repairLineStockInfoBusiness = repairLineStockInfoBusiness;
            this._faultyItemStockDetailBusiness = faultyItemStockDetailBusiness;
            this._itemBusiness = itemBusiness;
        }
        public IEnumerable<RepairLineStockDetail> GetRepairLineStockDetails(long orgId)
        {
            return _repairLineStockDetailRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public bool SaveRepairLineStockIn(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId)
        {
            List<RepairLineStockDetail> repairLineStockDetails = new List<RepairLineStockDetail>();
            foreach (var item in repairLineStockDetailDTO)
            {
                RepairLineStockDetail stockDetail = new RepairLineStockDetail();
                stockDetail.RepairLineId = item.RepairLineId;
                stockDetail.QCLineId = item.QCLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                // && o.QCLineId ==item.QCLineId // 30-Jun-2020
                var repairStockInfo = _repairLineStockInfoBusiness.GetRepairLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.RepairLineId == item.RepairLineId).FirstOrDefault();
                if (repairStockInfo != null)
                {
                    repairStockInfo.StockInQty += item.Quantity;
                    _repairLineStockInfoRepository.Update(repairStockInfo);
                }
                else
                {
                    RepairLineStockInfo info = new RepairLineStockInfo();
                    info.RepairLineId = item.RepairLineId;
                    info.QCLineId = item.QCLineId;
                    info.ProductionLineId = item.ProductionLineId;
                    info.WarehouseId = item.WarehouseId;
                    info.DescriptionId = item.DescriptionId;

                    info.ItemTypeId = item.ItemTypeId;
                    info.ItemId = item.ItemId;
                    info.UnitId = stockDetail.UnitId;
                    info.StockInQty = item.Quantity;
                    info.StockOutQty = 0;
                    info.OrganizationId = orgId;
                    info.EUserId = userId;
                    info.EntryDate = DateTime.Now;

                    _repairLineStockInfoRepository.Insert(info);
                }
                repairLineStockDetails.Add(stockDetail);
            }
            _repairLineStockDetailRepository.InsertAll(repairLineStockDetails);
            return _repairLineStockDetailRepository.Save();
        }

        public bool SaveRepairLineStockOut(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId, string flag)
        {
            List<RepairLineStockDetail> repairLineStockDetails = new List<RepairLineStockDetail>();
            foreach (var item in repairLineStockDetailDTO)
            {
                RepairLineStockDetail stockDetail = new RepairLineStockDetail();
                stockDetail.RepairLineId = item.RepairLineId;
                stockDetail.QCLineId = item.QCLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockOut;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                //&& o.QCLineId == item.QCLineId // 30-Jun-2020
                var repairStockInfo = _repairLineStockInfoBusiness.GetRepairLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.RepairLineId == item.RepairLineId).FirstOrDefault();
                repairStockInfo.StockOutQty += item.Quantity;
                _repairLineStockInfoRepository.Update(repairStockInfo);
                repairLineStockDetails.Add(stockDetail);
            }
            _repairLineStockDetailRepository.InsertAll(repairLineStockDetails);
            return _repairLineStockDetailRepository.Save();
        }

        public bool StockOutByFaultyItem(List<FaultyItemStockDetailDTO> details, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<RepairLineStockDetailDTO> repairStockDetail = new List<RepairLineStockDetailDTO>();
            string refCode =(details.FirstOrDefault().ReferenceNumber == null || details.FirstOrDefault().ReferenceNumber =="") ? (DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")) : details.FirstOrDefault().ReferenceNumber;
            foreach (var item in details)
            {
                item.ReferenceNumber = refCode;
                RepairLineStockDetailDTO stock = new RepairLineStockDetailDTO
                {
                    ProductionLineId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    RepairLineId = item.RepairLineId,
                    QCLineId = item.QCId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = this._itemBusiness.GetItemById(item.ItemId.Value,orgId).UnitId,
                    Quantity = item.Quantity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    StockStatus = StockStatus.StockOut,
                    RefferenceNumber = refCode,
                    Remarks="Stock Out By Faulty Item"
                };
                item.UnitId = stock.UnitId;
                repairStockDetail.Add(stock);
            }

            if (SaveRepairLineStockOut(repairStockDetail, userId, orgId,string.Empty))
            {
                IsSuccess= _faultyItemStockDetailBusiness.SaveFaultyItemStockIn(details, userId, orgId);
            }
            return IsSuccess;
        }
    }
}
