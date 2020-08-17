using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBLL.Common;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;

namespace ERPBLL.Inventory.Interface
{
    public class IQCStockDetailBusiness : IIQCStockDetailBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private IQCStockInfoRepository IQCStockInfoRepository;
        private IQCStockDetailRepository IQCStockDetailRepository;
        private readonly IQCItemReqInfoListRepository iQCItemReqInfoListRepository;
        private readonly IIQCStockInfoBusiness _iQCStockInfoBusiness;
        private readonly IIQCItemReqDetailList _iQCItemReqDetailList;
        private readonly IIQCItemReqInfoList _iQCItemReqInfoList;
        private readonly IQCItemReqDetailListBusiness _iQCItemReqDetailListBusiness;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        private readonly IQCItemReqDetailListRepository iQCItemReqDetailListRepository;
        public IQCStockDetailBusiness(IInventoryUnitOfWork inventoryUnitOfWork, IIQCStockInfoBusiness iQCStockInfoBusiness, IIQCItemReqDetailList iQCItemReqDetailList, IIQCItemReqInfoList iQCItemReqInfoList, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, IQCItemReqDetailListBusiness iQCItemReqDetailListBusiness)
        {
            this._inventoryUnitOfWork = inventoryUnitOfWork;
            this._iQCStockInfoBusiness = iQCStockInfoBusiness;
            this._iQCItemReqDetailList = iQCItemReqDetailList;
            this._iQCItemReqInfoList = iQCItemReqInfoList;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            this._iQCItemReqDetailListBusiness = iQCItemReqDetailListBusiness;
            IQCStockInfoRepository = new IQCStockInfoRepository(this._inventoryUnitOfWork);
            IQCStockDetailRepository = new IQCStockDetailRepository(this._inventoryUnitOfWork);
            iQCItemReqInfoListRepository = new IQCItemReqInfoListRepository(this._inventoryUnitOfWork);
            iQCItemReqDetailListRepository = new IQCItemReqDetailListRepository(this._inventoryUnitOfWork);
        }
        public bool SaveIQCStockIn(List<IQCStockDetailDTO> dto, long userId, long orgId)
        {
            List<IQCStockDetail> iQCStockDetails = new List<IQCStockDetail>();
            foreach (var item in dto)
            {
                IQCStockDetail stockDetail = new IQCStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.IQCId = item.IQCId;
                stockDetail.ReferenceNumber = item.ReferenceNumber;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = item.EntryDate;
                stockDetail.StockType = StockType.FreshStock;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.SupplierId = item.SupplierId;

                var iqcStockInfo = _iQCStockInfoBusiness.GetAllIQCStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.DescriptionId == item.DescriptionId).FirstOrDefault();
                if (iqcStockInfo != null)
                {
                    iqcStockInfo.StockInQty += item.Quantity;
                    iqcStockInfo.UpUserId = userId;
                    iqcStockInfo.UpdateDate = DateTime.Now;
                    IQCStockInfoRepository.Update(iqcStockInfo);
                }
                else
                {
                    IQCStockInfo stockInfo = new IQCStockInfo();
                    stockInfo.WarehouseId = item.WarehouseId;
                    stockInfo.DescriptionId = item.DescriptionId;
                    stockInfo.ItemTypeId = item.ItemTypeId;
                    stockInfo.SupplierId = item.SupplierId;
                    stockInfo.ItemId = item.ItemId;
                    stockInfo.UnitId = item.UnitId;
                    stockInfo.StockType = StockType.FreshStock;
                    stockInfo.StockInQty = item.Quantity;
                    stockInfo.StockOutQty = 0;
                    stockInfo.OrganizationId = orgId;
                    stockInfo.EUserId = userId;
                    stockInfo.Remarks = item.Remarks;
                    stockInfo.EntryDate = item.EntryDate;

                    IQCStockInfoRepository.Insert(stockInfo);
                }
                iQCStockDetails.Add(stockDetail);
            }
            IQCStockDetailRepository.InsertAll(iQCStockDetails);
            return IQCStockDetailRepository.Save();
        }

        public bool SaveIQCStockOut(List<IQCStockDetailDTO> iQCStockDetailDTOs, long userId, long orgId)
        {
            List<IQCStockDetail> iQCStockDetails = new List<IQCStockDetail>();
            bool isValidate = true;
            iQCStockDetailDTOs = iQCStockDetailDTOs.Where(s => s.Quantity > 0).ToList();
            var items = iQCStockDetailDTOs.Select(s => s.ItemId).ToList();
            var des = iQCStockDetailDTOs.Select(s => s.DescriptionId).FirstOrDefault();
            var stock = _iQCStockInfoBusiness.GetAllIQCStockInfoByOrgId(orgId).Where(s => items.Contains(s.ItemId.Value) && des == s.DescriptionId).Select(s => s.ItemId).ToList();


            if (items.Count() == stock.Count())
            {
                foreach (var item in iQCStockDetailDTOs)
                {
                    var iqcItemReqInfo = _iQCStockInfoBusiness.GetAllIQCStockInfoByOrgId(orgId).Where(s => s.ItemId == item.ItemId && (s.StockInQty - s.StockOutQty) >= item.Quantity && s.DescriptionId == item.DescriptionId).FirstOrDefault();
                    if (iqcItemReqInfo != null)
                    {
                        iqcItemReqInfo.StockOutQty += item.Quantity;
                        iqcItemReqInfo.UpUserId = userId;
                        iqcItemReqInfo.UpdateDate = DateTime.Now;

                        IQCStockDetail stockDetail = new IQCStockDetail()
                        {
                            WarehouseId = item.WarehouseId,
                            DescriptionId = item.DescriptionId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            Quantity = item.Quantity,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            OrganizationId = orgId,
                            Remarks = item.Remarks,
                            StockStatus = StockStatus.StockOut,
                            ReferenceNumber = item.ReferenceNumber,
                            UnitId = item.UnitId,
                            IQCId = item.IQCId,
                            SupplierId = item.SupplierId
                        };
                        IQCStockInfoRepository.Update(iqcItemReqInfo);
                        iQCStockDetails.Add(stockDetail);
                    }
                    else
                    {
                        isValidate = false;
                    }
                }
            }

            if (isValidate == true)
            {
                IQCStockDetailRepository.InsertAll(iQCStockDetails);
                return IQCStockDetailRepository.Save();
            }
            return false;
        }
        public bool SaveIQCStockInByIQCRequest(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _iQCItemReqDetailList.GetIQCItemReqInfoById(reqId, orgId);
            var reqDetail = _iQCItemReqDetailList.GetIQCReqDetailDetailByInfoId(reqId, orgId).ToList();
            if (reqInfo != null && reqInfo.StateStatus == RequisitionStatus.Approved && reqDetail.Count > 0)
            {
                List<IQCStockDetailDTO> iQCStockDetailDTOs = new List<IQCStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    IQCStockDetailDTO iQCStockDetailDTO = new IQCStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId,
                        Quantity = (int)item.IssueQty,
                        StockType = StockType.FreshStock,
                        StockStatus = StockStatus.StockIn,
                        Remarks = reqInfo.Remarks,   
                        ReferenceNumber = reqInfo.IQCReqCode,
                        DescriptionId = reqInfo.DescriptionId,
                        SupplierId = reqInfo.SupplierId,
                        IQCId = reqInfo.IQCId,
                    };
                    iQCStockDetailDTOs.Add(iQCStockDetailDTO);
                }
                if (SaveIQCStockIn(iQCStockDetailDTOs, userId, orgId) == true)
                {
                    return _iQCItemReqInfoList.SaveIQCReqInfoStatus(reqId, status, orgId, userId);
                }
            }
            return false;
        }

        public bool SaveIQCStockOutByIQCReturn(IQCItemReqInfoListDTO model, long orgId, long userId)
        {
            var reqInfo = _iQCItemReqDetailList.GetIQCItemReqInfoById(model.IQCItemReqInfoId, orgId);
            //var reqDetail = _iQCItemReqDetailList.GetIQCReqDetailDetailByInfoId(model.IQCItemReqInfoId, orgId).ToList();
            if (reqInfo != null && model.IQCItemReqDetails.Count > 0)
            {
                List<IQCStockDetailDTO> iQCStockDetailDTOs = new List<IQCStockDetailDTO>();
                foreach (var item in model.IQCItemReqDetails)
                {
                    IQCStockDetailDTO iQCStockDetailDTO = new IQCStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        UnitId = item.UnitId,
                        Quantity = (int)item.IssueQty,
                        StockType = StockType.FreshStock,
                        StockStatus = StockStatus.StockOut,
                        Remarks = "IQC Return Request has been issued",
                        ReferenceNumber = reqInfo.IQCReqCode,
                        DescriptionId = reqInfo.DescriptionId,
                        SupplierId = reqInfo.SupplierId,
                        IQCId = reqInfo.IQCId,
                    };
                    iQCStockDetailDTOs.Add(iQCStockDetailDTO);

                    // Update IQCItemReqDetails For WellGoods and Faulty
                    var reqDetails = _iQCItemReqDetailListBusiness.GetIQCItemReqDetailById(item.IQCItemReqDetailId, reqInfo.IQCItemReqInfoId, orgId);
                    if (reqDetails != null)
                    {
                        reqDetails.WellGoodsQty = item.WellGoodsQty;
                        reqDetails.FaultyGoodsQty = item.FaultyGoodsQty;
                        reqDetails.UpdateDate = DateTime.Now;
                        reqDetails.UpUserId = userId;
                        iQCItemReqDetailListRepository.Update(reqDetails);
                    }
                }
                if (_iQCItemReqDetailList.SaveIQCItemRequestStatus(model.IQCItemReqInfoId, "Return", orgId, userId) == true)
                {
                    return SaveIQCStockOut(iQCStockDetailDTOs, userId, orgId);
                }
            }
            return false;
        }

        public bool SaveWarehouseStockInByIQCReturnReacive(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _iQCItemReqDetailList.GetIQCItemReqInfoById(reqId, orgId);
            var reqDetail = _iQCItemReqDetailList.GetIQCReqDetailDetailByInfoId(reqId, orgId).ToList();
            if (reqInfo != null && reqDetail.Count > 0)
            {
                List<WarehouseStockDetailDTO> warehouseStocks = new List<WarehouseStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    WarehouseStockDetailDTO warehouse = new WarehouseStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        DescriptionId = reqInfo.DescriptionId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = Convert.ToInt32(item.WellGoodsQty),
                        OrganizationId = orgId,
                        UnitId = item.UnitId,
                        StockStatus = StockStatus.StockIn,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        RefferenceNumber = reqInfo.IQCReqCode,
                        Remarks = "IQC Return has been issued"
                    };
                    warehouseStocks.Add(warehouse);       
                }
                if (_iQCItemReqDetailList.SaveIQCItemRequestStatus(reqId, "Receive-Return", orgId, userId) == true)
                {
                    return _warehouseStockDetailBusiness.SaveWarehouseStockIn(warehouseStocks, userId, orgId);
                }
            }
            return false;
        }
        public IEnumerable<IQCStockDetailDTO> GetAllIQCStockDetailList(string refNum, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string status, string stockStatus, string formDate, string toDate, long orgId)
        {
            IEnumerable<IQCStockDetailDTO> iQCStockInfoDTOs = new List<IQCStockDetailDTO>();
            iQCStockInfoDTOs = this._inventoryUnitOfWork.Db.Database.SqlQuery<IQCStockDetailDTO>(QueryForStockDetailList(refNum, warehouseId,modelId,itemTypeId,itemId,status, stockStatus, formDate,toDate, orgId)).ToList();
            return iQCStockInfoDTOs;
        }

        private string QueryForStockDetailList(string refNum, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string status, string stockStatus, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and sd.OrganizationId = {0}", orgId);
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and sd.ReferenceNumber Like'%{0}%'", refNum);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and des.DescriptionId={0}", modelId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and sd.StockType='{0}'", status);
            }
            if (!string.IsNullOrEmpty(stockStatus) && stockStatus.Trim() != "")
            {
                param += string.Format(@" and sd.StockStatus='{0}'", stockStatus);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select sd.StockDetailId, iq.IQCName, wh.WarehouseName,des.DescriptionName 'ModelName', it.ItemName 'ItemTypeName', i.ItemName, u.UnitSymbol, sd.Quantity, sd.StockType, sd.StockStatus
,sd.EntryDate , ISNULL(sd.ReferenceNumber,'N/A') as 'ReferenceNumber', au.UserName 'EntryUser'
From tblIQCStockDetails sd
Left Join tblWarehouses wh on sd.WarehouseId = wh.Id
Left Join tblDescriptions des on sd.DescriptionId =des.DescriptionId
Left Join tblItemTypes it on sd.ItemTypeId = it.ItemId
Left Join tblItems i on sd.ItemId  = i.ItemId
Left Join tblUnits u on sd.UnitId= u.UnitId      
Left Join tblIQCList iq on sd.IQCId = iq.Id
Left Join [ControlPanel].dbo.tblApplicationUsers au on sd.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
