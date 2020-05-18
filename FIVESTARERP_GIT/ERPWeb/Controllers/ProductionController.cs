using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.ViewModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using ERPWeb.Filters;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class ProductionController : BaseController
    {
        // GET: Production
        private readonly IWarehouseBusiness _warehouseBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;
        private readonly IProductionStockDetailBusiness _productionStockDetailBusiness;
        
        private readonly IItemBusiness _itemBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness;
        private readonly IItemReturnDetailBusiness _itemReturnDetailBusiness;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly IFinishGoodsInfoBusiness _finishGoodsInfoBusiness;
        private readonly IFinishGoodsRowMaterialBusiness _finishGoodsRowMaterialBusiness;
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IFinishGoodsSendToWarehouseInfoBusiness _finishGoodsSendToWarehouseInfoBusiness;
        private readonly IFinishGoodsSendToWarehouseDetailBusiness _finishGoodsSendToWarehouseDetailBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;

        public ProductionController(IRequsitionInfoBusiness requsitionInfoBusiness, IWarehouseBusiness warehouseBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionLineBusiness productionLineBusiness, IItemBusiness itemBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IProductionStockDetailBusiness productionStockDetailBusiness, IProductionStockInfoBusiness productionStockInfoBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IDescriptionBusiness descriptionBusiness, IFinishGoodsInfoBusiness finishGoodsInfoBusiness, IFinishGoodsRowMaterialBusiness finishGoodsRowMaterialBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness, IFinishGoodsSendToWarehouseInfoBusiness finishGoodsSendToWarehouseInfoBusiness, IFinishGoodsSendToWarehouseDetailBusiness finishGoodsSendToWarehouseDetailBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness)
        {
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._warehouseBusiness = warehouseBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._itemBusiness = itemBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._itemReturnDetailBusiness = itemReturnDetailBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._finishGoodsInfoBusiness = finishGoodsInfoBusiness;
            this._finishGoodsRowMaterialBusiness = finishGoodsRowMaterialBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._finishGoodsSendToWarehouseInfoBusiness = finishGoodsSendToWarehouseInfoBusiness;
            this._finishGoodsSendToWarehouseDetailBusiness = finishGoodsSendToWarehouseDetailBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
        }

        #region ProductionLine
        public ActionResult GetProductionLineList(int? page)
        {
            IPagedList<ProductionLineViewModel> productionLineViewModels = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new ProductionLineViewModel
            {
                LineId = line.LineId,
                LineNumber = line.LineNumber,
                LineIncharge = line.LineIncharge,
                Remarks = line.Remarks,
                StateStatus = (line.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = line.OrganizationId,
                EUserId = line.EUserId,
                EntryDate = line.EntryDate,
                UpUserId = line.UpUserId,
                UpdateDate = line.UpdateDate
            }).OrderBy(line => line.LineId).ToPagedList(page ?? 1, 15);
            IEnumerable<ProductionLineViewModel> productionLineViewModelForPage = new List<ProductionLineViewModel>();
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetProductionLineList");
            return View(productionLineViewModels);
        }
        [HttpPost]
        public ActionResult SaveProductionLine(ProductionLineViewModel productionLineViewModel)
        {
            bool isSuccess = false;
            var privilege = UserPrivilege("Production", "GetProductionLineList");
            var permission = (productionLineViewModel.LineId == 0 && privilege.Add) || (productionLineViewModel.LineId > 0 && privilege.Edit);
            if (ModelState.IsValid && permission)
            {
                try
                {
                    ProductionLineDTO dto = new ProductionLineDTO();
                    AutoMapper.Mapper.Map(productionLineViewModel, dto);
                    isSuccess = _productionLineBusiness.SaveUnit(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Requsition
        [HttpGet]
        public ActionResult GetReqInfoList()
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReqInfoList");
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value != RequisitionStatus.Pending).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            ViewBag.ddlRequisitionType = Utility.ListOfRequisitionType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
            return View();
        }
        public ActionResult CreateRequsition()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
            ViewBag.ddlRequisitionType = Utility.ListOfRequisitionType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
            ViewBag.ddlExecutionType = new List<SelectListItem>
            {
                new SelectListItem(){Text=RequisitionExecuationType.Single,Value=RequisitionExecuationType.Single },
                new SelectListItem(){Text=RequisitionExecuationType.Bundle,Value=RequisitionExecuationType.Bundle }
            };

            return View();
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequsition(VmReqInfo model)
        {
            bool isSuccess = false;
            var privilege = UserPrivilege("Production", "CreateRequsition");
            var permission = (model.ReqInfoId == 0 && privilege.Add) || (model.ReqInfoId > 0 && privilege.Edit);
            if (ModelState.IsValid && model.ReqDetails.Count > 0 && permission)
            {
                try
                {
                    ReqInfoDTO dto = new ReqInfoDTO();
                    AutoMapper.Mapper.Map(model, dto);
                    if (model.ReqInfoId == 0)
                    {
                        isSuccess = _requsitionInfoBusiness.SaveRequisition(dto, User.UserId, User.OrgId);
                    }
                    else
                    {
                        isSuccess = _requsitionDetailBusiness.SaveRequisitionDetail(dto, User.UserId, User.OrgId);
                    }

                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        // Used By  GetReqInfoList ActionMethod
        public ActionResult GetReqInfoParitalList(string reqCode, long? warehouseId, string status, long? modelId, long? line, string fromDate, string toDate, string requisitionType)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReqInfoList");
            var descriptionData = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);
            IEnumerable<RequsitionInfoDTO> requsitionInfoDTO = _requsitionInfoBusiness.GetAllReqInfoByOrgId(User.OrgId).Where(req =>
                //req.StateStatus != RequisitionStatus.Pending &&
                (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode)) &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId) &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim()) &&
                (requisitionType == null || requisitionType.Trim() == "" || req.RequisitionType == requisitionType.Trim()) &&
                (line == null || line <= 0 || req.LineId == line) &&
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
                RequisitionType = info.RequisitionType
            }).OrderByDescending(s => s.ReqInfoId).ToList();
            List<RequsitionInfoViewModel> requsitionInfoViewModels = new List<RequsitionInfoViewModel>();
            AutoMapper.Mapper.Map(requsitionInfoDTO, requsitionInfoViewModels);
            return PartialView(requsitionInfoViewModels);
        }
        public ActionResult GetRequsitionDetails(long? reqId)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReqInfoList");
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

            return PartialView("_GetRequsitionDetails", requsitionDetailViewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            var privilege = UserPrivilege("Production", "GetReqInfoList");
            var permission = (reqId > 0 && privilege != null && privilege.Edit);
            if (permission)
            {
                if (reqId > 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
                {
                    IsSuccess = _productionStockDetailBusiness.SaveProductionStockInByProductionRequistion(reqId, status, User.OrgId, User.UserId);
                }
                else
                {
                    IsSuccess = _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, User.OrgId);
                }
            }
            return Json(IsSuccess);
        }
        public ActionResult GetRequsitionDetailsEdit(long reqId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId);
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId);
            var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId);

            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(User.OrgId).Where(r => r.ReqInfoId == reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ReqInfoId = d.ReqInfoId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, User.OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, User.OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, User.OrgId).UnitSymbol)
            }).ToList();
            var info = _requsitionInfoBusiness.GetRequisitionById(reqId, User.OrgId);
            RequsitionInfoViewModel requsitionInfoView = new RequsitionInfoViewModel()
            {
                ReqInfoId = info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, User.OrgId).LineNumber,
                LineId = info.LineId,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName,
                WarehouseId = info.WarehouseId,
                ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, User.OrgId).DescriptionName,
                DescriptionId = info.DescriptionId,
                RequisitionType = info.RequisitionType
            };

            ViewBag.RequsitionInfoViewModel = requsitionInfoView;

            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);
            return PartialView("GetRequsitionDetailsEdit", requsitionDetailViewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetBundleItems(long modelId, long itemId)
        {
            var info = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItem(modelId, itemId, User.OrgId);
            List<ItemPreparationDetailViewModel> details = new List<ItemPreparationDetailViewModel>();
            if(info != null)
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).ToList();
                var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).ToList();
                var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();
                var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId).ToList();
                var mobileModels = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).ToList();

                details = _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoId(info.PreparationInfoId, User.OrgId).Select(i => new ItemPreparationDetailViewModel
                {
                    WarehouseId = i.WarehouseId,
                    WarehouseName = warehouses.FirstOrDefault(w => w.Id == i.WarehouseId).WarehouseName,
                    ItemTypeId = i.ItemTypeId,
                    ItemTypeName = itemTypes.FirstOrDefault(it => it.ItemId == i.ItemTypeId).ItemName,
                    ItemId = i.ItemId,
                    ItemName = items.FirstOrDefault(it => it.ItemId == i.ItemId).ItemName,
                    UnitName = units.FirstOrDefault(u => u.UnitId == i.UnitId).UnitSymbol,
                    Quantity = i.Quantity,
                    Remarks = i.Remarks
                }).ToList();
            }
            return Json(details);
        }
        #endregion

        #region Production -Stock
        [HttpGet]
        public ActionResult GetProductionStockInfoList()
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetProductionStockInfoList");
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetProductionStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId, long? LineId, long? ModelId, string lessOrEq)
        {
            IEnumerable<ProductionStockInfoDTO> productionStockInfoDTO = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(User.OrgId).Select(info => new ProductionStockInfoDTO
            {
                StockInfoId = info.ProductionStockInfoId,
                LineId = info.LineId.Value,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, User.OrgId).LineNumber,
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
                DescriptionId = info.DescriptionId,
                ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, info.OrganizationId).DescriptionName)
            }).AsEnumerable();

            productionStockInfoDTO = productionStockInfoDTO.Where(ws =>
            (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId)
            && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId)
            && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)
            && (LineId == null || LineId == 0 || ws.LineId == LineId)
            && (ModelId == null || ModelId == 0 || ws.DescriptionId == ModelId)
            && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
            ).OrderByDescending(s => s.StockInfoId).ToList();

            List<ProductionStockInfoViewModel> productionStockInfoViews = new List<ProductionStockInfoViewModel>();
            AutoMapper.Mapper.Map(productionStockInfoDTO, productionStockInfoViews);
            return PartialView("_productionStockInfoList", productionStockInfoViews);
        }

        public ActionResult GetProductionStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            if (string.IsNullOrEmpty(flag))
            {
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

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                return View();
            }
            else
            {
                var dto = _productionStockDetailBusiness.GetProductionStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum, User.OrgId).OrderByDescending(s => s.StockDetailId).ToList();
                IEnumerable<ProductionStockDetailInfoListViewModel> viewModels = new List<ProductionStockDetailInfoListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetProductionStockDetailInfoList", viewModels);
            }
        }

        #endregion

        #region Item Return
        public ActionResult GetItemReturnList(string flag, string code, long? lineId, long? warehouseId, string status, string returnType, string faultyCase, string fromDate, string toDate, long? modelId)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetItemReturnList");
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

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).ToList();
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).ToList();
                var descriptions = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).ToList();
                IEnumerable<ItemReturnInfoDTO> itemReturnInfoDTOs = _itemReturnInfoBusiness.GetItemReturnInfos(User.OrgId).Where(i => 1 == 1
                && (code == null || code.Trim() == "" || i.IRCode.Contains(code))
                && (lineId == null || lineId <= 0 || i.LineId == lineId)
                && (warehouseId == null || warehouseId <= 0 || i.WarehouseId == warehouseId)
                && (modelId == null || modelId <= 0 || i.DescriptionId == modelId)
                && (status == null || status.Trim() == "" || i.StateStatus == status.Trim())
                && (returnType == null || returnType.Trim() == "" || i.ReturnType == returnType.Trim())
                && (faultyCase == null || faultyCase.Trim() == "" || i.FaultyCase == faultyCase.Trim())
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
                    DescriptionId = i.DescriptionId,
                    Model = descriptions.FirstOrDefault(d => d.DescriptionId == i.DescriptionId).DescriptionName

                }).OrderByDescending(s => s.IRInfoId).ToList();

                List<ItemReturnInfoViewModel> itemReturnInfoViewModels = new List<ItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(itemReturnInfoDTOs, itemReturnInfoViewModels);
                return PartialView("_GetItemReturnList", itemReturnInfoViewModels);
            }
        }

        [HttpGet]
        public ActionResult CreateFaultyGoodReturn()
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

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFaultyItemOrGoodsReturn(ItemReturnInfoViewModel info, List<ItemReturnDetailViewModel> details)
        {
            bool IsSuccess = false;
            var privilege = UserPrivilege("Production", "GetItemReturnList");
            var permission = (info.IRInfoId == 0 && privilege.Add) || (info.IRInfoId > 0 && privilege.Edit);
            if (ModelState.IsValid && details.Count > 0 && permission)
            {
                var dtoInfo = new ItemReturnInfoDTO();
                AutoMapper.Mapper.Map(info, dtoInfo);
                dtoInfo.OrganizationId = User.OrgId;
                dtoInfo.EUserId = User.UserId;
                var dtoDetail = new List<ItemReturnDetailDTO>();
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _itemReturnInfoBusiness.SaveFaultyItemOrGoodsReturn(dtoInfo, dtoDetail);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetProductionItemReturnDetails(long itemReturnInfoId)
        {
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
            }).OrderByDescending(s => s.IRDetailId).ToList();

            var info = _itemReturnInfoBusiness.GetItemReturnInfo(User.OrgId, itemReturnInfoId);
            ItemReturnInfoViewModel itemReturnInfoViewModel = new ItemReturnInfoViewModel()
            {
                IRCode = info.IRCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, User.OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName
            };

            ViewBag.ReturnInfoViewModel = itemReturnInfoViewModel;

            IEnumerable<ItemReturnDetailViewModel> itemReturnDetailViews = new List<ItemReturnDetailViewModel>();
            AutoMapper.Mapper.Map(itemReturnDetailDTOs, itemReturnDetailViews);
            return PartialView("_GetProductionItemReturnDetails", itemReturnDetailViews);
        }

        public ActionResult GetProductionFaultyOrReturnItemDetailList(string flag, string refNum, string returnType, string faultyCase, long? lineId, long? warehouseId, string status, long? itemTypeId, long? itemId, string fromDate, string toDate, long? modelId)
        {
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

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                IEnumerable<ItemReturnDetailListDTO> listdto = _itemReturnDetailBusiness.GetItemReturnDetailList(refNum, returnType, faultyCase, lineId, warehouseId, status, itemTypeId, itemId, fromDate, toDate, modelId, User.OrgId).OrderByDescending(s => s.IRDetailId).ToList();
                IEnumerable<ItemReturnDetailListViewModel> listViewModels = new List<ItemReturnDetailListViewModel>();
                AutoMapper.Mapper.Map(listdto, listViewModels);
                return PartialView("_GetProductionFaultyOrReturnItemDetailList", listViewModels);
            }
        }
        #endregion

        #region Finish Goods Stock

        [HttpGet]
        public ActionResult CreateFinishGoods()
        {
            ViewBag.ddlProductionLines = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
            {
                Text = l.LineNumber,
                Value = l.LineId.ToString()
            }).ToList();

            ViewBag.ddlModel = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem
            {
                Text = des.text,
                Value = des.value
            }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlProductionTypes = Utility.ListOfProductionType().Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinishGoods(FinishGoodsInfoViewModel info, List<FinishGoodsRowMaterialViewModel> details)
        {
            bool IsSucess = false;
            var privilege = UserPrivilege("Production", "GetFinishGoodsList");
            var permission = (info.FinishGoodsInfoId == 0 && privilege.Add) || (info.FinishGoodsInfoId > 0 && privilege.Edit);
            if (ModelState.IsValid && details.Count() > 0 && permission)
            {
                FinishGoodsInfoDTO finishGoodsInfoDTO = new FinishGoodsInfoDTO();
                List<FinishGoodsRowMaterialDTO> finishGoodsRowMaterialDTOs = new List<FinishGoodsRowMaterialDTO>();
                AutoMapper.Mapper.Map(info, finishGoodsInfoDTO);
                AutoMapper.Mapper.Map(details, finishGoodsRowMaterialDTOs);
                IsSucess = _finishGoodsInfoBusiness.SaveFinishGoods(finishGoodsInfoDTO, finishGoodsRowMaterialDTOs, User.UserId, User.OrgId);
            }
            return Json(IsSucess);
        }

        public ActionResult GetFinishGoodsList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string finishQty, string fromDate, string toDate)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetFinishGoodsList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

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
                IEnumerable<FinishGoodsInfoDTO> finishGoodsInfoDTOs = _finishGoodsInfoBusiness.GetFinishGoodsByOrg(User.OrgId).Select(info => new FinishGoodsInfoDTO
                {
                    FinishGoodsInfoId = info.FinishGoodsInfoId,
                    ProductionLineId = info.ProductionLineId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.ProductionLineId, User.OrgId).LineNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId, User.OrgId).ItemName),
                    ItemId = info.ItemId,
                    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId, User.OrgId).ItemName),
                    UnitId = info.UnitId,
                    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId, User.OrgId).UnitSymbol),
                    Quanity = info.Quanity,
                    OrganizationId = info.OrganizationId,
                    DescriptionId = info.DescriptionId,
                    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, info.OrganizationId).DescriptionName),
                    EntryDate = info.EntryDate
                }).AsEnumerable();

                finishGoodsInfoDTOs = finishGoodsInfoDTOs.Where(fg =>
               (warehouseId == null || warehouseId == 0 || fg.WarehouseId == warehouseId)
               && (itemTypeId == null || itemTypeId == 0 || fg.ItemTypeId == itemTypeId)
               && (itemId == null || itemId == 0 || fg.ItemId == itemId)
               && (lineId == null || lineId == 0 || fg.ProductionLineId == lineId)
               && (modelId == null || modelId == 0 || fg.DescriptionId == modelId)
               && (string.IsNullOrEmpty(finishQty) || fg.Quanity <= Convert.ToInt32(finishQty))
               ).OrderByDescending(s => s.FinishGoodsInfoId).ToList();

                IEnumerable<FinishGoodsInfoViewModel> finishGoodsInfoViewModels = new List<FinishGoodsInfoViewModel>();
                AutoMapper.Mapper.Map(finishGoodsInfoDTOs, finishGoodsInfoViewModels);
                return PartialView("_GetFinishGoodsList", finishGoodsInfoViewModels);
            }
        }

        public ActionResult GetFinishGoodsMaterialDetails(long finishGoodsInfoId)
        {
            IEnumerable<FinishGoodsRowMaterialViewModel> viewModels = new List<FinishGoodsRowMaterialViewModel>();
            if (finishGoodsInfoId > 0)
            {
                IEnumerable<FinishGoodsRowMaterialDTO> finishGoodsRowMaterialDTO = _finishGoodsRowMaterialBusiness.GetGoodsRowMaterialsByOrgAndFinishInfoId(User.OrgId, finishGoodsInfoId).Select(r => new FinishGoodsRowMaterialDTO
                {
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(r.WarehouseId, User.OrgId).WarehouseName,
                    ItemTypeName = _itemTypeBusiness.GetItemType(r.ItemTypeId, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(r.ItemId, User.OrgId).ItemName,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(r.UnitId, User.OrgId).UnitSymbol,
                    Quanity = r.Quanity
                }).ToList();
                AutoMapper.Mapper.Map(finishGoodsRowMaterialDTO, viewModels);
            }
            return PartialView("_GetFinishGoodsMaterialDetails", viewModels);
        }

        public ActionResult GetFinishGoodsStockInfo(string flag, long? WarehouseId, long? ItemTypeId, long? ItemId, long? LineId, long? ModelId, string lessOrEq)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetFinishGoodsStockInfo");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

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
                IEnumerable<FinishGoodsStockInfoDTO> finishGoodsStockInfoDTOs = _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByOrgId(User.OrgId).Select(info => new FinishGoodsStockInfoDTO
                {
                    FinishGoodsStockInfoId = info.FinishGoodsStockInfoId,
                    LineId = info.LineId.Value,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, User.OrgId).LineNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                    ItemId = info.ItemId,
                    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                    UnitId = info.UnitId,
                    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
                    StockInQty = info.StockInQty,
                    StockOutQty = info.StockOutQty,
                    Remarks = info.Remarks,
                    OrganizationId = info.OrganizationId,
                    DescriptionId = info.DescriptionId,
                    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, info.OrganizationId).DescriptionName)
                }).AsEnumerable();

                finishGoodsStockInfoDTOs = finishGoodsStockInfoDTOs.Where(fs =>
                (WarehouseId == null || WarehouseId == 0 || fs.WarehouseId == WarehouseId)
                && (ItemTypeId == null || ItemTypeId == 0 || fs.ItemTypeId == ItemTypeId)
                && (ItemId == null || ItemId == 0 || fs.ItemId == ItemId)
                && (LineId == null || LineId == 0 || fs.LineId == LineId)
                && (ModelId == null || ModelId == 0 || fs.DescriptionId == ModelId)
                && (string.IsNullOrEmpty(lessOrEq) || (fs.StockInQty - fs.StockOutQty) <= Convert.ToInt32(lessOrEq))
                ).ToList();

                List<FinishGoodsStockInfoViewModel> finishGoodsStockInfoViewModels = new List<FinishGoodsStockInfoViewModel>();
                AutoMapper.Mapper.Map(finishGoodsStockInfoDTOs, finishGoodsStockInfoViewModels);
                return PartialView("_GetFinishGoodsStockInfo", finishGoodsStockInfoViewModels);
            }
        }

        public ActionResult GetFinishGoodsStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            if (string.IsNullOrEmpty(flag))
            {
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

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                return View();
            }
            else
            {
                var dto = _finishGoodsStockDetailBusiness.GetFinishGoodsStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum).OrderByDescending(s => s.StockDetailId).ToList();
                IEnumerable<FinishGoodsStockDetailInfoListViewModel> viewModels = new List<FinishGoodsStockDetailInfoListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFinishGoodsStockDetailInfoList", viewModels);
            }
        }
        #endregion

        #region Finish Goods Send To Warehouse
        public ActionResult GetFinishGoodsSendToWarehouse(string flag, long? lineId, long? warehouseId, long? modelId, string status, string fromDate, string toDate, string refNo)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetFinishGoodsSendToWarehouse");
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

                var tblwarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId);
                var tblLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId);
                var tblModel = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);

                List<FinishGoodsSendToWarehouseInfoDTO> listOfFinishGoodsSendInfo = _finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseList(User.OrgId)
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
                         RefferenceNumber = f.RefferenceNumber
                     }).OrderByDescending(s => s.SendId).ToList();
                List<FinishGoodsSendToWarehouseInfoViewModel> listOfFinishGoodsSendToWarehouseInfoViewModels = new List<FinishGoodsSendToWarehouseInfoViewModel>();
                AutoMapper.Mapper.Map(listOfFinishGoodsSendInfo, listOfFinishGoodsSendToWarehouseInfoViewModels);
                return PartialView("_GetFinishGoodsSendToWarehouse", listOfFinishGoodsSendToWarehouseInfoViewModels);
            }
        }

        [HttpGet]
        public ActionResult CreateFinishGoodsToWarehouse()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(w => w.WarehouseName == "Warehouse 2" || w.WarehouseName == "Warehouse 3").Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinishGoodsSendToWarehouse(FinishGoodsSendToWarehouseInfoViewModel info, List<FinishGoodsSendToWarehouseDetailViewModel> detail)
        {
            bool IsSucess = false;
            var privilege = UserPrivilege("Production", "GetFinishGoodsSendToWarehouse");
            var permission = (info.SendId == 0 && privilege.Add) || (info.SendId > 0 && privilege.Edit);

            if (ModelState.IsValid && detail.Count() > 0)
            {
                FinishGoodsSendToWarehouseInfoDTO dtoInfo = new FinishGoodsSendToWarehouseInfoDTO();
                List<FinishGoodsSendToWarehouseDetailDTO> dtoDetail = new List<FinishGoodsSendToWarehouseDetailDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(detail, dtoDetail);
                IsSucess = _finishGoodsSendToWarehouseInfoBusiness.SaveFinishGoodsSendToWarehouse(dtoInfo, dtoDetail, User.UserId, User.OrgId);
            }
            return Json(IsSucess);
        }

        public ActionResult GetFinishGoodsSendItemDetail(long sendId)
        {
            List<FinishGoodsSendToWarehouseDetailViewModel> viewModels = new List<FinishGoodsSendToWarehouseDetailViewModel>();
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
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, User.OrgId).DescriptionName
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
                }).ToList();

                AutoMapper.Mapper.Map(dtos, viewModels);
            }
            return PartialView("_GetFinishGoodsSendItemDetail", viewModels);
        }

        public ActionResult GetFinishGoodsSendItemDetailList(string flag, long? lineId, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string status, string refNum, string fromDate, string toDate)
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
                IEnumerable<FinishGoodsSendDetailListDTO> dto = _finishGoodsSendToWarehouseDetailBusiness.GetGoodsSendDetailList(lineId, warehouseId, modelId, itemTypeId, itemId, status, refNum, User.OrgId, fromDate, toDate).OrderByDescending(s => s.SendDetailId).ToList();
                IEnumerable<FinishGoodsSendDetailListViewModel> viewModels = new List<FinishGoodsSendDetailListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFinishGoodsSendItemDetailList", viewModels);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}