using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class WarehouseStockDetailBusiness : IWarehouseStockDetailBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for       - Repository
        /// </summary>

        private readonly IInventoryUnitOfWork _inventoryDb; // db
        private readonly IItemBusiness _itemBusiness;
        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        private readonly WarehouseStockDetailRepository warehouseStockDetailRepository; // repo 
        private readonly WarehouseStockInfoRepository warehouseStockInfoRepository; // repo 
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness; // BC
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness; // BC
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness; // BC
        private readonly IItemReturnDetailBusiness _itemReturnDetailBusiness; // BC
        private readonly IRepairStockDetailBusiness _repairStockDetailBusiness; //BC

        public WarehouseStockDetailBusiness(IInventoryUnitOfWork inventoryDb, IItemBusiness itemBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IRepairStockDetailBusiness repairStockDetailBusiness)
        {
            this._inventoryDb = inventoryDb;
            warehouseStockDetailRepository = new WarehouseStockDetailRepository(this._inventoryDb);
            warehouseStockInfoRepository = new WarehouseStockInfoRepository(this._inventoryDb);

            this._warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            this._itemBusiness = itemBusiness;
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._itemReturnDetailBusiness = itemReturnDetailBusiness;
            this._repairStockDetailBusiness = repairStockDetailBusiness;
        }
        public IEnumerable<WarehouseStockDetail> GelAllWarehouseStockDetailByOrgId(long orgId)
        {
            return warehouseStockDetailRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public bool SaveWarehouseStockIn(List<WarehouseStockDetailDTO> warehouseStockDetailDTO, long userId, long orgId)
        {
            List<WarehouseStockDetail> warehouseStockDetails = new List<WarehouseStockDetail>();

            foreach (var item in warehouseStockDetailDTO)
            {
                WarehouseStockDetail stockDetail = new WarehouseStockDetail();
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

                var warehouseInfo = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId).FirstOrDefault();
                if (warehouseInfo != null)
                {
                    warehouseInfo.StockInQty += item.Quantity;
                    warehouseStockInfoRepository.Update(warehouseInfo);
                }
                else
                {
                    WarehouseStockInfo warehouseStockInfo = new WarehouseStockInfo();
                    warehouseStockInfo.WarehouseId = item.WarehouseId;
                    warehouseStockInfo.ItemTypeId = item.ItemTypeId;
                    warehouseStockInfo.ItemId = item.ItemId;
                    warehouseStockInfo.UnitId = stockDetail.UnitId;
                    warehouseStockInfo.StockInQty = item.Quantity;
                    warehouseStockInfo.StockOutQty = 0;
                    warehouseStockInfo.OrganizationId = orgId;
                    warehouseStockInfo.EUserId = userId;
                    warehouseStockInfo.EntryDate = DateTime.Now;
                    warehouseStockInfoRepository.Insert(warehouseStockInfo);
                }
                warehouseStockDetails.Add(stockDetail);
            }
            warehouseStockDetailRepository.InsertAll(warehouseStockDetails);
            return warehouseStockDetailRepository.Save();
        }
        public bool SaveWarehouseStockOut(List<WarehouseStockDetailDTO> warehouseStockDetailDTOs, long userId, long orgId, string flag)
        {
            List<WarehouseStockDetail> warehouseStockDetails = new List<WarehouseStockDetail>();
            bool isValidate = true;
            switch (flag)
            {
                case
                #region Production Requistion
         "Production Requistion":
                    var items = warehouseStockDetailDTOs.Select(s => s.ItemId).ToList();
                    var stock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(s => items.Contains(s.ItemId.Value)).Select(s => s.ItemId).ToList();

                    if (items.Count() == stock.Count())
                    {
                        foreach (var item in warehouseStockDetailDTOs)
                        {
                            var warehouseInfo = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(s => s.ItemId == item.ItemId && (s.StockInQty - s.StockOutQty) >= item.Quantity).FirstOrDefault();
                            if (warehouseInfo != null)
                            {
                                warehouseInfo.StockOutQty += item.Quantity;
                                warehouseInfo.UpUserId = userId;
                                warehouseInfo.UpdateDate = DateTime.Now;

                                WarehouseStockDetail warehouseStockDetail = new WarehouseStockDetail()
                                {
                                    WarehouseId = item.WarehouseId,
                                    ItemTypeId = item.ItemTypeId,
                                    ItemId = item.ItemId,
                                    Quantity = item.Quantity,
                                    EUserId = userId,
                                    EntryDate = DateTime.Now,
                                    OrganizationId = orgId,
                                    Remarks = item.Remarks,
                                    StockStatus = StockStatus.StockOut,
                                    RefferenceNumber = item.RefferenceNumber,
                                    UnitId = item.UnitId
                                };
                                warehouseStockInfoRepository.Update(warehouseInfo);
                                warehouseStockDetails.Add(warehouseStockDetail);
                            }
                            else
                            {
                                isValidate = false;
                            }
                        }
                    }
                    if (isValidate == true)
                    {
                        warehouseStockDetailRepository.InsertAll(warehouseStockDetails);
                        return warehouseStockDetailRepository.Save();
                    }
                    break;
                #endregion
                default:
                    break;
            }
            return false;
        }
        public bool SaveWarehouseStockOutByProductionRequistion(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqId, orgId);
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqId, orgId);
            if (reqInfo != null && reqDetail.Count() > 0)
            {
                List<WarehouseStockDetailDTO> stockDetailDTOs = new List<WarehouseStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    WarehouseStockDetailDTO stockDetailDTO = new WarehouseStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        ItemTypeId = item.ItemTypeId.Value,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId.Value,
                        OrganizationId = item.OrganizationId,
                        Quantity = (int)item.Quantity.Value,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = "Stock Out By Production Requistion " + "(" + reqInfo.ReqInfoCode + ")",
                        RefferenceNumber = reqInfo.ReqInfoCode,
                        StockStatus = StockStatus.StockOut
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (SaveWarehouseStockOut(stockDetailDTOs, userId, orgId, "Production Requistion") == true)
                {
                    return _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, orgId);
                }
            }
            return false;
        }
        public bool SaveWarehouseStockInByProductionItemReturn(long irInfoId, string status, long orgId, long userId)
        {
            bool executionStatus = false;
            if (status == RequisitionStatus.Accepted)
            {
                var irInfo = _itemReturnInfoBusiness.GetItemReturnInfo(orgId, irInfoId);
                var irDetails = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(orgId, irInfoId);
                if (irInfo.StateStatus == RequisitionStatus.Approved)
                {
                    if (irInfo.ReturnType == ReturnType.ProductionGoodsReturn || irInfo.ReturnType == ReturnType.RepairGoodsReturn)
                    {
                        // Warehouse Stock-In
                        List<WarehouseStockDetailDTO> warehouseStockDetailDTOs = new List<WarehouseStockDetailDTO>();
                        foreach (var item in irDetails)
                        {
                            WarehouseStockDetailDTO stockDetailDTO = new WarehouseStockDetailDTO()
                            {
                                WarehouseId = irInfo.WarehouseId,
                                ItemTypeId = item.ItemTypeId,
                                ItemId = item.ItemId,
                                Quantity = item.Quantity,
                                OrganizationId = orgId,
                                EUserId = userId,
                                Remarks = "Stock In By Production Item Return" + "(" + irInfo.IRCode + ")",
                                UnitId = _itemBusiness.GetItemById(item.ItemId, orgId).UnitId,
                                EntryDate = DateTime.Now,
                                StockStatus = StockStatus.StockIn,
                                RefferenceNumber = irInfo.IRCode
                            };
                            warehouseStockDetailDTOs.Add(stockDetailDTO);
                        }
                        if (SaveWarehouseStockIn(warehouseStockDetailDTOs, userId, orgId) == true)
                        {
                            executionStatus = _itemReturnInfoBusiness.SaveItemReturnStatus(irInfoId, status, orgId);
                        }
                    }

                    else if (irInfo.ReturnType == ReturnType.ProductionFaultyReturn || irInfo.ReturnType == ReturnType.RepairFaultyReturn)
                    {
                        List<RepairStockDetailDTO> repairStockDetailDTOs = new List<RepairStockDetailDTO>();
                        foreach (var item in irDetails)
                        {
                            RepairStockDetailDTO stockDetailDTO = new RepairStockDetailDTO()
                            {
                                WarehouseId = irInfo.WarehouseId,
                                ItemTypeId = item.ItemTypeId,
                                ItemId = item.ItemId,
                                Quantity = item.Quantity,
                                OrganizationId = orgId,
                                EUserId = userId,
                                Remarks = "Stock In By Production Faulty Return" + "(" + irInfo.IRCode + ")",
                                UnitId = _itemBusiness.GetItemById(item.ItemId, orgId).UnitId,
                                EntryDate = DateTime.Now,
                                StockStatus = StockStatus.StockIn,
                                RefferenceNumber = irInfo.IRCode,
                                DescriptionId = irInfo.DescriptionId,
                                LineId = irInfo.LineId
                            };
                            repairStockDetailDTOs.Add(stockDetailDTO);
                        }
                        if (_repairStockDetailBusiness.SaveRepairStockIn(repairStockDetailDTOs, userId, orgId) == true)
                        {
                            executionStatus = _itemReturnInfoBusiness.SaveItemReturnStatus(irInfoId, status, orgId);
                        }
                    }
                }
            }
            return executionStatus;
        }

        public IEnumerable<WarehouseStockDetailInfoListDTO> GetWarehouseStockDetailInfoLists(long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            IEnumerable<WarehouseStockDetailInfoListDTO> warehouseStockDetailInfoLists = new List<WarehouseStockDetailInfoListDTO>();
            warehouseStockDetailInfoLists = this._inventoryDb.Db.Database.SqlQuery<WarehouseStockDetailInfoListDTO>(QueryForWarehouseStockDetailInfo(warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum)).ToList();
            return warehouseStockDetailInfoLists;
        }

        private string QueryForWarehouseStockDetailInfo(long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(stockStatus) && stockStatus.Trim() != "")
            {
                param += string.Format(@" and wsd.StockStatus='{0}'", stockStatus);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and wsd.RefferenceNumber Like'%{0}%'", refNum);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(wsd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(wsd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(wsd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select wsd.StockDetailId,wh.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName,u.UnitSymbol 'UnitName',wsd.Quantity,wsd.StockStatus
,Convert(nvarchar(20),wsd.EntryDate,106) 'EntryDate', ISNULL(wsd.RefferenceNumber,'N/A') as 'RefferenceNumber'  From tblWarehouseStockDetails wsd
Left Join tblWarehouses wh on wsd.WarehouseId = wh.Id
Left Join tblItemTypes it on wsd.ItemTypeId = it.ItemId
Left Join tblItems i on wsd.ItemId  = i.ItemId
Left Join tblUnits u on wsd.UnitId= u.UnitId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
