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
        // Business Class
        private readonly IRepairLineStockInfoBusiness _repairLineStockInfoBusiness;
        private readonly IFaultyItemStockDetailBusiness _faultyItemStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IRepairSectionRequisitionInfoBusiness _repairSectionRequisitionInfoBusiness;
        private readonly IRepairSectionRequisitionDetailBusiness _repairSectionRequisitionDetailBusiness;
        //private readonly IQRCodeTransferToRepairInfoBusiness _qRCodeTransferToRepairInfoBusiness;

        // Repository
        private readonly RepairLineStockDetailRepository _repairLineStockDetailRepository;
        private readonly RepairLineStockInfoRepository _repairLineStockInfoRepository;
        private readonly RepairSectionRequisitionInfoRepository _repairSectionRequisitionInfoRepository;


        public RepairLineStockDetailBusiness(IProductionUnitOfWork productionDb, IRepairLineStockInfoBusiness repairLineStockInfoBusiness, IFaultyItemStockDetailBusiness faultyItemStockDetailBusiness, IItemBusiness itemBusiness, IRepairSectionRequisitionInfoBusiness repairSectionRequisitionInfoBusiness, IRepairSectionRequisitionDetailBusiness repairSectionRequisitionDetailBusiness)
        {
            this._productionDb = productionDb;
            // Repository
            this._repairLineStockInfoRepository = new RepairLineStockInfoRepository(this._productionDb);
            this._repairLineStockDetailRepository = new RepairLineStockDetailRepository(this._productionDb);
            this._repairSectionRequisitionInfoRepository = new RepairSectionRequisitionInfoRepository(this._productionDb);

            // Business Class
            this._repairLineStockInfoBusiness = repairLineStockInfoBusiness;
            this._faultyItemStockDetailBusiness = faultyItemStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._repairSectionRequisitionInfoBusiness = repairSectionRequisitionInfoBusiness;
            this._repairSectionRequisitionDetailBusiness = repairSectionRequisitionDetailBusiness;
            //, IQRCodeTransferToRepairInfoBusiness qRCodeTransferToRepairInfoBusiness
            //this._qRCodeTransferToRepairInfoBusiness = qRCodeTransferToRepairInfoBusiness;
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

        public bool StockInByRepairSectionRequisition(long reqId, string status, long userId, long orgId)
        {
            var reqInfo = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(reqId, orgId);
            if (reqInfo != null && reqInfo.StateStatus == RequisitionStatus.HandOver)
            {
                if (_repairSectionRequisitionInfoBusiness.SaveRepairSectionRequisitionStatus(reqId, RequisitionStatus.Accepted, orgId, userId))
                {
                    var reqDetail = _repairSectionRequisitionDetailBusiness.GetRepairSectionRequisitionDetailByInfoId(reqId, orgId);
                    List<RepairLineStockDetailDTO> repairStocks = new List<RepairLineStockDetailDTO>();
                    foreach (var item in reqDetail)
                    {
                        RepairLineStockDetailDTO repairStock = new RepairLineStockDetailDTO()
                        {
                            RepairLineId = reqInfo.RepairLineId,
                            ProductionLineId = reqInfo.ProductionFloorId,
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
                    return SaveRepairLineStockIn(repairStocks, userId, orgId);
                }
            }
            return false;
        }

        public bool StockOutByFaultyItem(List<FaultyItemStockDetailDTO> details, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<RepairLineStockDetailDTO> repairStockDetail = new List<RepairLineStockDetailDTO>();
            string refCode = (details.FirstOrDefault().ReferenceNumber == null || details.FirstOrDefault().ReferenceNumber == "") ? (DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")) : details.FirstOrDefault().ReferenceNumber;
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
                    UnitId = this._itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId,
                    Quantity = item.Quantity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    StockStatus = StockStatus.StockOut,
                    RefferenceNumber = refCode,
                    Remarks = "Stock Out By Faulty Item"
                };
                item.UnitId = stock.UnitId;
                repairStockDetail.Add(stock);
            }

            if (SaveRepairLineStockOut(repairStockDetail, userId, orgId, string.Empty))
            {
                IsSuccess = _faultyItemStockDetailBusiness.SaveFaultyItemStockIn(details, userId, orgId);
            }
            return IsSuccess;
        }

        public async Task<bool> SaveRepairLineStockOutAsync(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId, string flag)
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
                var repairStockInfo = await _repairLineStockInfoBusiness.GetRepairLineStockInfoByRepairAndItemAndModelIdAsync(item.RepairLineId.Value,item.ItemId.Value, item.DescriptionId.Value, orgId);
                repairStockInfo.StockOutQty += item.Quantity;
                _repairLineStockInfoRepository.Update(repairStockInfo);
                repairLineStockDetails.Add(stockDetail);
            }
            _repairLineStockDetailRepository.InsertAll(repairLineStockDetails);
            return  await _repairLineStockDetailRepository.SaveAsync();
        }

        // Adding Faulty Item By QRCode Scanning //
        //public bool StockOutByAddingFaultyWithQRCode(FaultyInfoByQRCodeDTO model, long userId, long orgId)
        //{
        //    // Check if the QRCode is exist with the status Received
        //    //var qrCodeInfo = _qRCodeTransferToRepairInfoBusiness.GetQRCodeWiseItemInfo(model.QRCode, FinishGoodsSendStatus.Received, orgId);
        //    //if (qrCodeInfo != null && qrCodeInfo.TransferId == model.TransferId && qrCodeInfo.DescriptionId == model.ModelId)
        //    //{
        //    //    var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId).ToList();
        //    //    List<RepairLineStockDetailDTO> repairLineStocks = new List<RepairLineStockDetailDTO>();
        //    //    List<FaultyItemStockDetailDTO> faultyItemStocks = new List<FaultyItemStockDetailDTO>();
        //    //    foreach (var item in model.FaultyItems)
        //    //    {
        //    //        var itemInfo = allItemsInDb.FirstOrDefault(i => i.ItemId == item.ItemId);
        //    //        RepairLineStockDetailDTO repairLineStock = new RepairLineStockDetailDTO()
        //    //        {
        //    //            ProductionLineId = qrCodeInfo.FloorId,
        //    //            QCLineId = qrCodeInfo.QCLineId,
        //    //            RepairLineId = qrCodeInfo.RepairLineId,
        //    //            DescriptionId = qrCodeInfo.DescriptionId,
        //    //            WarehouseId = item.WarehouseId,
        //    //            ItemTypeId = item.ItemTypeId,
        //    //            ItemId = item.ItemId,
        //    //            Quantity = item.Quantity,
        //    //            StockStatus = StockStatus.StockOut,
        //    //            OrganizationId = orgId,
        //    //            EUserId = userId,
        //    //            Remarks = "Stock Out By QRCode Scanning",
        //    //            EntryDate = DateTime.Now,
        //    //            RefferenceNumber = qrCodeInfo.QRCode + "#" + qrCodeInfo.TransferCode + "#" + qrCodeInfo.TransferId.ToString(),
        //    //            UnitId = itemInfo.UnitId
        //    //        };
        //    //        repairLineStocks.Add(repairLineStock);

        //    //        FaultyItemStockDetailDTO faultyItemStock = new FaultyItemStockDetailDTO()
        //    //        {
        //    //            ProductionFloorId = qrCodeInfo.FloorId,
        //    //            QCId = qrCodeInfo.QCLineId,
        //    //            RepairLineId = qrCodeInfo.RepairLineId,
        //    //            DescriptionId = qrCodeInfo.DescriptionId,
        //    //            WarehouseId = item.WarehouseId,
        //    //            ItemTypeId = item.ItemTypeId,
        //    //            ItemId = item.ItemId,
        //    //            Quantity = item.Quantity,
        //    //            StockStatus = StockStatus.StockIn,
        //    //            OrganizationId = orgId,
        //    //            EUserId = userId,
        //    //            Remarks = "Stock In By QRCode Scanning",
        //    //            EntryDate = DateTime.Now,
        //    //            ReferenceNumber = qrCodeInfo.QRCode,
        //    //            TransferCode = qrCodeInfo.TransferCode,
        //    //            TransferId = qrCodeInfo.TransferId
        //    //        };

        //    //        faultyItemStocks.Add(faultyItemStock);
        //    //    }

        //    //    if (repairLineStocks.Count > 0 && SaveRepairLineStockOut(repairLineStocks, userId, orgId,string.Empty))
        //    //    {
        //    //        return _faultyItemStockDetailBusiness.SaveFaultyItemStockIn(faultyItemStocks, userId, orgId);
        //    //    }
        //    //}
        //    return false;
        //}
    }
}
