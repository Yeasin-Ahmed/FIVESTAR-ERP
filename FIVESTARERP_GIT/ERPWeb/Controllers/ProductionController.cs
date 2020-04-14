using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
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

        private readonly long UserId = 1;
        private readonly long OrgId = 1;
        public ProductionController(IRequsitionInfoBusiness requsitionInfoBusiness, IWarehouseBusiness warehouseBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionLineBusiness productionLineBusiness, IItemBusiness itemBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IProductionStockDetailBusiness productionStockDetailBusiness, IProductionStockInfoBusiness productionStockInfoBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IDescriptionBusiness descriptionBusiness , IFinishGoodsInfoBusiness finishGoodsInfoBusiness, IFinishGoodsRowMaterialBusiness finishGoodsRowMaterialBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness)
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
        }

        #region ProductionLine
        public ActionResult GetProductionLineList(int? page)
        {
            IPagedList<ProductionLineViewModel> productionLineViewModels = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new ProductionLineViewModel
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
            // List<ProductionLineViewModel> productionLineViewModels = new List<ProductionLineViewModel>();
            // AutoMapper.Mapper.Map(productionLineDTO, productionLineViewModels);
            return View(productionLineViewModels);
        }
        [HttpPost]
        public ActionResult SaveProductionLine(ProductionLineViewModel productionLineViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ProductionLineDTO dto = new ProductionLineDTO();
                    AutoMapper.Mapper.Map(productionLineViewModel, dto);
                    isSuccess = _productionLineBusiness.SaveUnit(dto, UserId, OrgId);
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
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value != RequisitionStatus.Pending).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
            return View();
        }
        public ActionResult CreateRequsition()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequsition(VmReqInfo model)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && model.ReqDetails.Count > 0)
            {
                try
                {
                    ReqInfoDTO dto = new ReqInfoDTO();
                    AutoMapper.Mapper.Map(model, dto);
                    if (model.ReqInfoId == 0) {
                        isSuccess = _requsitionInfoBusiness.SaveRequisition(dto, UserId, OrgId);
                    }
                    else
                    {
                        isSuccess = _requsitionDetailBusiness.SaveRequisitionDetail(dto, UserId, OrgId);
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
        public ActionResult GetReqInfoParitalList(string reqCode, long? warehouseId, string status, long? modelId, long? line, string fromDate, string toDate)
        {
            var descriptionData = _descriptionBusiness.GetDescriptionByOrgId(OrgId);
            IEnumerable<RequsitionInfoDTO> requsitionInfoDTO = _requsitionInfoBusiness.GetAllReqInfoByOrgId(OrgId).Where(req =>
                //req.StateStatus != RequisitionStatus.Pending &&
                (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode)) &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId) &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim()) &&
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
            if (reqId > 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
            {
                IsSuccess = _productionStockDetailBusiness.SaveProductionStockInByProductionRequistion(reqId, status, OrgId, UserId);
            }
            else
            {
                IsSuccess = _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, OrgId);
            }
            return Json(IsSuccess);
        }
        public ActionResult GetRequsitionDetailsEdit(long reqId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(OrgId);
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId);
            var units = _unitBusiness.GetAllUnitByOrgId(OrgId);
            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(OrgId).Where(r=>r.ReqInfoId==reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ReqInfoId =d.ReqInfoId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, OrgId).UnitSymbol)
            }).ToList();

            var info = _requsitionInfoBusiness.GetRequisitionById(reqId,OrgId);
            RequsitionInfoViewModel requsitionInfoView = new RequsitionInfoViewModel()
            {
                ReqInfoId=info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, OrgId).LineNumber,
                LineId = info.LineId,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, OrgId).WarehouseName,
                WarehouseId = info.WarehouseId,
                ModelName=_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId,OrgId).DescriptionName,
                DescriptionId = info.DescriptionId
            };

            ViewBag.RequsitionInfoViewModel = requsitionInfoView;

            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);
            return PartialView("GetRequsitionDetailsEdit", requsitionDetailViewModels);
        }

        #endregion

        #region Production -Stock
        [HttpGet]
        public ActionResult GetProductionStockInfoList()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetProductionStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId, long? LineId, long? ModelId, string lessOrEq)
        {
            IEnumerable<ProductionStockInfoDTO> productionStockInfoDTO = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(OrgId).Select(info => new ProductionStockInfoDTO
            {
                StockInfoId = info.ProductionStockInfoId,
                LineId = info.LineId.Value,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, OrgId).LineNumber,
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
            ).ToList();

            List<ProductionStockInfoViewModel> productionStockInfoViews = new List<ProductionStockInfoViewModel>();
            AutoMapper.Mapper.Map(productionStockInfoDTO, productionStockInfoViews);
            return PartialView("_productionStockInfoList", productionStockInfoViews);
        }

        public ActionResult GetProductionStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            if (string.IsNullOrEmpty(flag))
            {
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

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                return View();
            }
            else
            {
                var dto = _productionStockDetailBusiness.GetProductionStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum);
                IEnumerable<ProductionStockDetailInfoListViewModel> viewModels = new List<ProductionStockDetailInfoListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetProductionStockDetailInfoList", viewModels);
            }
        }

        #endregion

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

        #region Item Return
        public ActionResult GetItemReturnList(string flag, string code, long? lineId, long? warehouseId, string status, string returnType, string faultyCase, string fromDate, string toDate, long? modelId)
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

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).ToList();
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).ToList();
                var descriptions = _descriptionBusiness.GetDescriptionByOrgId(OrgId).ToList();
                IEnumerable<ItemReturnInfoDTO> itemReturnInfoDTOs = _itemReturnInfoBusiness.GetItemReturnInfos(OrgId).Where(i => 1 == 1
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
                    Qty = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(OrgId, i.IRInfoId).Count(),
                    StateStatus = i.StateStatus,
                    EntryDate = i.EntryDate,
                    DescriptionId = i.DescriptionId,
                    Model = descriptions.FirstOrDefault(d => d.DescriptionId == i.DescriptionId).DescriptionName

                }).ToList();

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

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFaultyItemOrGoodsReturn(ItemReturnInfoViewModel info, List<ItemReturnDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count > 0)
            {
                var dtoInfo = new ItemReturnInfoDTO();
                AutoMapper.Mapper.Map(info, dtoInfo);
                dtoInfo.OrganizationId = OrgId;
                dtoInfo.EUserId = UserId;
                var dtoDetail = new List<ItemReturnDetailDTO>();
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _itemReturnInfoBusiness.SaveFaultyItemOrGoodsReturn(dtoInfo, dtoDetail);
            }
            return Json(IsSuccess);
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

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                IEnumerable<ItemReturnDetailListDTO> listdto = _itemReturnDetailBusiness.GetItemReturnDetailList(refNum, returnType, faultyCase, lineId, warehouseId, status, itemTypeId, itemId, fromDate, toDate, modelId);
                IEnumerable<ItemReturnDetailListViewModel> listViewModels = new List<ItemReturnDetailListViewModel>();
                AutoMapper.Mapper.Map(listdto, listViewModels);
                return PartialView("_GetProductionFaultyOrReturnItemDetailList", listViewModels);
            }
        }
        #endregion

        #region Finish Goods

        [HttpGet]
        public ActionResult CreateFinishGoods()
        {
            ViewBag.ddlProductionLines = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(l => new SelectListItem
            {
                Text = l.LineNumber,
                Value = l.LineId.ToString()
            }).ToList();

            ViewBag.ddlModel = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem
            {
                Text = des.text,
                Value = des.value
            }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
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

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinishGoods(FinishGoodsInfoViewModel info, List<FinishGoodsRowMaterialViewModel> details)
        {
            bool IsSucess = false;
            if (ModelState.IsValid && details.Count() > 0)
            {
                FinishGoodsInfoDTO finishGoodsInfoDTO = new FinishGoodsInfoDTO();
                List<FinishGoodsRowMaterialDTO> finishGoodsRowMaterialDTOs = new List<FinishGoodsRowMaterialDTO>();
                AutoMapper.Mapper.Map(info, finishGoodsInfoDTO);
                AutoMapper.Mapper.Map(details, finishGoodsRowMaterialDTOs);
                IsSucess= _finishGoodsInfoBusiness.SaveFinishGoods(finishGoodsInfoDTO, finishGoodsRowMaterialDTOs, UserId, OrgId);
            }
            return Json(IsSucess);
        }

        public ActionResult GetFinishGoodsList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string finishQty, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

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
                IEnumerable<FinishGoodsInfoDTO> finishGoodsInfoDTOs = _finishGoodsInfoBusiness.GetFinishGoodsByOrg(OrgId).Select(info => new FinishGoodsInfoDTO
                {
                    FinishGoodsInfoId = info.FinishGoodsInfoId,
                    ProductionLineId = info.ProductionLineId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.ProductionLineId, OrgId).LineNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId, OrgId).ItemName),
                    ItemId = info.ItemId,
                    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId, OrgId).ItemName),
                    UnitId = info.UnitId,
                    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId, OrgId).UnitSymbol),
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
               ).ToList();

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
                IEnumerable<FinishGoodsRowMaterialDTO> finishGoodsRowMaterialDTO = _finishGoodsRowMaterialBusiness.GetGoodsRowMaterialsByOrgAndFinishInfoId(OrgId, finishGoodsInfoId).Select(r => new FinishGoodsRowMaterialDTO
                {
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(r.WarehouseId,OrgId).WarehouseName,
                    ItemTypeName = _itemTypeBusiness.GetItemType(r.ItemTypeId,OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(r.ItemId,OrgId).ItemName,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(r.UnitId,OrgId).UnitSymbol,
                    Quanity = r.Quanity
                }).ToList();
                AutoMapper.Mapper.Map(finishGoodsRowMaterialDTO, viewModels);
            }
            return PartialView("_GetFinishGoodsMaterialDetails", viewModels);
        }

        public ActionResult GetFinishGoodsStockInfo(string flag,long? WarehouseId, long? ItemTypeId, long? ItemId, long? LineId, long? ModelId, string lessOrEq)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

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
                IEnumerable<FinishGoodsStockInfoDTO> finishGoodsStockInfoDTOs = _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByOrgId(OrgId).Select(info => new FinishGoodsStockInfoDTO
                {
                    FinishGoodsStockInfoId = info.FinishGoodsStockInfoId,
                    LineId = info.LineId.Value,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, OrgId).LineNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, OrgId).ItemName),
                    ItemId = info.ItemId,
                    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, OrgId).ItemName),
                    UnitId = info.UnitId,
                    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, OrgId).UnitSymbol),
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

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                return View();
            }
            else
            {
                var dto = _finishGoodsStockDetailBusiness.GetFinishGoodsStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum);
                IEnumerable<FinishGoodsStockDetailInfoListViewModel> viewModels = new List<FinishGoodsStockDetailInfoListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFinishGoodsStockDetailInfoList", viewModels);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}