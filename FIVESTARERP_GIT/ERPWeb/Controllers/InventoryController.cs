using ERPBLL.Inventory.Interface;
using ERPBO.ControlPanel.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DTOModels;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPBO.Production.DTOModel;
using ERPBLL.Production.Interface;
using ERPBO.Production.ViewModels;
using ERPBLL.Common;
using System.Data.Entity;
using ERPBO.Production.DomainModels;
using ERPBO.Common;
using PagedList;

namespace ERPWeb.Controllers
{
    public class InventoryController : BaseController
    {
        // GET: Inventory
        private readonly IWarehouseBusiness _warehouseBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness;
        private readonly IItemReturnDetailBusiness _itemReturnDetailBusiness;
        private readonly IRepairStockInfoBusiness _repairStockInfoBusiness;
        private readonly IRepairStockDetailBusiness _repairStockDetailBusiness;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly IFinishGoodsSendToWarehouseInfoBusiness _finishGoodsSendToWarehouseInfoBusiness;
        private readonly IFinishGoodsSendToWarehouseDetailBusiness _finishGoodsSendToWarehouseDetailBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;

        public InventoryController(IWarehouseBusiness warehouseBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, IProductionLineBusiness productionLineBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IRepairStockInfoBusiness repairStockInfoBusiness, IRepairStockDetailBusiness repairStockDetailBusiness, IDescriptionBusiness descriptionBusiness, IFinishGoodsSendToWarehouseInfoBusiness finishGoodsSendToWarehouseInfoBusiness, IFinishGoodsSendToWarehouseDetailBusiness finishGoodsSendToWarehouseDetailBusiness)
        {
            this._warehouseBusiness = warehouseBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._itemBusiness = itemBusiness;
            this._warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._itemReturnDetailBusiness = itemReturnDetailBusiness;
            this._repairStockInfoBusiness = repairStockInfoBusiness;
            this._repairStockDetailBusiness = repairStockDetailBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._finishGoodsSendToWarehouseInfoBusiness = finishGoodsSendToWarehouseInfoBusiness;
            this._finishGoodsSendToWarehouseDetailBusiness = finishGoodsSendToWarehouseDetailBusiness;
        }
        // GET: Account
        #region Description
        public ActionResult GetDescriptionList(int? page)
        {
            IPagedList<DescriptionViewModel> descriptionViewModels = _descriptionBusiness.GetDescriptionByOrgId(OrgId).Select(des => new DescriptionViewModel
            {
                DescriptionId = des.DescriptionId,
                DescriptionName = des.DescriptionName,
                SubCategoryId = des.SubCategoryId,
                StateStatus = (des.IsActive == true ? "Active" : "Inactive"),
                Remarks = des.Remarks,
                OrganizationId = des.OrganizationId,
                EUserId = des.EUserId,
                EntryDate = des.EntryDate,
                UpUserId = des.UpUserId,
                UpdateDate = des.UpdateDate

            }).OrderBy(des => des.DescriptionId).ToPagedList(page ?? 1, 15);
            IEnumerable<DescriptionViewModel> descriptionViewModelForPage = new List<DescriptionViewModel>();
            return View(descriptionViewModels);
        }
        #endregion

        #region Warehouse - Table
        [HttpGet]
        public ActionResult GetWarehouseList()
        {
            IEnumerable<WarehouseDTO> warehousesDomains = _warehouseBusiness.GetAllWarehouseByOrgId(1).Select(ware => new WarehouseDTO
            {
                Id = ware.Id,
                WarehouseName = ware.WarehouseName,
                Remarks = ware.Remarks,
                StateStatus = (ware.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = ware.OrganizationId
            }).ToList();
            List<WarehouseViewModel> warehouseViewModels = new List<WarehouseViewModel>();
            AutoMapper.Mapper.Map(warehousesDomains, warehouseViewModels);
            return View(warehouseViewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveWarehouse(WarehouseViewModel viewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    WarehouseDTO dto = new WarehouseDTO();
                    AutoMapper.Mapper.Map(viewModel, dto);
                    isSuccess = _warehouseBusiness.SaveWarehouse(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        #endregion

        #region ItemType - Table
        public ActionResult GetItemTypeList(int? page)
        {
            ViewBag.pageNum = page.ToString();
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            IPagedList<ItemTypeViewModel> itemTypesDomains = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId).Select(item => new ItemTypeViewModel
            {
                ItemId = item.ItemId,
                WarehouseId = item.WarehouseId,
                ItemName = item.ItemName,
                Remarks = item.Remarks,
                StateStatus = (item.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = item.OrganizationId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(item.WarehouseId, OrgId).WarehouseName)
            }).OrderBy(item => item.ItemId).ToPagedList(page?? 1, 3);
            IEnumerable<ItemTypeViewModel> itemTypeViewModelsForPage =new List<ItemTypeViewModel>();
            //List<ItemTypeViewModel> itemTypeViewModels = new List<ItemTypeViewModel>();
            //AutoMapper.Mapper.Map(itemTypesDomains, itemTypeViewModels);
            
            return View(itemTypesDomains);
        }

        public ActionResult SaveItemType(ItemTypeViewModel itemTypeViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ItemTypeDTO dto = new ItemTypeDTO();
                    AutoMapper.Mapper.Map(itemTypeViewModel, dto);
                    isSuccess = _itemTypeBusiness.SaveItemType(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Unit - Table
        public ActionResult GetAllUnitList()
        {
            IEnumerable<UnitDomainDTO> unitDomains = _unitBusiness.GetAllUnitByOrgId(1).Select(unit => new UnitDomainDTO
            {
                UnitId = unit.UnitId,
                UnitName = unit.UnitName,
                UnitSymbol = unit.UnitSymbol,
                Remarks = unit.Remarks,
                OrganizationId = unit.OrganizationId
            }).ToList();
            List<UnitViewModel> unitViewModels = new List<UnitViewModel>();
            AutoMapper.Mapper.Map(unitDomains, unitViewModels);
            return View(unitViewModels);
        }

        public ActionResult SaveUnit(UnitViewModel unitViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    UnitDomainDTO dto = new UnitDomainDTO();
                    AutoMapper.Mapper.Map(unitViewModel, dto);
                    isSuccess = _unitBusiness.SaveUnit(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Item - Table
        public ActionResult GetItemList(int? page)
        {
            ViewBag.pageNum = page.ToString();
            ViewBag.ddlItemTypeName = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId).Select(itemtype => new SelectListItem { Text = itemtype.ItemName, Value = itemtype.ItemId.ToString() }).ToList();

            ViewBag.ddlUnitName = _unitBusiness.GetAllUnitByOrgId(OrgId).Select(unit => new SelectListItem { Text = unit.UnitName, Value = unit.UnitId.ToString() }).ToList();

            IPagedList<ItemViewModel> itemViewModels = _itemBusiness.GetAllItemByOrgId(1).Select(item => new ItemViewModel
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Remarks = item.Remarks,
                StateStatus = (item.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = item.OrganizationId,
                ItemTypeId = item.ItemTypeId,
                ItemTypeName = _itemTypeBusiness.GetItemType(item.ItemTypeId, OrgId).ItemName,
                UnitId = item.UnitId,
                UnitName = _unitBusiness.GetUnitOneByOrgId(item.UnitId, OrgId).UnitName,
                ItemCode = item.ItemCode
            }).OrderBy(item => item.ItemId).ToPagedList(page ?? 1, 3);
            //IEnumerable<ItemViewModel> itemViewModelsForPage = new List<ItemViewModel>();
            //AutoMapper.Mapper.Map(itemDomains, itemViewModels);
            return View(itemViewModels);
        }
        public ActionResult SaveItem(ItemViewModel itemViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ItemDomainDTO dto = new ItemDomainDTO();
                    AutoMapper.Mapper.Map(itemViewModel, dto);
                    isSuccess = _itemBusiness.SaveItem(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemById(long id)
        {
            ItemDomainDTO itemDTO = _itemBusiness.GetItemById(id, OrgId);
            itemDTO.UnitName = _unitBusiness.GetUnitOneByOrgId(itemDTO.UnitId, OrgId).UnitName;
            itemDTO.ItemTypeName = _itemTypeBusiness.GetItemType(itemDTO.ItemTypeId, OrgId).ItemName;
            ItemViewModel itemViewModel = new ItemViewModel();
            AutoMapper.Mapper.Map(itemDTO, itemViewModel);
            return Json(itemViewModel);
        }
        #endregion

        #region Warehouse Stock Info -Table

        [HttpGet]
        public ActionResult GetWarehouseStockInfoList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetWarehouseStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId, string lessOrEq)
        {
            IEnumerable<WarehouseStockInfoDTO> warehouseStockInfoDTO = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(OrgId).Select(info => new WarehouseStockInfoDTO
            {
                StockInfoId = info.StockInfoId,
                WarehouseId = info.WarehouseId,
                Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, OrgId).WarehouseName),
                ItemTypeId = info.ItemTypeId,
                ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, OrgId).ItemName),
                ItemId = info.ItemId,
                Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, OrgId).ItemName),
                UnitId = info.UnitId,
                Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, OrgId).UnitSymbol),
                StockInQty = info.StockInQty,
                StockOutQty = info.StockOutQty,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
            }).AsEnumerable();

            warehouseStockInfoDTO = warehouseStockInfoDTO.Where(ws =>
            (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId)
            && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId)
            && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)
            && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
            ).ToList();

            List<WarehouseStockInfoViewModel> warehouseStockInfoViews = new List<WarehouseStockInfoViewModel>();
            AutoMapper.Mapper.Map(warehouseStockInfoDTO, warehouseStockInfoViews);
            return PartialView("_WarehouseStockInfoList", warehouseStockInfoViews);
        }

        public ActionResult CreateStock()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult SaveWarehouseStockIn(List<WarehouseStockDetailViewModel> models)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && models.Count > 0)
            {
                try
                {
                    List<WarehouseStockDetailDTO> dtos = new List<WarehouseStockDetailDTO>();
                    AutoMapper.Mapper.Map(models, dtos);
                    isSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockIn(dtos, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        public ActionResult GetWarehouseStockDetailInfoList(string flag, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();
            }
            else
            {
                var dto = _warehouseStockDetailBusiness.GetWarehouseStockDetailInfoLists(warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum,OrgId);
                IEnumerable<WarehouseStockDetailInfoListViewModel> viewModel = new List<WarehouseStockDetailInfoListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetWarehouseStockDetailInfoList", viewModel);
            }
            return View();
        }
        #endregion

        #region Production Requisition -Table
        [HttpGet]
        public ActionResult GetReqInfoList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Accepted || status.value == RequisitionStatus.Rejected).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            return View();
        }

        // Used By  GetReqInfoList
        public ActionResult GetReqInfoParitalList(string reqCode, long? warehouseId, string status, long? line, long? modelId, string fromDate, string toDate)
        {
            var descriptionData = _descriptionBusiness.GetDescriptionByOrgId(OrgId);
            IEnumerable<RequsitionInfoDTO> requsitionInfoDTO = _requsitionInfoBusiness.GetAllReqInfoByOrgId(OrgId).Where(req =>
                //(req.StateStatus == RequisitionStatus.Pending || req.StateStatus == RequisitionStatus.Accepted || req.StateStatus == RequisitionStatus.Rejected)
                //    &&
                (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode))
                &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId)
                &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim())
                &&
                (line == null || line <= 0 || req.LineId == line)
                &&
                (modelId == null || modelId <= 0 || req.DescriptionId == modelId) &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )
            ).Select(info => new RequsitionInfoDTO
            {
                ReqInfoId = info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineId = info.LineId,
                LineNumber = (_productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, OrgId).LineNumber),
                StateStatus = info.StateStatus,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
                EntryDate = info.EntryDate,
                WarehouseId = info.WarehouseId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, OrgId).WarehouseName),
                ModelName = descriptionData.FirstOrDefault(d => d.DescriptionId == info.DescriptionId).DescriptionName,
                Qty = _requsitionDetailBusiness.GetRequsitionDetailByReqId(info.ReqInfoId, OrgId).Select(s => s.ItemId).Distinct().Count(),
            }).ToList();

            List<RequsitionInfoViewModel> requsitionInfoViewModels = new List<RequsitionInfoViewModel>();
            AutoMapper.Mapper.Map(requsitionInfoDTO, requsitionInfoViewModels);
            return PartialView(requsitionInfoViewModels);
        }

        public ActionResult GetRequsitionDetails(long? reqId)
        {
            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(OrgId).Where(rqd => reqId == null || reqId == 0 || rqd.ReqInfoId == reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, OrgId).UnitSymbol)
            }).ToList();
            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);

            ViewBag.RequisitionStatus = _requsitionInfoBusiness.GetRequisitionById(reqId.Value, OrgId).StateStatus;

            return PartialView("_GetRequsitionDetails", requsitionDetailViewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            if (reqId > 0 && !string.IsNullOrEmpty(status))
            {
                if (RequisitionStatus.Rejected == status || RequisitionStatus.Recheck == status)
                {
                    IsSuccess = _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, OrgId);
                }
                else if (RequisitionStatus.Approved == status)
                {
                    if (GetExecutionStockAvailableForRequisition(reqId).isSuccess == true)
                    {
                        IsSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockOutByProductionRequistion(reqId, status, OrgId, UserId);
                    }
                }
            }
            return Json(IsSuccess);
        }

        [NonAction]
        private ExecutionStateWithText GetExecutionStockAvailableForRequisition(long? reqInfoId)
        {
            ExecutionStateWithText stateWithText = new ExecutionStateWithText();
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId.Value, OrgId).ToArray();
            var warehouseStock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(OrgId).ToList();
            var items = _itemBusiness.GetAllItemByOrgId(OrgId).ToList();
            stateWithText.isSuccess = true;

            for (int i = 0; i < reqDetail.Length; i++)
            {
                var w = warehouseStock.FirstOrDefault(wr => wr.ItemId == reqDetail[i].ItemId);
                if (w != null)
                {
                    if ((w.StockInQty - w.StockOutQty) < reqDetail[i].Quantity)
                    {
                        stateWithText.isSuccess = false;
                        stateWithText.text += items.FirstOrDefault(it => it.ItemId == reqDetail[i].ItemId).ItemName + " does not have enough stock </br>";
                        break;
                    }
                }
                else
                {
                    stateWithText.isSuccess = false;
                    stateWithText.text += items.FirstOrDefault(it => it.ItemId == reqDetail[i].ItemId).ItemName + " does not have enough stock </br>";
                    break;
                }
            }
            return stateWithText;
        }
        #endregion

        #region Item Return -Table
        public ActionResult GetItemReturnList(string flag, string code, long? lineId, long? modelId, long? warehouseId, string status, string returnType, string faultyCase, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.Status = Utility.ListOfReqStatus().Where(s
 => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModel = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).ToList();
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).ToList();
                IEnumerable<ItemReturnInfoDTO> itemReturnInfoDTOs = _itemReturnInfoBusiness.GetItemReturnInfos(OrgId).Where(i => 1 == 1
                && (code == null || code.Trim() == "" || i.IRCode.Contains(code))
                && (lineId == null || lineId <= 0 || i.LineId == lineId)
                && (warehouseId == null || warehouseId <= 0 || i.WarehouseId == warehouseId)
                && ((status == null && (i.StateStatus == RequisitionStatus.Approved || i.StateStatus == RequisitionStatus.Accepted)) || (status.Trim() == "" && (i.StateStatus == RequisitionStatus.Approved || i.StateStatus == RequisitionStatus.Accepted)) || i.StateStatus == status.Trim())
                && (returnType == null || returnType.Trim() == "" || i.ReturnType == returnType.Trim())
                && (faultyCase == null || faultyCase.Trim() == "" || i.FaultyCase == faultyCase.Trim())
                && (modelId == null || modelId <= 0 || i.DescriptionId == modelId)
                &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        i.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        i.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && i.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && i.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )
                ).Select(i => new ItemReturnInfoDTO
                {
                    IRInfoId = i.IRInfoId,
                    IRCode = i.IRCode,
                    ReturnType = i.ReturnType,
                    FaultyCase = i.FaultyCase,
                    LineId = lines.Where(l => l.LineId == i.LineId).FirstOrDefault().LineId,
                    LineNumber = lines.Where(l => l.LineId == i.LineId).FirstOrDefault().LineNumber,
                    WarehouseId = warehouses.Where(w => w.Id == i.WarehouseId).FirstOrDefault().Id,
                    WarehouseName = warehouses.Where(w => w.Id == i.WarehouseId).FirstOrDefault().WarehouseName,
                    Qty = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(OrgId, i.IRInfoId).Count(),
                    StateStatus = i.StateStatus,
                    EntryDate = i.EntryDate

                }).ToList();

                List<ItemReturnInfoViewModel> itemReturnInfoViewModels = new List<ItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(itemReturnInfoDTOs, itemReturnInfoViewModels);
                return PartialView("_GetItemReturnList", itemReturnInfoViewModels);
            }
        }

        public ActionResult GetProductionItemReturnDetails(long itemReturnInfoId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(OrgId);
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId);
            var units = _unitBusiness.GetAllUnitByOrgId(OrgId);
            IEnumerable<ItemReturnDetailDTO> itemReturnDetailDTOs = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(OrgId, itemReturnInfoId).Select(s => new ItemReturnDetailDTO
            {
                IRDetailId = s.IRDetailId,
                ItemTypeId = s.ItemTypeId,
                ItemTypeName = itemTypes.FirstOrDefault(i => i.ItemId == s.ItemTypeId).ItemName,
                ItemId = s.ItemId,
                ItemName = items.FirstOrDefault(i => i.ItemId == s.ItemId).ItemName,
                UnitId = s.UnitId,
                UnitName = units.FirstOrDefault(i => i.UnitId == s.UnitId).UnitName,
                Quantity = s.Quantity,
                Remarks = s.Remarks
            }).ToList();

            var info = _itemReturnInfoBusiness.GetItemReturnInfo(OrgId, itemReturnInfoId);
            ItemReturnInfoViewModel itemReturnInfoViewModel = new ItemReturnInfoViewModel()
            {
                IRCode = info.IRCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, OrgId).WarehouseName
            };

            ViewBag.ReturnInfoViewModel = itemReturnInfoViewModel;
            ViewBag.RequisitionStatus = info.StateStatus;

            IEnumerable<ItemReturnDetailViewModel> itemReturnDetailViews = new List<ItemReturnDetailViewModel>();
            AutoMapper.Mapper.Map(itemReturnDetailDTOs, itemReturnDetailViews);
            return PartialView("_GetProductionItemReturnDetails", itemReturnDetailViews);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveWarehouseStockInByItemReturn(long returnInfoId, string status)
        {
            bool IsSuccess = false;
            if (returnInfoId > 0 && !string.IsNullOrEmpty(status))
            {
                IsSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockInByProductionItemReturn(returnInfoId, status, OrgId, UserId);
            }
            return Json(IsSuccess);
        }

        #endregion

        #region RepairStock -Table
        public ActionResult GetRepairStockInfoList(string flag, long? LineId, long? ModelId, long? WarehouseId, long? ItemTypeId, long? ItemId, string lessOrEq)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else
            {
                var descriptions = _descriptionBusiness.GetDescriptionByOrgId(OrgId);
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId);
                IEnumerable<RepairStockInfoDTO> repairStockInfoDTOs = _repairStockInfoBusiness.GetRepairStockInfos(OrgId).Select(info => new RepairStockInfoDTO
                {
                    RStockInfoId = info.RStockInfoId,
                    DescriptionId = info.DescriptionId,
                    LineId = info.LineId,
                    WarehouseId = info.WarehouseId,
                    Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, OrgId).ItemName),
                    ItemId = info.ItemId,
                    Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, OrgId).ItemName),
                    UnitId = info.UnitId,
                    Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, OrgId).UnitSymbol),
                    StockInQty = info.StockInQty,
                    StockOutQty = info.StockOutQty,
                    Remarks = info.Remarks,
                    OrganizationId = info.OrganizationId,
                    ModelName = (descriptions.FirstOrDefault(m => m.DescriptionId == info.DescriptionId).DescriptionName),
                    LineNumber = (lines.FirstOrDefault(l => l.LineId == info.LineId).LineNumber)

                }).AsEnumerable();

                repairStockInfoDTOs = repairStockInfoDTOs.Where(ws =>
                (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId) &&
                (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId)
                && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)
                && (LineId == null || LineId == 0 || ws.LineId == LineId)
                && (ModelId == null || ModelId == 0 || ws.DescriptionId == ModelId)
                && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
                ).ToList();

                List<RepairStockInfoViewModel> repairStockInfoViewModels = new List<RepairStockInfoViewModel>();
                AutoMapper.Mapper.Map(repairStockInfoDTOs, repairStockInfoViewModels);
                return PartialView("_RepairStockInfoList", repairStockInfoViewModels);
            }
        }

        public ActionResult GetRepairStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();
            }
            else
            {
                var dto = _repairStockDetailBusiness.GetRepairStockDetailList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum,OrgId);
                IEnumerable<RepairStockDetailListViewModel> viewModel = new List<RepairStockDetailListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetRepairStockDetailInfoList", viewModel);
            }
            return View();
        }



        #endregion

        #region Finish Goods Stock

        public ActionResult GetFinishGoodsSendToWarehouse(string flag, long? lineId, long? warehouseId, long? modelId, string status, string fromDate, string toDate, string refNo)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Where(w => w.WarehouseName == "Warehouse 2" || w.WarehouseName == "Warehouse 3").Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlSendStatus = Utility.ListOfFinishGoodsSendStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                return View();
            }
            else
            {
                var tblwarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId);
                var tblLine = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId);
                var tblModel = _descriptionBusiness.GetDescriptionByOrgId(OrgId);

                List<FinishGoodsSendToWarehouseInfoDTO> listOfFinishGoodsSendInfo = _finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseList(OrgId)
                     .Where(f => 1 == 1 &&
                         (refNo == null || refNo.Trim() == "" || f.RefferenceNumber.Contains(refNo)) &&
                         (warehouseId == null || warehouseId <= 0 || f.WarehouseId == warehouseId) &&
                         (status == null || status.Trim() == "" || f.StateStatus == status.Trim()) &&
                         (lineId == null || lineId <= 0 || f.LineId == lineId) &&
                         (modelId == null || modelId <= 0 || f.DescriptionId == modelId) &&
                         (
                             (fromDate == null && toDate == null)
                             ||
                              (fromDate == "" && toDate == "")
                             ||
                             (fromDate.Trim() != "" && toDate.Trim() != "" &&

                                 f.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                                 f.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                             ||
                             (fromDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                             ||
                             (toDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                         )
                     )
                     .Select(f => new FinishGoodsSendToWarehouseInfoDTO
                     {
                         SendId = f.SendId,
                         LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(f.LineId, OrgId).LineNumber,
                         WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(f.WarehouseId, OrgId).WarehouseName),
                         ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(f.DescriptionId, OrgId).DescriptionName,
                         StateStatus = f.StateStatus,
                         ItemCount = this._finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(f.SendId, OrgId).Count(),
                         Remarks = f.Remarks,
                         EntryDate = f.EntryDate,
                         RefferenceNumber = f.RefferenceNumber
                     }).ToList();

                List<FinishGoodsSendToWarehouseInfoViewModel> listOfFinishGoodsSendToWarehouseInfoViewModels = new List<FinishGoodsSendToWarehouseInfoViewModel>();
                AutoMapper.Mapper.Map(listOfFinishGoodsSendInfo, listOfFinishGoodsSendToWarehouseInfoViewModels);
                return PartialView("_GetFinishGoodsSendToWarehouse", listOfFinishGoodsSendToWarehouseInfoViewModels);
            }
        }

        public ActionResult GetFinishGoodsSendItemDetail(long sendId)
        {
            List<FinishGoodsSendToWarehouseDetailViewModel> viewModels = new List<FinishGoodsSendToWarehouseDetailViewModel>();
            if (sendId > 0)
            {
                var info = _finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseById(sendId, OrgId);
                FinishGoodsSendToWarehouseInfoViewModel infoViewModel = new FinishGoodsSendToWarehouseInfoViewModel
                {
                    SendId = info.SendId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, OrgId).LineNumber,
                    RefferenceNumber = info.RefferenceNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, OrgId).WarehouseName,
                    DescriptionId = info.DescriptionId,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, OrgId).DescriptionName,
                    StateStatus = info.StateStatus
                };

                ViewBag.FinishGoodsSendInfo = infoViewModel;
                List<FinishGoodsSendToWarehouseDetailDTO> dtos = new List<FinishGoodsSendToWarehouseDetailDTO>();
                dtos = _finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(sendId, OrgId).Select(f => new FinishGoodsSendToWarehouseDetailDTO
                {
                    SendDetailId = f.SendDetailId,
                    ItemTypeName = _itemTypeBusiness.GetItemType(f.ItemTypeId, OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemById(f.ItemId, OrgId).ItemName,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(f.UnitId, OrgId).UnitSymbol,
                    Quantity = f.Quantity
                }).ToList();

                AutoMapper.Mapper.Map(dtos, viewModels);
            }
            return PartialView("_GetFinishGoodsSendItemDetail", viewModels);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinishGoodsItems(long sendId)
        {
            bool IsSuccess = false;
            if(sendId > 0)
            {
                IsSuccess=_finishGoodsSendToWarehouseInfoBusiness.SaveFinishGoodsStatus(sendId, UserId, OrgId);
            }
            return Json(IsSuccess);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}