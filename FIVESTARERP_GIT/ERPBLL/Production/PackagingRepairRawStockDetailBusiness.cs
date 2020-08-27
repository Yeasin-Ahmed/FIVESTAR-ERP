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
    public class PackagingRepairRawStockDetailBusiness : IPackagingRepairRawStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingRepairRawStockDetailRepository _packagingRepairRawStockDetailRepository;
        private readonly PackagingRepairRawStockInfoRepository _packagingRepairRawStockInfoRepository;
        private readonly IPackagingRepairRawStockInfoBusiness _packagingRepairRawStockInfoBusiness;
        private readonly RepairSectionRequisitionInfoBusiness _repairSectionRequisitionInfoBusiness;
        private readonly RepairSectionRequisitionDetailBusiness _repairSectionRequisitionDetailBusiness;
        public PackagingRepairRawStockDetailBusiness(IProductionUnitOfWork productionDb, IPackagingRepairRawStockInfoBusiness packagingRepairRawStockInfoBusiness, RepairSectionRequisitionInfoBusiness repairSectionRequisitionInfoBusinesss, RepairSectionRequisitionDetailBusiness repairSectionRequisitionDetailBusiness)
        {
            this._productionDb = productionDb;
            this._packagingRepairRawStockInfoBusiness = packagingRepairRawStockInfoBusiness;
            this._repairSectionRequisitionInfoBusiness = repairSectionRequisitionInfoBusinesss;
            this._repairSectionRequisitionDetailBusiness = repairSectionRequisitionDetailBusiness;
            // Repository
            this._packagingRepairRawStockDetailRepository = new PackagingRepairRawStockDetailRepository(this._productionDb);
            this._packagingRepairRawStockInfoRepository = new PackagingRepairRawStockInfoRepository(this._productionDb);
        }
        public IEnumerable<PackagingRepairRawStockDetailDTO> GetPackagingRepairRawStockDetailByQuery(long floorId, long packagingLine, long modelId, long warehouseId, long itemTypeId, long itemId, string refNum, string fromDate, string toDate, string status)
        {
            throw new NotImplementedException();
        }

        public bool SavePackagingRepairRawStockIn(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<PackagingRepairRawStockDetail> packagingRepairRawStockDetails = new List<PackagingRepairRawStockDetail>();
            foreach (var item in stockDetailDTOs)
            {
                PackagingRepairRawStockDetail stockDetail = new PackagingRepairRawStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.FloorId = item.FloorId;
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

                var packagingStockInfo = _packagingRepairRawStockInfoBusiness.GetPackagingRepairRawStockInfoByPackagingLineAndModelAndItem(item.FloorId.Value, item.PackagingLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);
                if (packagingStockInfo != null)
                {
                    packagingStockInfo.StockInQty += item.Quantity;
                    _packagingRepairRawStockInfoRepository.Update(packagingStockInfo);
                }
                else
                {
                    PackagingRepairRawStockInfo info = new PackagingRepairRawStockInfo();
                    info.PackagingLineId = item.PackagingLineId;
                    info.FloorId = item.FloorId;
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
                    _packagingRepairRawStockInfoRepository.Insert(info);
                }
                packagingRepairRawStockDetails.Add(stockDetail);
            }
            _packagingRepairRawStockDetailRepository.InsertAll(packagingRepairRawStockDetails);
            return _packagingRepairRawStockDetailRepository.Save();
        }

        public async Task<bool> SavePackagingRepairRawStockInAsync(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<PackagingRepairRawStockDetail> packagingRepairRawStockDetails = new List<PackagingRepairRawStockDetail>();
            foreach (var item in stockDetailDTOs)
            {
                PackagingRepairRawStockDetail stockDetail = new PackagingRepairRawStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.FloorId = item.FloorId;
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

                var packagingStockInfo = await _packagingRepairRawStockInfoBusiness.GetPackagingRepairRawStockInfoByPackagingLineAndModelAndItemAsync(item.FloorId.Value, item.PackagingLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);
                if (packagingStockInfo != null)
                {
                    packagingStockInfo.StockInQty += item.Quantity;
                    _packagingRepairRawStockInfoRepository.Update(packagingStockInfo);
                }
                else
                {
                    PackagingRepairRawStockInfo info = new PackagingRepairRawStockInfo();
                    info.PackagingLineId = item.PackagingLineId;
                    info.FloorId = item.FloorId;
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
                    _packagingRepairRawStockInfoRepository.Insert(info);
                }
                packagingRepairRawStockDetails.Add(stockDetail);
            }
            _packagingRepairRawStockDetailRepository.InsertAll(packagingRepairRawStockDetails);
            return await _packagingRepairRawStockDetailRepository.SaveAsync();
        }

        public bool SavePackagingRepairRawStockOut(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<PackagingRepairRawStockDetail> packagingRepairRawStockDetails = new List<PackagingRepairRawStockDetail>();
            foreach (var item in stockDetailDTOs)
            {
                PackagingRepairRawStockDetail stockDetail = new PackagingRepairRawStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.FloorId = item.FloorId;
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

                var packagingStockInfo = _packagingRepairRawStockInfoBusiness.GetPackagingRepairRawStockInfoByPackagingLineAndModelAndItem(item.FloorId.Value, item.PackagingLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                packagingStockInfo.StockOutQty += item.Quantity;
                _packagingRepairRawStockInfoRepository.Update(packagingStockInfo);
                packagingRepairRawStockDetails.Add(stockDetail);
            }
            _packagingRepairRawStockDetailRepository.InsertAll(packagingRepairRawStockDetails);
            return _packagingRepairRawStockDetailRepository.Save();
        }

        public bool SavePackagingRepairRawStockReturn(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<PackagingRepairRawStockDetail> packagingRepairRawStockDetails = new List<PackagingRepairRawStockDetail>();
            foreach (var item in stockDetailDTOs)
            {
                PackagingRepairRawStockDetail stockDetail = new PackagingRepairRawStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.FloorId = item.FloorId;
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
                stockDetail.StockStatus = StockStatus.StockReturn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var packagingStockInfo = _packagingRepairRawStockInfoBusiness.GetPackagingRepairRawStockInfoByPackagingLineAndModelAndItem(item.FloorId.Value, item.PackagingLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                packagingStockInfo.StockOutQty += item.Quantity;
                _packagingRepairRawStockInfoRepository.Update(packagingStockInfo);
                packagingRepairRawStockDetails.Add(stockDetail);
            }
            _packagingRepairRawStockDetailRepository.InsertAll(packagingRepairRawStockDetails);
            return _packagingRepairRawStockDetailRepository.Save();
        }

        public async Task<bool> SavePackagingRepairRawStockOutAsync(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<PackagingRepairRawStockDetail> packagingRepairRawStockDetails = new List<PackagingRepairRawStockDetail>();
            foreach (var item in stockDetailDTOs)
            {
                PackagingRepairRawStockDetail stockDetail = new PackagingRepairRawStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.FloorId = item.FloorId;
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

                var packagingStockInfo = await _packagingRepairRawStockInfoBusiness.GetPackagingRepairRawStockInfoByPackagingLineAndModelAndItemAsync(item.FloorId.Value, item.PackagingLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                packagingStockInfo.StockOutQty += item.Quantity;
                _packagingRepairRawStockInfoRepository.Update(packagingStockInfo);
                packagingRepairRawStockDetails.Add(stockDetail);
            }
            _packagingRepairRawStockDetailRepository.InsertAll(packagingRepairRawStockDetails);
            return await _packagingRepairRawStockDetailRepository.SaveAsync();
        }

        public bool StockInByPackagingSectionRequisition(long reqId, string status, long userId, long orgId)
        {
            var reqInfo = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(reqId, orgId);
            if (reqInfo != null && reqInfo.StateStatus == RequisitionStatus.HandOver)
            {
                if (_repairSectionRequisitionInfoBusiness.SaveRepairSectionRequisitionStatus(reqId, RequisitionStatus.Accepted, orgId, userId))
                {
                    var reqDetail = _repairSectionRequisitionDetailBusiness.GetRepairSectionRequisitionDetailByInfoId(reqId, orgId);
                    List<PackagingRepairRawStockDetailDTO> repairStocks = new List<PackagingRepairRawStockDetailDTO>();
                    foreach (var item in reqDetail)
                    {
                        PackagingRepairRawStockDetailDTO repairStock = new PackagingRepairRawStockDetailDTO()
                        {
                            PackagingLineId = reqInfo.PackagingLineId,
                            FloorId = reqInfo.ProductionFloorId,
                            DescriptionId = reqInfo.DescriptionId,
                            WarehouseId = reqInfo.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            OrganizationId = orgId,
                            UnitId = item.UnitId,
                            Quantity = item.IssueQty,
                            RefferenceNumber = reqInfo.RequisitionCode,
                            EUserId = userId,
                            StockStatus = StockStatus.StockIn,
                            EntryDate = DateTime.Now,
                            Remarks = "Stock In By Repair Section Requisition"
                        };
                        repairStocks.Add(repairStock);
                    }
                    return SavePackagingRepairRawStockIn(repairStocks, userId, orgId);
                }
            }
            return false;
        }
    }
}
