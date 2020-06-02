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
using ERPBO.ControlPanel.ViewModels;
using ERPBO.Inventory.DomainModel;
using ERPWeb.Infrastructure;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
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
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;

        public InventoryController(IWarehouseBusiness warehouseBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, IProductionLineBusiness productionLineBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IRepairStockInfoBusiness repairStockInfoBusiness, IRepairStockDetailBusiness repairStockDetailBusiness, IDescriptionBusiness descriptionBusiness, IFinishGoodsSendToWarehouseInfoBusiness finishGoodsSendToWarehouseInfoBusiness, IFinishGoodsSendToWarehouseDetailBusiness finishGoodsSendToWarehouseDetailBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness)
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
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
        }

        // GET: Account
        #region Description
        public ActionResult GetDescriptionList(int? page)
        {
            IEnumerable<DescriptionDTO> dto = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new DescriptionDTO
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
                UpdateDate = des.UpdateDate,
                EntryUser = UserForEachRecord(des.EUserId.Value).UserName,
                UpdateUser = (des.UpUserId == null || des.UpUserId == 0) ? "" : UserForEachRecord(des.UpUserId.Value).UserName

            }).OrderBy(des => des.DescriptionId).ToList();

            
            IEnumerable<DescriptionViewModel> viewModel = new List<DescriptionViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            return View(viewModel);
        }
        #endregion

        #region Warehouse - Table
        [HttpGet]
        public ActionResult GetWarehouseList()
        {
            IEnumerable<WarehouseDTO> dto = _warehouseBusiness.GetAllWarehouseByOrgId(1).Select(ware => new WarehouseDTO
            {
                Id = ware.Id,
                WarehouseName = ware.WarehouseName,
                Remarks = ware.Remarks,
                StateStatus = (ware.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = ware.OrganizationId,
                EntryUser = UserForEachRecord(ware.EUserId.Value).UserName,
                UpdateUser = (ware.UpUserId == null || ware.UpUserId == 0) ? "" : UserForEachRecord(ware.UpUserId.Value).UserName

            }).ToList();
            List<WarehouseViewModel> viewModel = new List<WarehouseViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetWarehouseList");
            return View(viewModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveWarehouse(WarehouseViewModel viewModel)
        {
            bool isSuccess = false;
            var pre = UserPrivilege("Inventory", "GetWarehouseList");
            var permission = (viewModel.Id == 0 && pre.Add) || (viewModel.Id > 0 && pre.Edit);
            if (ModelState.IsValid && permission)
            {
                try
                {
                    WarehouseDTO dto = new WarehouseDTO();
                    AutoMapper.Mapper.Map(viewModel, dto);
                    isSuccess = _warehouseBusiness.SaveWarehouse(dto, User.UserId, User.OrgId);
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
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();
            var allData = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId);
            IEnumerable<ItemTypeDTO> dto = allData.Select(item => new ItemTypeDTO
            {
                ItemId = item.ItemId,
                WarehouseId = item.WarehouseId,
                ShortName = item.ShortName,
                ItemName = item.ItemName,
                Remarks = item.Remarks,
                StateStatus = (item.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = item.OrganizationId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(item.WarehouseId, User.OrgId).WarehouseName),
                EntryUser = UserForEachRecord(item.EUserId.Value).UserName,
                UpdateUser = (item.UpUserId == null || item.UpUserId == 0) ? "" : UserForEachRecord(item.UpUserId.Value).UserName
            }).OrderBy(item => item.ItemId).ToPagedList(page ?? 1, 15);

            IEnumerable<ItemTypeViewModel> viewModels = new List<ItemTypeViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemTypeList");
            return View(viewModels);
        }

        public ActionResult SaveItemType(ItemTypeViewModel itemTypeViewModel)
        {
            bool isSuccess = false;
            var privilege = UserPrivilege("Inventory", "GetItemTypeList");
            bool permission = (itemTypeViewModel.ItemId == 0 && privilege.Add) || (itemTypeViewModel.ItemId > 0 && privilege.Edit);
            if (ModelState.IsValid && permission)
            {
                try
                {
                    ItemTypeDTO dto = new ItemTypeDTO();
                    AutoMapper.Mapper.Map(itemTypeViewModel, dto);
                    isSuccess = _itemTypeBusiness.SaveItemType(dto, User.UserId, User.OrgId);
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
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetAllUnitList");
            IEnumerable<UnitDomainDTO> unitDomains = _unitBusiness.GetAllUnitByOrgId(User.OrgId).Select(unit => new UnitDomainDTO
            {
                UnitId = unit.UnitId,
                UnitName = unit.UnitName,
                UnitSymbol = unit.UnitSymbol,
                Remarks = unit.Remarks,
                OrganizationId = unit.OrganizationId,
                EntryUser = UserForEachRecord(unit.EUserId.Value).UserName,
                UpdateUser = (unit.UpUserId == null || unit.UpUserId == 0) ? "" : UserForEachRecord(unit.UpUserId.Value).UserName
            }).ToList();
            List<UnitViewModel> unitViewModels = new List<UnitViewModel>();
            AutoMapper.Mapper.Map(unitDomains, unitViewModels);
            return View(unitViewModels);
        }

        public ActionResult SaveUnit(UnitViewModel unitViewModel)
        {
            bool isSuccess = false;
            var privilege = UserPrivilege("Inventory", "GetAllUnitList");
            var permission = (unitViewModel.UnitId == 0 && privilege.Add) || (unitViewModel.UnitId > 0 && privilege.Edit);
            if (ModelState.IsValid && permission)
            {
                try
                {
                    UnitDomainDTO dto = new UnitDomainDTO();
                    AutoMapper.Mapper.Map(unitViewModel, dto);
                    isSuccess = _unitBusiness.SaveUnit(dto, User.UserId, User.OrgId);
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
        public ActionResult GetItemList()
        {
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemList");
            ViewBag.ddlItemTypeName = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).Select(itemtype => new SelectListItem { Text = itemtype.ItemName, Value = itemtype.ItemId.ToString() }).ToList();

            ViewBag.ddlUnitName = _unitBusiness.GetAllUnitByOrgId(User.OrgId).Select(unit => new SelectListItem { Text = unit.UnitName, Value = unit.UnitId.ToString() }).ToList();

            var allData = _itemBusiness.GetAllItemByOrgId(1);
            IEnumerable<ItemDomainDTO> dto = allData.Select(item => new ItemDomainDTO
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Remarks = item.Remarks,
                StateStatus = (item.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = item.OrganizationId,
                ItemTypeId = item.ItemTypeId,
                ItemTypeName = _itemTypeBusiness.GetItemType(item.ItemTypeId, User.OrgId).ItemName,
                UnitId = item.UnitId,
                UnitName = _unitBusiness.GetUnitOneByOrgId(item.UnitId, User.OrgId).UnitName,
                ItemCode = item.ItemCode,
                EntryUser = UserForEachRecord(item.EUserId.Value).UserName,
                UpdateUser = (item.UpUserId == null || item.UpUserId == 0) ? "" : UserForEachRecord(item.UpUserId.Value).UserName
            }).OrderBy(item => item.ItemId).ToList();
            IEnumerable<ItemViewModel> viewModel = new List<ItemViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            return View(viewModel);
        }
        public ActionResult SaveItem(ItemViewModel itemViewModel)
        {
            bool isSuccess = false;
            var privilege = UserPrivilege("Inventory", "GetItemList");
            var permission = (itemViewModel.ItemId == 0 && privilege.Add) || (itemViewModel.ItemId > 0 && privilege.Edit);
            if (ModelState.IsValid && permission)
            {
                try
                {
                    ItemDomainDTO dto = new ItemDomainDTO();
                    AutoMapper.Mapper.Map(itemViewModel, dto);
                    isSuccess = _itemBusiness.SaveItem(dto, User.UserId, User.OrgId);
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
            ItemDomainDTO itemDTO = _itemBusiness.GetItemById(id, User.OrgId);
            itemDTO.UnitName = _unitBusiness.GetUnitOneByOrgId(itemDTO.UnitId, User.OrgId).UnitName;
            itemDTO.ItemTypeName = _itemTypeBusiness.GetItemType(itemDTO.ItemTypeId, User.OrgId).ItemName;
            ItemViewModel itemViewModel = new ItemViewModel();
            AutoMapper.Mapper.Map(itemDTO, itemViewModel);
            return Json(itemViewModel);
        }
        #endregion

        #region Warehouse Stock Info -Table
        [HttpGet]
        public ActionResult GetWarehouseStockInfoList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetWarehouseStockInfoList");
            return View();
        }

        [HttpGet]
        public ActionResult GetWarehouseStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId, string lessOrEq,int page=1)
        {
            IEnumerable<WarehouseStockInfoDTO> dto = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(User.OrgId).Select(info => new WarehouseStockInfoDTO
            {
                StockInfoId = info.StockInfoId,
                WarehouseId = info.WarehouseId,
                Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
                ItemTypeId = info.ItemTypeId,
                ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                ItemId = info.ItemId,
                Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                UnitId = info.UnitId,
                Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
                StockInQty = info.StockInQty,
                StockOutQty = info.StockOutQty,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId
            }).AsEnumerable();

            dto = dto.Where(ws =>
            (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId)
            && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId)
            && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)
            && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
            ).ToList();

            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
            dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //-----------------//

            List<WarehouseStockInfoViewModel> warehouseStockInfoViews = new List<WarehouseStockInfoViewModel>();
            AutoMapper.Mapper.Map(dto, warehouseStockInfoViews);
            return PartialView("_WarehouseStockInfoList", warehouseStockInfoViews);
        }

        public ActionResult CreateStock()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
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
            var pre = UserPrivilege("Inventory", "GetWarehouseStockInfoList");
            var permission = ((pre.Add) || (pre.Edit));
            if (ModelState.IsValid && models.Count > 0 && permission)
            {
                try
                {
                    List<WarehouseStockDetailDTO> dtos = new List<WarehouseStockDetailDTO>();
                    AutoMapper.Mapper.Map(models, dtos);
                    isSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockIn(dtos, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        public ActionResult GetWarehouseStockDetailInfoList(string flag, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
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
                var dto = _warehouseStockDetailBusiness.GetWarehouseStockDetailInfoLists(warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum, User.OrgId).OrderByDescending(s => s.StockDetailId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
                dto = dto.Skip((page - 1) * 15).Take(15).ToList();
                //-----------------//

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
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value == RequisitionStatus.Pending || status.value == RequisitionStatus.Accepted || status.value == RequisitionStatus.Rejected).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlRequisitionType = Utility.ListOfRequisitionType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

            return View();
        }

        // Used By  GetReqInfoList
        public ActionResult GetReqInfoParitalList(string reqCode, long? warehouseId, string status, long? line, long? modelId, string fromDate, string toDate, string requisitionType,int page=1)
        {
            var descriptionData = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);
            IEnumerable<RequsitionInfoDTO> dto = _requsitionInfoBusiness.GetAllReqInfoByOrgId(User.OrgId).Where(req =>
                (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode))
                &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId)
                &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim())
                &&
                (requisitionType == null || requisitionType.Trim() == "" || req.RequisitionType == requisitionType.Trim()) &&
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
                LineNumber = (_productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, User.OrgId).LineNumber),
                StateStatus = info.StateStatus,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
                EntryDate = info.EntryDate,
                WarehouseId = info.WarehouseId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName),
                ModelName = descriptionData.FirstOrDefault(d => d.DescriptionId == info.DescriptionId).DescriptionName,
                Qty = _requsitionDetailBusiness.GetRequsitionDetailByReqId(info.ReqInfoId, User.OrgId).Select(s => s.ItemId).Distinct().Count(),
                RequisitionType = info.RequisitionType,
                EntryUser = UserForEachRecord(info.EUserId.Value).UserName,
                UpdateUser = (info.UpUserId == null || info.UpUserId == 0) ? "" : UserForEachRecord(info.UpUserId.Value).UserName
            }).OrderByDescending(s => s.ReqInfoId).ToList();

            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
            dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //-----------------//

            List<RequsitionInfoViewModel> requsitionInfoViewModels = new List<RequsitionInfoViewModel>();
            AutoMapper.Mapper.Map(dto, requsitionInfoViewModels);
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetReqInfoList");
            return PartialView(requsitionInfoViewModels);
        }

        public ActionResult GetRequsitionDetails(long? reqId)
        {
            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(User.OrgId).Where(rqd => reqId == null || reqId == 0 || rqd.ReqInfoId == reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, User.OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, User.OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, User.OrgId).UnitSymbol)
            }).ToList();
            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);

            ViewBag.RequisitionStatus = _requsitionInfoBusiness.GetRequisitionById(reqId.Value, User.OrgId).StateStatus;
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetReqInfoList");
            return PartialView("_GetRequsitionDetails", requsitionDetailViewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            var pre = UserPrivilege("Inventory", "GetReqInfoList");
            var permission = ((pre.Edit) || (pre.Add));
            if (reqId > 0 && !string.IsNullOrEmpty(status) && permission)
            {
                if (RequisitionStatus.Rejected == status || RequisitionStatus.Recheck == status)
                {
                    IsSuccess = _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, User.OrgId, User.UserId);
                }
                else if (RequisitionStatus.Approved == status)
                {
                    if (GetExecutionStockAvailableForRequisition(reqId).isSuccess == true)
                    {
                        IsSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockOutByProductionRequistion(reqId, status, User.OrgId, User.UserId);
                    }
                }
            }
            return Json(IsSuccess);
        }

        [NonAction]
        private ExecutionStateWithText GetExecutionStockAvailableForRequisition(long? reqInfoId)
        {
            ExecutionStateWithText stateWithText = new ExecutionStateWithText();
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId.Value, User.OrgId).ToArray();
            var warehouseStock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(User.OrgId).ToList();
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();
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
        public ActionResult GetItemReturnList(string flag, string code, long? lineId, long? modelId, long? warehouseId, string status, string returnType, string faultyCase, string fromDate, string toDate,int page=1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemReturnList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.Status = Utility.ListOfReqStatus().Where(s
 => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModel = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).ToList();
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).ToList();
                IEnumerable<ItemReturnInfoDTO> dto = _itemReturnInfoBusiness.GetItemReturnInfos(User.OrgId).Where(i => 1 == 1
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
                    Qty = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(User.OrgId, i.IRInfoId).Count(),
                    StateStatus = i.StateStatus,
                    EntryDate = i.EntryDate,
                    EntryUser = UserForEachRecord(i.EUserId.Value).UserName,
                    UpdateUser = (i.UpUserId == null || i.UpUserId == 0) ? "" : UserForEachRecord(i.UpUserId.Value).UserName

                }).OrderByDescending(s => s.IRInfoId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<ItemReturnInfoViewModel> itemReturnInfoViewModels = new List<ItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(dto, itemReturnInfoViewModels);
                return PartialView("_GetItemReturnList", itemReturnInfoViewModels);
            }
        }

        public ActionResult GetProductionItemReturnDetails(long itemReturnInfoId)
        {
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemReturnList");
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId);
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId);
            var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId);
            IEnumerable<ItemReturnDetailDTO> itemReturnDetailDTOs = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(User.OrgId, itemReturnInfoId).Select(s => new ItemReturnDetailDTO
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

            var info = _itemReturnInfoBusiness.GetItemReturnInfo(User.OrgId, itemReturnInfoId);
            ItemReturnInfoViewModel itemReturnInfoViewModel = new ItemReturnInfoViewModel()
            {
                IRCode = info.IRCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, User.OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName
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
            var privilege = UserPrivilege("Inventory", "GetItemReturnList");
            if (returnInfoId > 0 && !string.IsNullOrEmpty(status) && privilege.Edit)
            {
                IsSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockInByProductionItemReturn(returnInfoId, status, User.OrgId, User.UserId);
            }
            return Json(IsSuccess);
        }

        #endregion

        #region RepairStock -Table
        public ActionResult GetRepairStockInfoList(string flag, long? LineId, long? ModelId, long? WarehouseId, long? ItemTypeId, long? ItemId, string lessOrEq, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetRepairStockInfoList");
                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else
            {
                var descriptions = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId);
                IEnumerable<RepairStockInfoDTO> dto = _repairStockInfoBusiness.GetRepairStockInfos(User.OrgId).Select(info => new RepairStockInfoDTO
                {
                    RStockInfoId = info.RStockInfoId,
                    DescriptionId = info.DescriptionId,
                    LineId = info.LineId,
                    WarehouseId = info.WarehouseId,
                    Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                    ItemId = info.ItemId,
                    Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                    UnitId = info.UnitId,
                    Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
                    StockInQty = info.StockInQty,
                    StockOutQty = info.StockOutQty,
                    Remarks = info.Remarks,
                    OrganizationId = info.OrganizationId,
                    ModelName = (descriptions.FirstOrDefault(m => m.DescriptionId == info.DescriptionId).DescriptionName),
                    LineNumber = (lines.FirstOrDefault(l => l.LineId == info.LineId).LineNumber)

                }).AsEnumerable();

                dto = dto.Where(ws =>
                (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId) &&
                (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId)
                && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)
                && (LineId == null || LineId == 0 || ws.LineId == LineId)
                && (ModelId == null || ModelId == 0 || ws.DescriptionId == ModelId)
                && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
                ).OrderByDescending(s => s.RStockInfoId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<RepairStockInfoViewModel> repairStockInfoViewModels = new List<RepairStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, repairStockInfoViewModels);
                return PartialView("_RepairStockInfoList", repairStockInfoViewModels);
            }
        }

        public ActionResult GetRepairStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, string returnType,string faultyCase, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                ViewBag.ddlReturnType = Utility.ListOfReturnType().Where(r => r.text == ReturnType.RepairFaultyReturn || r.text == ReturnType.ProductionFaultyReturn).Select(r => new SelectListItem
                {
                    Text = r.text,
                    Value = r.value
                }).ToList();

                ViewBag.ddlFaultyCase = Utility.ListOfFaultyCase().Select(f => new SelectListItem
                {
                    Text = f.text,
                    Value = f.value
                }).ToList();
            }
            else
            {
                var dto = _repairStockDetailBusiness.GetRepairStockDetailList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum, User.OrgId, returnType, faultyCase).OrderByDescending(s => s.RStockDetailId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                IEnumerable<RepairStockDetailListViewModel> viewModel = new List<RepairStockDetailListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetRepairStockDetailInfoList", viewModel);
            }
            return View();
        }

        #endregion

        #region Finish Goods Stock
        public ActionResult GetFinishGoodsSendToWarehouse(string flag, long? lineId, long? warehouseId, long? modelId, string status, string fromDate, string toDate, string refNo, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(w => w.WarehouseName == "Warehouse 2" || w.WarehouseName == "Warehouse 3").Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlSendStatus = Utility.ListOfFinishGoodsSendStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                return View();
            }
            else
            {
                ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetFinishGoodsSendToWarehouse");
                var tblwarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId);
                var tblLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId);
                var tblModel = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);

                List<FinishGoodsSendToWarehouseInfoDTO> dto = _finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseList(User.OrgId)
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
                         LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(f.LineId, User.OrgId).LineNumber,
                         WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(f.WarehouseId, User.OrgId).WarehouseName),
                         ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(f.DescriptionId, User.OrgId).DescriptionName,
                         StateStatus = f.StateStatus,
                         ItemCount = this._finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(f.SendId, User.OrgId).Count(),
                         Remarks = f.Remarks,
                         EntryDate = f.EntryDate,
                         RefferenceNumber = f.RefferenceNumber,
                         EntryUser = UserForEachRecord(f.EUserId.Value).UserName,
                         UpdateUser = (f.UpUserId == null || f.UpUserId == 0) ? "" : UserForEachRecord(f.UpUserId.Value).UserName
                     }).OrderByDescending(s => s.SendId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<FinishGoodsSendToWarehouseInfoViewModel> listOfFinishGoodsSendToWarehouseInfoViewModels = new List<FinishGoodsSendToWarehouseInfoViewModel>();
                AutoMapper.Mapper.Map(dto, listOfFinishGoodsSendToWarehouseInfoViewModels);
                return PartialView("_GetFinishGoodsSendToWarehouse", listOfFinishGoodsSendToWarehouseInfoViewModels);
            }
        }

        public ActionResult GetFinishGoodsSendItemDetail(long sendId)
        {
            List<FinishGoodsSendToWarehouseDetailViewModel> viewModels = new List<FinishGoodsSendToWarehouseDetailViewModel>();
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetFinishGoodsSendToWarehouse");
            if (sendId > 0)
            {
                var info = _finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseById(sendId, User.OrgId);
                FinishGoodsSendToWarehouseInfoViewModel infoViewModel = new FinishGoodsSendToWarehouseInfoViewModel
                {
                    SendId = info.SendId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, User.OrgId).LineNumber,
                    RefferenceNumber = info.RefferenceNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName,
                    DescriptionId = info.DescriptionId,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, User.OrgId).DescriptionName,
                    StateStatus = info.StateStatus
                };

                ViewBag.FinishGoodsSendInfo = infoViewModel;
                List<FinishGoodsSendToWarehouseDetailDTO> dtos = new List<FinishGoodsSendToWarehouseDetailDTO>();
                dtos = _finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(sendId, User.OrgId).Select(f => new FinishGoodsSendToWarehouseDetailDTO
                {
                    SendDetailId = f.SendDetailId,
                    ItemTypeName = _itemTypeBusiness.GetItemType(f.ItemTypeId, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemById(f.ItemId, User.OrgId).ItemName,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(f.UnitId, User.OrgId).UnitSymbol,
                    Quantity = f.Quantity
                }).OrderByDescending(s => s.SendDetailId).ToList();

                AutoMapper.Mapper.Map(dtos, viewModels);
            }
            return PartialView("_GetFinishGoodsSendItemDetail", viewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinishGoodsItems(long sendId)
        {
            bool IsSuccess = false;
            var privilege = UserPrivilege("Inventory", "GetFinishGoodsSendToWarehouse");
            if (sendId > 0 && privilege.Edit)
            {
                IsSuccess = _finishGoodsSendToWarehouseInfoBusiness.SaveFinishGoodsStatus(sendId, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        #endregion

        #region Item Preparation
        public ActionResult GetItemPreparation(string flag, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? id, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemPreparation");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag == Flag.View)
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).ToList();
                var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).ToList();
                var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();
                var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId).ToList();
                var mobileModels = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).ToList();

                var dto = _itemPreparationInfoBusiness.GetItemPreparationInfosByOrgId(User.OrgId).Where(i => 1 == 1 &&
                    (modelId == null || modelId <= 0 || modelId == i.DescriptionId) &&
                    (warehouseId == null || warehouseId <= 0 || warehouseId == i.WarehouseId) &&
                    (itemTypeId == null || itemTypeId <= 0 || itemTypeId == i.ItemTypeId) &&
                    (itemId == null || itemId <= 0 || itemId == i.ItemId)
                ).Select(i => new ItemPreparationInfoDTO
                {
                    PreparationInfoId = i.PreparationInfoId,
                    WarehouseName = warehouses.FirstOrDefault(w => w.Id == i.WarehouseId).WarehouseName,
                    ItemTypeName = itemTypes.FirstOrDefault(it => it.ItemId == i.ItemTypeId).ItemName,
                    ItemName = items.FirstOrDefault(it => it.ItemId == i.ItemId).ItemName,
                    UnitName = units.FirstOrDefault(it => it.UnitId == i.UnitId).UnitSymbol,
                    ModelName = mobileModels.FirstOrDefault(it => it.DescriptionId == i.DescriptionId).DescriptionName,
                    ItemCount = _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoId(i.PreparationInfoId, User.OrgId).Count(),
                    EntryDate = i.EntryDate,
                    EntryUser = UserForEachRecord(i.EUserId.Value).UserName,
                    UpdateUser = (i.UpUserId == null || i.UpUserId == 0) ? "" : UserForEachRecord(i.UpUserId.Value).UserName
                }).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<ItemPreparationInfoViewModel> viewModels = new List<ItemPreparationInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetItemPreparation", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == Flag.Detail)
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).ToList();
                var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).ToList();
                var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();
                var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId).ToList();
                var mobileModels = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).ToList();
                var info = _itemPreparationInfoBusiness.GetItemPreparationInfoOneByOrgId(id.Value, User.OrgId);

                List<ItemPreparationDetailViewModel> details = new List<ItemPreparationDetailViewModel>();
                if (info != null)
                {
                    ViewBag.Info = new ItemPreparationInfoViewModel
                    {
                        ModelName = mobileModels.FirstOrDefault(it => it.DescriptionId == info.DescriptionId).DescriptionName,
                        ItemTypeName = itemTypes.FirstOrDefault(it => it.ItemId == info.ItemTypeId).ItemName,
                        ItemName = items.FirstOrDefault(it => it.ItemId == info.ItemId).ItemName
                    };
                    details = _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoId(id.Value, User.OrgId).Select(i => new ItemPreparationDetailViewModel
                    {
                        WarehouseName = warehouses.FirstOrDefault(w => w.Id == i.WarehouseId).WarehouseName,
                        ItemTypeName = itemTypes.FirstOrDefault(it => it.ItemId == i.ItemTypeId).ItemName,
                        ItemName = items.FirstOrDefault(it => it.ItemId == i.ItemId).ItemName,
                        UnitName = units.FirstOrDefault(u => u.UnitId == i.UnitId).UnitSymbol,
                        Quantity = i.Quantity,
                        Remarks = i.Remarks
                    }).ToList();
                }
                else
                {
                    ViewBag.Info = new ItemPreparationInfoViewModel();
                }
                return PartialView("_GetItemPreparationDetail", details);
            }
            else
            {
                if (!string.IsNullOrEmpty(flag) && flag == Flag.Delete)
                {
                    bool IsSuccess = false;
                    if (id != null && id > 0)
                    {
                        IsSuccess = _itemPreparationInfoBusiness.DeleteItemPreparation(id.Value, User.UserId, User.OrgId);
                    }
                    return Json(IsSuccess);
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult CreateItemPreparation(long? id)
        {
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

            ViewBag.ddlWarehouseTarget = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlWarehouseSource = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            return View();
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveItemPreparation(ItemPreparationInfoViewModel info, List<ItemPreparationDetailViewModel> details)
        {
            bool IsSuccess = false;
            var pre = UserPrivilege("Inventory", "GetItemPreparation");
            var permission = ((pre.Edit) || (pre.Add));
            if (ModelState.IsValid && details.Count > 0 && permission)
            {
                ItemPreparationInfoDTO infoDTO = new ItemPreparationInfoDTO();
                List<ItemPreparationDetailDTO> detailDTOs = new List<ItemPreparationDetailDTO>();
                AutoMapper.Mapper.Map(info, infoDTO);
                AutoMapper.Mapper.Map(details, detailDTOs);
                IsSuccess = _itemPreparationInfoBusiness.SaveItemPreparations(infoDTO, detailDTOs, User.UserId, User.OrgId);
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