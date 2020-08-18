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
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using ERPDAL.InventoryDAL;
using System.Configuration;
using System.Data.OleDb;
using LinqToExcel;
using System.Data.Entity.Validation;

namespace ERPWeb.Controllers
{
    
    [CustomAuthorize]
    public class InventoryController : BaseController
    {
        // GET: Inventory
        #region Inventory
        private readonly IInventoryUnitOfWork _inventoryDb;
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
        private readonly ISupplierBusiness _supplierBusiness;
        private readonly IIQCBusiness _iQCBusiness;
        private readonly IIQCItemReqDetailList _iQCItemReqDetailList;
        private readonly IIQCItemReqInfoList _iQCItemReqInfoList;
        private readonly IIQCStockDetailBusiness _iQCStockDetailBusiness;
        private readonly IIQCStockInfoBusiness _iQCStockInfoBusiness;
        private readonly WarehouseStockInfoRepository warehouseStockInfoRepository;
        private readonly WarehouseStockDetailRepository warehouseStockDetailRepository;
        #endregion

        #region Production
        private readonly IRepairLineBusiness _repairLineBusiness;
        private readonly IRepairSectionRequisitionInfoBusiness _repairSectionRequisitionInfoBusiness;
        private readonly IRepairSectionRequisitionDetailBusiness _repairSectionRequisitionDetailBusiness;
        #endregion


        public InventoryController(IWarehouseBusiness warehouseBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, IProductionLineBusiness productionLineBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IRepairStockInfoBusiness repairStockInfoBusiness, IRepairStockDetailBusiness repairStockDetailBusiness, IDescriptionBusiness descriptionBusiness, IFinishGoodsSendToWarehouseInfoBusiness finishGoodsSendToWarehouseInfoBusiness, IFinishGoodsSendToWarehouseDetailBusiness finishGoodsSendToWarehouseDetailBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, ISupplierBusiness supplierBusiness, IRepairSectionRequisitionInfoBusiness repairSectionRequisitionInfoBusiness, IRepairLineBusiness repairLineBusiness, IRepairSectionRequisitionDetailBusiness repairSectionRequisitionDetailBusiness, IIQCBusiness iQCBusiness, IIQCItemReqDetailList iQCItemReqDetailList, IIQCItemReqInfoList iQCItemReqInfoList, IIQCStockDetailBusiness iQCStockDetailBusiness, IIQCStockInfoBusiness iQCStockInfoBusiness, IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
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
            this._supplierBusiness = supplierBusiness;
            this._iQCBusiness = iQCBusiness;
            this._iQCItemReqDetailList = iQCItemReqDetailList;
            this._iQCItemReqInfoList = iQCItemReqInfoList;
            this._iQCStockDetailBusiness = iQCStockDetailBusiness;
            this._iQCStockInfoBusiness = iQCStockInfoBusiness;
            warehouseStockInfoRepository = new WarehouseStockInfoRepository(this._inventoryDb);
            warehouseStockDetailRepository =new WarehouseStockDetailRepository(this._inventoryDb);
            #region Production
            this._repairSectionRequisitionInfoBusiness = repairSectionRequisitionInfoBusiness;
            this._repairLineBusiness = repairLineBusiness;
            this._repairSectionRequisitionDetailBusiness = repairSectionRequisitionDetailBusiness;
            #endregion

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
            ViewBag.ddlItemTypeName = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).Select(itemtype => new SelectListItem { Text = itemtype.ItemName, Value = itemtype.ItemId.ToString() }).ToList();
            ViewBag.ddlUnitName = _unitBusiness.GetAllUnitByOrgId(User.OrgId).Select(unit => new SelectListItem { Text = unit.UnitName, Value = unit.UnitId.ToString() }).ToList();
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

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
        public ActionResult GetWarehouseStockInfoList(string tab, string flag, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
                ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetWarehouseStockInfoList");
                ViewBag.tab = tab;
            }
            else
            {
                var dto = _warehouseStockInfoBusiness.GetWarehouseStockInfoLists(warehouseId, modelId, itemTypeId, itemId, lessOrEq, User.OrgId).OrderByDescending(s => s.StockInfoId).ToList();
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
                dto = dto.Skip((page - 1) * 15).Take(15).ToList();
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<WarehouseStockInfoViewModel> warehouseStockInfoViews = new List<WarehouseStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, warehouseStockInfoViews);
                return PartialView("_WarehouseStockInfoList", warehouseStockInfoViews);
            }

            return View();
        }
        #region Comment Old GetWarehouseStockInfoPartialList
        //[HttpGet]
        //public ActionResult GetWarehouseStockInfoPartialList(string flag, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        //{
        //    var dto = _warehouseStockInfoBusiness.GetWarehouseStockInfoLists(warehouseId, modelId, itemTypeId, itemId, lessOrEq,  User.OrgId).OrderByDescending(s => s.StockInfoId).ToList();

        //    IEnumerable<WarehouseStockInfoDTO> dto = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(User.OrgId).Select(info => new WarehouseStockInfoDTO
        //    {
        //        StockInfoId = info.StockInfoId,
        //        WarehouseId = info.WarehouseId,
        //        DescriptionId = info.DescriptionId,
        //        Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
        //        ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, User.OrgId).DescriptionName),
        //        ItemTypeId = info.ItemTypeId,
        //        ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
        //        ItemId = info.ItemId,
        //        Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
        //        UnitId = info.UnitId,
        //        Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
        //        StockInQty = info.StockInQty,
        //        StockOutQty = info.StockOutQty,
        //        Remarks = info.Remarks,
        //        OrganizationId = info.OrganizationId
        //    }).AsEnumerable();

        //    dto = dto.Where(ws =>
        //    (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId)
        //    && (modelId == null || modelId == 0 || ws.DescriptionId == modelId)
        //    && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId)
        //    && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)
        //    && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
        //    ).ToList();

        //    // Pagination //
        //    ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
        //    dto = dto.Skip((page - 1) * 15).Take(15).ToList();
        //    //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
        //    //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //    //-----------------//

        //    List<WarehouseStockInfoViewModel> warehouseStockInfoViews = new List<WarehouseStockInfoViewModel>();
        //    AutoMapper.Mapper.Map(dto, warehouseStockInfoViews);
        //    return PartialView("_WarehouseStockInfoList", warehouseStockInfoViews);
        //}
        #endregion

        public ActionResult CreateStock()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlModel = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            ViewBag.ddlSupplier = _supplierBusiness.GetSuppliers(User.OrgId).Select(sup => new SelectListItem { Text = sup.SupplierName, Value = sup.SupplierId.ToString() }).ToList();
            ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();
            ViewBag.ddlItemTypeName = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).Select(itemtype => new SelectListItem { Text = itemtype.ItemName, Value = itemtype.ItemId.ToString() }).ToList();

            ViewBag.ddlUnitName = _unitBusiness.GetAllUnitByOrgId(User.OrgId).Select(unit => new SelectListItem { Text = unit.UnitName, Value = unit.UnitId.ToString() }).ToList();

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

        public ActionResult GetWarehouseStockDetailInfoList(string flag, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, long? supplierId, int page = 1)
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
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
                ViewBag.ddlSupplier = _supplierBusiness.GetSuppliers(User.OrgId).Select(sup => new SelectListItem { Text = sup.SupplierName, Value = sup.SupplierId.ToString() }).ToList();
            }
            else
            {
                var dto = _warehouseStockDetailBusiness.GetWarehouseStockDetailInfoLists(warehouseId, modelId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum, supplierId, User.OrgId).OrderByDescending(s => s.StockDetailId).ToList();

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
        public ActionResult GetReqInfoParitalList(string reqCode, long? warehouseId, string status, long? line, long? modelId, string fromDate, string toDate, string requisitionType, long? reqInfoId, int page = 1)
        {
            var dto = _requsitionInfoBusiness.GetRequsitionInfosByQuery(line, 0, 0, 0, warehouseId, modelId, reqCode, requisitionType, string.Empty, fromDate, toDate, status, string.Empty, reqInfoId, User.OrgId);

            //var descriptionData = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);
            //IEnumerable<RequsitionInfoDTO> dto = _requsitionInfoBusiness.GetAllReqInfoByOrgId(User.OrgId).Where(req =>
            //    (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode))
            //    &&
            //    (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId)
            //    &&
            //    (status == null || status.Trim() == "" || req.StateStatus == status.Trim())
            //    &&
            //    (requisitionType == null || requisitionType.Trim() == "" || req.RequisitionType == requisitionType.Trim()) &&
            //    (line == null || line <= 0 || req.LineId == line)
            //    &&
            //    (modelId == null || modelId <= 0 || req.DescriptionId == modelId) &&
            //    (
            //        (fromDate == null && toDate == null)
            //        ||
            //         (fromDate == "" && toDate == "")
            //        ||
            //        (fromDate.Trim() != "" && toDate.Trim() != "" &&

            //            req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
            //            req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
            //        ||
            //        (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
            //        ||
            //        (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
            //    )
            //).Select(info => new RequsitionInfoDTO
            //{
            //    ReqInfoId = info.ReqInfoId,
            //    ReqInfoCode = info.ReqInfoCode,
            //    LineId = info.LineId,
            //    LineNumber = (_productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, User.OrgId).LineNumber),
            //    StateStatus = info.StateStatus,
            //    Remarks = info.Remarks,
            //    OrganizationId = info.OrganizationId,
            //    EntryDate = info.EntryDate,
            //    WarehouseId = info.WarehouseId,
            //    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName),
            //    ModelName = descriptionData.FirstOrDefault(d => d.DescriptionId == info.DescriptionId).DescriptionName,
            //    Qty = _requsitionDetailBusiness.GetRequsitionDetailByReqId(info.ReqInfoId, User.OrgId).Select(s => s.ItemId).Distinct().Count(),
            //    RequisitionType = info.RequisitionType,
            //    EntryUser = UserForEachRecord(info.EUserId.Value).UserName,
            //    UpdateUser = (info.UpUserId == null || info.UpUserId == 0) ? "" : UserForEachRecord(info.UpUserId.Value).UserName
            //}).OrderByDescending(s => s.ReqInfoId).ToList();

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
                if (RequisitionStatus.Rejected == status || RequisitionStatus.Rechecked == status)
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
            var descriptionId = _requsitionInfoBusiness.GetRequisitionById(reqInfoId.Value, User.OrgId).DescriptionId;
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId.Value, User.OrgId).ToArray();
            var warehouseStock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(User.OrgId).ToList();
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();
            stateWithText.isSuccess = true;

            for (int i = 0; i < reqDetail.Length; i++)
            {
                var w = warehouseStock.FirstOrDefault(wr => wr.ItemId == reqDetail[i].ItemId && wr.DescriptionId == descriptionId);
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
        public ActionResult GetItemReturnList(string flag, string code, long? lineId, long? modelId, long? warehouseId, string status, string returnType, string faultyCase, string fromDate, string toDate, int page = 1)
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

        public ActionResult GetRepairStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, string returnType, string faultyCase, int page = 1)
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
        public ActionResult GetItemPreparation(string flag, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? id, string type, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemPreparation");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlItemPreparationType = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text = ItemPreparationType.Production, Value= ItemPreparationType.Production },
                    new SelectListItem(){ Text = ItemPreparationType.Packaging, Value= ItemPreparationType.Packaging }
                }.ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                ViewBag.ddlItemTgt = _itemBusiness.GetItemDetails(User.OrgId).Where(s => s.ItemName.Contains("Warehouse 3")).Select(d => new SelectListItem { Text = d.ItemName, Value = d.ItemId.ToString() }).ToList();

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
                    (itemId == null || itemId <= 0 || itemId == i.ItemId) &&
                    (string.IsNullOrEmpty(type) || type.Trim() == "" || type == i.PreparationType)
                ).Select(i => new ItemPreparationInfoDTO
                {
                    PreparationInfoId = i.PreparationInfoId,
                    PreparationType = i.PreparationType,
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
                ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
                dto = dto.Skip((page - 1) * 15).Take(15).ToList();
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
                        PreparationType = info.PreparationType,

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
        public ActionResult CreateItemPreparation2(long? id)
        {
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

            var allItemsInDb = _itemBusiness.GetItemDetails(User.OrgId);
            //
            ViewBag.ddlItemTgt = allItemsInDb.Where(s=> s.ItemName.Contains("Warehouse 3")).Select(d => new SelectListItem { Text = d.ItemName, Value = d.ItemId.ToString() }).ToList();

            ViewBag.ddlWarehouseSource = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(s => s.WarehouseName == "Warehouse 1" || s.WarehouseName == "Warehouse 2").Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlItemPreparationType = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = ItemPreparationType.Production,Value= ItemPreparationType.Production },
                new SelectListItem(){ Text = ItemPreparationType.Packaging,Value= ItemPreparationType.Packaging }
            }.ToList();

            return View();
        }

        [HttpGet]
        public ActionResult CreateItemPreparation(long? id)
        {
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

            //_itemBusiness.GetItemDetails

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

            ViewBag.ddlItemPreparationType = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = ItemPreparationType.Production,Value= ItemPreparationType.Production },
                new SelectListItem(){ Text = ItemPreparationType.Packaging,Value= ItemPreparationType.Packaging }
            }.ToList();

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

        #region Supplier
        public ActionResult GetSuppliers()
        {
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetSuppliers");
            var supplierDto = _supplierBusiness.GetSuppliers(User.OrgId).Select(s => new SupplierDTO
            {
                SupplierId = s.SupplierId,
                SupplierName = s.SupplierName,
                Address = s.Address,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                MobileNumber = s.MobileNumber,
                IsActive = s.IsActive,
                Remarks = s.Remarks,
                EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName
            }).ToList();
            IEnumerable<SupplierViewModel> viewModels = new List<SupplierViewModel>();
            AutoMapper.Mapper.Map(supplierDto, viewModels);
            return View(viewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveSupplier(SupplierViewModel model)
        {
            bool IsSuccess = false;
            var privilege = UserPrivilege("Inventory", "GetSuppliers");
            var permission = (model.SupplierId == 0 && privilege.Add) || (model.SupplierId > 0 && privilege.Edit);
            if (ModelState.IsValid && permission)
            {
                SupplierDTO dto = new SupplierDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _supplierBusiness.SaveSupplier(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        #endregion

        #region Repair Section Requisition
        public ActionResult GetRepairSectionRequisitionInfoList(string flag, long? repairLineId, long? modelId, long? warehouseId, string status, string requisitionCode, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();

                ViewBag.ddlRepairLine = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else
            {
                var dto = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionInfoList(repairLineId, modelId, warehouseId, status, requisitionCode, fromDate, toDate, "Warehouse", User.OrgId);
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<RepairSectionRequisitionInfoViewModel> viewModels = new List<RepairSectionRequisitionInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairSectionRequisitionInfoList", viewModels);
            }
        }

        public ActionResult IssueRepairSectionRequisition(long requisitionId)
        {
            var req = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(requisitionId, User.OrgId);
            if (req != null && req.StateStatus == RequisitionStatus.Checked)
            {
                var reqInfo = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionDataById(req.RSRInfoId, User.OrgId);
                RepairSectionRequisitionInfoViewModel viewModel = new RepairSectionRequisitionInfoViewModel();
                AutoMapper.Mapper.Map(reqInfo, viewModel);
                return View(viewModel);
            }
            return RedirectToAction("GetRepairSectionRequisitionInfoList");
        }
        public ActionResult IssueRepairSectionRequisitionDetails(long requisitionId)
        {
            var requisition = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(requisitionId, User.OrgId);
            if (requisitionId > 0 && requisition.StateStatus == RequisitionStatus.Checked)
            {
                var dto = _repairSectionRequisitionDetailBusiness.GetRepairSectionRequisitionDetailPendingByReqId(requisitionId, User.OrgId);
                IEnumerable<RepairSectionRequisitionDetailViewModel> viewModels = new List<RepairSectionRequisitionDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_IssueRepairSectionRequisitionDetails", viewModels);
            }
            return RedirectToAction("GetRepairSectionRequisitionInfoList");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepairSectionRequisitionState(RepairRequisitionInfoStateViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && model.Details.Count > 0)
            {
                if (model.Status == RequisitionStatus.Approved)
                {
                    RepairRequisitionInfoStateDTO dto = new RepairRequisitionInfoStateDTO();
                    AutoMapper.Mapper.Map(model, dto);
                    IsSuccess = _repairSectionRequisitionInfoBusiness.SaveRepairSectionRequisitionIssueByWarehouse(dto, User.OrgId, User.UserId);
                }
                else
                {
                    IsSuccess = _repairSectionRequisitionInfoBusiness.SaveRepairSectionRequisitionStatus(model.RequistionId, model.Status, User.OrgId, User.UserId);
                }
            }
            return Json(IsSuccess);
        }
        #endregion

        #region PartialView

        public ActionResult _GetWarehousePartialList()
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
            return PartialView(viewModel);
        }
        public ActionResult _GetDescriptionPartialList(int? page)
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
            return PartialView(viewModel);
        }

        public ActionResult _GetItemTypePartialList(int? page)
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
            //ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemTypeList");
            return PartialView(viewModels);
        }

        public ActionResult _GetAllUnitPartialList()
        {
            //ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetAllUnitList");
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
            return PartialView(unitViewModels);
        }

        public ActionResult _GetItemPartialList()
        {
            //ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetItemList");
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
            return PartialView(viewModel);
        }
        #endregion

        #region IQC Item
        #region Create IQC
        [HttpGet]
        public ActionResult GetIQCList()
        {
            IEnumerable<IQCDTO> dto = _iQCBusiness.GetAllIQCByOrgId(1).Select(iqc => new IQCDTO
            {
                Id = iqc.Id,
                IQCName = iqc.IQCName,
                Remarks = iqc.Remarks,
                StateStatus = iqc.IsActive == true ? "Active" : "InActive",
                OrganizationId = iqc.OrganizationId,
                EntryUser = UserForEachRecord(iqc.EUserId.Value).UserName,
                UpdateUser = iqc.UpUserId == null || iqc.UpUserId == 0 ? "" : UserForEachRecord(iqc.EUserId.Value).UserName,
            }).ToList();

            List<IQCViewModel> viewModel = new List<IQCViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            //ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetIQCList");

            return PartialView(viewModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveIQC(IQCViewModel viewModel)
        {
            bool isSuccess = false;
            var pre = UserPrivilege("Inventory", "GetIQCList");
            var permission = (viewModel.Id == 0 && pre.Add) || (viewModel.Id > 0 && pre.Edit);
            if (ModelState.IsValid && permission)
            {
                try
                {
                    IQCDTO dto = new IQCDTO();
                    AutoMapper.Mapper.Map(viewModel, dto);
                    isSuccess = _iQCBusiness.SaveIQC(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region CreateIQCItemRequest
        public ActionResult CreateIQCReq()
        {
            ViewBag.ddlIQC = _iQCBusiness.GetAllIQCByOrgId(User.OrgId).Select(iqc => new SelectListItem { Text = iqc.IQCName, Value = iqc.Id.ToString() }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlModel = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            ViewBag.ddlSupplier = _supplierBusiness.GetSuppliers(User.OrgId).Select(sup => new SelectListItem { Text = sup.SupplierName, Value = sup.SupplierId.ToString() }).ToList();
            //ViewBag.UserPrivilege = UserPrivilege("Inventory", "CreateIQCReq");
            return View();
        }

        [HttpGet]
        public ActionResult GetIQCItemReqInfoList(string tab)
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();
            ViewBag.UserPrivilege = UserPrivilege("Inventory", "GetIQCItemReqInfoList");
            ViewBag.tab = tab;
            return View();
        }
        public ActionResult SaveIQCReq(List<IQCItemReqDetailListViewModel> models)
        {
            bool isSuccess = false;
            var pre = UserPrivilege("Inventory", "GetIQCItemReqInfoList");
            var permission = ((pre.Add) || (pre.Edit));
            if (ModelState.IsValid && models.Count > 0 && permission)
            {
                try
                {
                    List<IQCItemReqDetailListDTO> dtos = new List<IQCItemReqDetailListDTO>();
                    AutoMapper.Mapper.Map(models, dtos);
                    isSuccess = _iQCItemReqDetailList.SaveIQCItemReq(dtos, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpGet]
        public ActionResult GetIQCReqInfoList(long? WarehouseId, long? modelId, string stateStatus, int page = 1)
        {
            var dto = _iQCItemReqInfoList.GetIQCItemReqInfoLists(WarehouseId, modelId, stateStatus, User.OrgId).OrderByDescending(s => s.IQCItemReqInfoId).ToList();
            #region Comment Old Code
            // IEnumerable<IQCItemReqInfoListDTO> dto = _iQCItemReqInfoList.GetIQCItemReqInfoListByOrgId(User.OrgId).Select(info => new IQCItemReqInfoListDTO
            // {
            //     IQCId = info.IQCId,
            //     IQCItemReqInfoId = info.IQCItemReqInfoId,
            //     IQCName = (_iQCBusiness.GetIQCOneByOrgId(info.IQCId.Value, User.OrgId).IQCName),
            //     StateStatus = info.StateStatus,
            //     WarehouseId = info.WarehouseId,
            //     DescriptionId = info.DescriptionId,
            //     Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
            //     ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, User.OrgId).DescriptionName),
            //     SupplierId = info.SupplierId,
            //     Supplier = (_supplierBusiness.GetSupplierById(info.SupplierId.Value, User.OrgId).SupplierName),
            //     Remarks = info.Remarks,
            //     OrganizationId = info.OrganizationId
            // }).OrderByDescending(s=> s.IQCItemReqInfoId).ToList();

            // dto = dto.Where(iqc =>
            //(WarehouseId == null || WarehouseId == 0 || iqc.WarehouseId == WarehouseId)
            //&& (modelId == null || modelId == 0 || iqc.DescriptionId == modelId)
            //&& (stateStatus == null || stateStatus == "" || iqc.StateStatus == stateStatus)
            //).OrderByDescending(s => s.IQCItemReqInfoId).ToList();
            #endregion
            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
            dto = dto.Skip((page - 1) * 15).Take(15).ToList();

            List<IQCItemReqInfoListViewModel> iQCItemReqInfoListViewModels = new List<IQCItemReqInfoListViewModel>();
            AutoMapper.Mapper.Map(dto, iQCItemReqInfoListViewModels);
            return PartialView(iQCItemReqInfoListViewModels);
        }

        public ActionResult GetIQCItemReqDetailInfoList(string flag, long? warehouseId, long? modelId, long? unitId, long? itemTypeId, long? itemId, string fromDate, string toDate, string qty, long? reqInfoId, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
                ViewBag.ddlSupplier = _supplierBusiness.GetSuppliers(User.OrgId).Select(sup => new SelectListItem { Text = sup.SupplierName, Value = sup.SupplierId.ToString() }).ToList();
                ViewBag.ddlItemType = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).Select(item => new SelectListItem { Text = item.ItemName, Value = item.ItemId.ToString() });
            }
            else
            {
                var dto = _iQCItemReqDetailList.GetIQCItemReqDetailList(itemTypeId, itemId, unitId, qty, fromDate, toDate, User.OrgId).OrderByDescending(s => s.IQCItemReqInfoId).ToList();
                #region Comment Old Code
                //IEnumerable<IQCItemReqDetailListDTO> dto = _iQCItemReqDetailList.GetIQCItemReqInfoListByOrgId(User.OrgId).Select(info => new IQCItemReqDetailListDTO
                //{
                //    ItemTypeId = info.ItemTypeId,
                //    ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                //    ItemId = info.ItemId,
                //    Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                //    UnitId = info.UnitId,
                //    Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitName),
                //    EUserId = info.EUserId,
                //    EntryDate = info.EntryDate,
                //    Quantity = info.Quantity,
                //    EntryUser = UserForEachRecord(info.EUserId.Value).UserName,
                //    OrganizationId = info.OrganizationId,
                //}).OrderByDescending(s=> s.IQCItemReqDetailId).ToList();

                //dto = dto.Where(iqc =>
                //(itemId == null || itemId == 0 || iqc.ItemId == itemId)
                //&& (itemTypeId == null || itemTypeId == 0 || iqc.ItemTypeId == itemTypeId)
                //&& (qty == null || qty == "" || iqc.Quantity >= Convert.ToDecimal(qty) )
                //&& (fromDate == null || fromDate == "" || iqc.EntryDate >= DateTime.Parse(fromDate))
                //&& (toDate == null || toDate == "" || iqc.EntryDate >= DateTime.Parse(toDate))
                //).OrderByDescending(s => s.IQCItemReqInfoId).ToList();
                #endregion
                List<IQCItemReqDetailListViewModel> viewModel = new List<IQCItemReqDetailListViewModel>();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
                dto = dto.Skip((page - 1) * 15).Take(15).ToList();

                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetAllIQCItemReqPartialList", viewModel);
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetIQCItemReqDetailList(long? reqInfoId, int page = 1)
        {
            IQCItemReqInfoListDTO dto = _iQCItemReqInfoList.GetIQCItemReqDataById(reqInfoId.Value, User.OrgId);
            dto.IQCItemReqDetails = _iQCItemReqDetailList.GetIQCItemReqDetails(reqInfoId.Value, User.OrgId).ToList();
            #region Comment Old Code
            //IQCItemReqInfoListDTO dto = _iQCItemReqInfoList.GetIQCItemReqInfoListByOrgId(User.OrgId).Where(s => s.IQCItemReqInfoId == reqInfoId).Select(info => new IQCItemReqInfoListDTO
            //{
            //    IQCItemReqInfoId = info.IQCItemReqInfoId,
            //    IQCId = info.IQCId,
            //    IQCReqCode = info.IQCReqCode,
            //    IQCName = (_iQCBusiness.GetIQCOneByOrgId(info.IQCId.Value, User.OrgId).IQCName),
            //    StateStatus = info.StateStatus,
            //    WarehouseId = info.WarehouseId,
            //    DescriptionId = info.DescriptionId,
            //    Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
            //    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, User.OrgId).DescriptionName),
            //    SupplierId = info.SupplierId,
            //    Supplier = (_supplierBusiness.GetSupplierById(info.SupplierId.Value, User.OrgId).SupplierName),
            //    Remarks = info.Remarks,
            //    OrganizationId = info.OrganizationId,
            //    EntryDate = info.EntryDate,
            //    EntryUser = UserForEachRecord(info.EUserId.Value).UserName,
            //}).FirstOrDefault();

            //dto.IQCItemReqDetails = _iQCItemReqDetailList.GetIQCItemReqInfoListByInfo(reqInfoId.Value, User.OrgId).Select(info => new IQCItemReqDetailListDTO
            //{
            //    ItemTypeId = info.ItemTypeId,
            //    ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
            //    ItemId = info.ItemId,
            //    Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
            //    UnitId = info.UnitId,
            //    Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitName),
            //    EUserId = info.EUserId,
            //    EntryDate = info.EntryDate.ToString(),
            //    Quantity = info.Quantity,
            //    IssueQty = info.IssueQty,
            //    EntryUser = UserForEachRecord(info.EUserId.Value).UserName,
            //    OrganizationId = info.OrganizationId,
            //}).ToList();

            #endregion

            IQCItemReqInfoListViewModel iQCItemReqInfoListViewModel = new IQCItemReqInfoListViewModel();
            AutoMapper.Mapper.Map(dto, iQCItemReqInfoListViewModel);
            return PartialView(iQCItemReqInfoListViewModel);
        }

        [HttpGet]
        public ActionResult GetIQCReqInfoListForWarehouse(long? WarehouseId, long? modelId, string stateStatus, int page = 1)
        {
            var dto = _iQCItemReqInfoList.GetIQCItemReqInfoLists(WarehouseId, modelId, stateStatus, User.OrgId).OrderByDescending(s => s.IQCItemReqInfoId).ToList();
            #region Comment Old Code
            // IEnumerable<IQCItemReqInfoListDTO> dto = _iQCItemReqInfoList.GetIQCItemReqInfoListByOrgId(User.OrgId).Select(info => new IQCItemReqInfoListDTO
            // {
            //     IQCId = info.IQCId,
            //     IQCItemReqInfoId = info.IQCItemReqInfoId,
            //     IQCName = (_iQCBusiness.GetIQCOneByOrgId(info.IQCId.Value, User.OrgId).IQCName),
            //     StateStatus = info.StateStatus,
            //     WarehouseId = info.WarehouseId,
            //     DescriptionId = info.DescriptionId,
            //     Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
            //     ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, User.OrgId).DescriptionName),
            //     SupplierId = info.SupplierId,
            //     Supplier = (_supplierBusiness.GetSupplierById(info.SupplierId.Value, User.OrgId).SupplierName),
            //     Remarks = info.Remarks,
            //     OrganizationId = info.OrganizationId
            // }).OrderByDescending(s => s.IQCItemReqInfoId).ToList();

            // dto = dto.Where(iqc =>
            //(WarehouseId == null || WarehouseId == 0 || iqc.WarehouseId == WarehouseId)
            //&& (modelId == null || modelId == 0 || iqc.DescriptionId == modelId)
            //&& (stateStatus == null || stateStatus == "" || iqc.StateStatus == stateStatus)
            //).OrderByDescending(s => s.IQCItemReqInfoId).ToList();
            #endregion
            //Pagination 
            ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
            dto = dto.Skip((page - 1) * 15).Take(15).ToList();
            //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
            //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            List<IQCItemReqInfoListViewModel> iQCItemReqInfoListViewModels = new List<IQCItemReqInfoListViewModel>();
            AutoMapper.Mapper.Map(dto, iQCItemReqInfoListViewModels);
            return PartialView(iQCItemReqInfoListViewModels);
        }
        public ActionResult IssueIQCItemReq(long reqId)
        {
            var req = _iQCItemReqInfoList.GetIQCItemReqById(reqId, User.OrgId);
            if (req != null && req.StateStatus == RequisitionStatus.Pending)
            {
                IQCItemReqInfoListDTO dto = _iQCItemReqInfoList.GetIQCItemReqDataById(reqId, User.OrgId);
                dto.IQCItemReqDetails = _iQCItemReqDetailList.GetIQCItemReqDetails(reqId, User.OrgId).ToList();

                IQCItemReqInfoListViewModel viewModel = new IQCItemReqInfoListViewModel();
                AutoMapper.Mapper.Map(dto, viewModel);
                return View(viewModel);
            }
            return RedirectToAction("GetIQCReqInfoListForWarehouse");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveIQCItemReqStatus(IQCItemReqInfoListViewModel models)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                if (models.StateStatus == RequisitionStatus.Approved && models.IQCItemReqDetails.Count > 0)
                {
                    IQCItemReqInfoListDTO dto = new IQCItemReqInfoListDTO();
                    AutoMapper.Mapper.Map(models, dto);
                    IsSuccess = _iQCItemReqDetailList.SaveIQCItemRequestIssueByWarehouse(dto, User.OrgId, User.UserId);
                }
                else
                {
                    IsSuccess = _iQCItemReqDetailList.SaveIQCItemRequestStatus(models.IQCItemReqInfoId, models.StateStatus, User.OrgId, User.UserId);
                }
            }
            return Json(IsSuccess);
        }
        #endregion

        #region IQCStock

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveIQCStockInStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            if (reqId > 0)
            {
                if (reqId > 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
                {
                    IsSuccess = _iQCStockDetailBusiness.SaveIQCStockInByIQCRequest(reqId, status, User.OrgId, User.UserId);
                }
                else
                {
                    IsSuccess = _iQCItemReqInfoList.SaveIQCReqInfoStatus(reqId, status, User.OrgId, User.UserId);
                }
            }
            return Json(IsSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveIQCStockIn(List<IQCStockDetailViewModel> models)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && models.Count > 0)
            {
                try
                {
                    List<IQCStockDetailDTO> dto = new List<IQCStockDetailDTO>();
                    AutoMapper.Mapper.Map(models, dto);
                    IsSuccess = _iQCStockDetailBusiness.SaveIQCStockIn(dto, User.OrgId, User.UserId);
                }
                catch (Exception)
                {
                    IsSuccess = false;
                }
            }
            return Json(IsSuccess);
        }

        public ActionResult GetAllIQCStockInfoList( long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        {
            var dto = _iQCStockInfoBusiness.GetAllIQCStockInformationList(warehouseId, modelId, itemTypeId, itemId, lessOrEq, User.OrgId).OrderByDescending(s=> s.StockInfoId).ToList();
            //Pagination 
            ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
            dto = dto.Skip((page - 1) * 15).Take(15).ToList();

            IEnumerable<IQCStockInfoViewModel> viewModels = new List<IQCStockInfoViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return PartialView(viewModels);
        }

        public ActionResult GetAllIQCStockDetailList(string flag, string refNum, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string status, string stockStatus, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                ViewBag.ddlStockType = Utility.ListOfStockType().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();
                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
                ViewBag.ddlSupplier = _supplierBusiness.GetSuppliers(User.OrgId).Select(sup => new SelectListItem { Text = sup.SupplierName, Value = sup.SupplierId.ToString() }).ToList();
            }
            else
            {
                var dto = _iQCStockDetailBusiness.GetAllIQCStockDetailList(refNum, warehouseId, modelId, itemTypeId, itemId, status, stockStatus, fromDate, toDate, User.OrgId).ToList();
                //Pagination 
                ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
                dto = dto.Skip((page - 1) * 15).Take(15).ToList();

                List<IQCStockDetailViewModel> viewModels = new List<IQCStockDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetAllIQCStockDetailPartialList", viewModels);
            }
            return View();
        }
        #endregion
        #region IQC Return
        public ActionResult CreateIQCItemReturn()
        {
            ViewBag.ddlIQCReqCode = _iQCItemReqInfoList.GetIQCItemReqInfoListByOrgId(User.OrgId).Where(s=> s.StateStatus == "Accepted").Select(reqCode => new SelectListItem { Text = reqCode.IQCReqCode, Value = reqCode.IQCItemReqInfoId.ToString() });
            return View();
        }

        public ActionResult ReturnIQCItemReqData(long reqId)
        {
            var req = _iQCItemReqInfoList.GetIQCItemReqById(reqId, User.OrgId);
                IQCItemReqInfoListDTO dto = _iQCItemReqInfoList.GetIQCItemReqDataById(reqId, User.OrgId);
                dto.IQCItemReqDetails = _iQCItemReqDetailList.GetIQCItemReqDetails(reqId, User.OrgId).ToList();

                IQCItemReqInfoListViewModel viewModel = new IQCItemReqInfoListViewModel();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView(viewModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveIQCStockReturnInStatus(IQCItemReqInfoListViewModel models)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                IQCItemReqInfoListDTO dto = new IQCItemReqInfoListDTO();
                AutoMapper.Mapper.Map(models, dto);
                IsSuccess = _iQCStockDetailBusiness.SaveIQCStockOutByIQCReturn(dto, User.OrgId, User.UserId);
            }
            return Json(IsSuccess);
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveWarehouseStockInByIQCReturn(long reqId, string status)
        {
            bool IsSuccess = false;
            if (reqId > 0)
            {
                if (reqId > 0 && !string.IsNullOrEmpty(status) && status == "Receive-Return")
                {
                    IsSuccess = _iQCStockDetailBusiness.SaveWarehouseStockInByIQCReturnReacive(reqId, status, User.OrgId, User.UserId);
                }
                else
                {
                    IsSuccess = _iQCStockDetailBusiness.SaveWarehouseStockInByIQCReturnReacive(reqId, status, User.OrgId, User.UserId);
                }
            }
            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult GetIQCReturnDataInfoList(long? WarehouseId, long? modelId, string stateStatus, int page = 1)
        {
            stateStatus = string.IsNullOrEmpty(stateStatus) ? string.Format(@"'Return','Receive-Return'") : string.Format(@"'{0}'",stateStatus) ;
            var dto = _iQCItemReqInfoList.GetIQCItemReqInfoLists(WarehouseId, modelId, stateStatus, User.OrgId).OrderByDescending(s => s.IQCItemReqInfoId).ToList();
            
            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
            dto = dto.Skip((page - 1) * 15).Take(15).ToList();

            List<IQCItemReqInfoListViewModel> iQCItemReqInfoListViewModels = new List<IQCItemReqInfoListViewModel>();
            AutoMapper.Mapper.Map(dto, iQCItemReqInfoListViewModels);
            return PartialView(iQCItemReqInfoListViewModels);
        }

        [HttpGet]
        public ActionResult GetIQCReturnDataInfoListForWarehouse(long? WarehouseId, long? modelId, string stateStatus, int page = 1)
        {
            stateStatus = string.IsNullOrEmpty(stateStatus) ? string.Format(@"'Return','Receive-Return'") : string.Format(@"'{0}'", stateStatus);

            var dto = _iQCItemReqInfoList.GetIQCItemReqInfoLists(WarehouseId, modelId, stateStatus, User.OrgId).OrderByDescending(s => s.IQCItemReqInfoId).ToList();

            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
            dto = dto.Skip((page - 1) * 15).Take(15).ToList();

            List<IQCItemReqInfoListViewModel> iQCItemReqInfoListViewModels = new List<IQCItemReqInfoListViewModel>();
            AutoMapper.Mapper.Map(dto, iQCItemReqInfoListViewModels);
            return PartialView(iQCItemReqInfoListViewModels);
        }
        #endregion
        #endregion

        #region EXCEL DATA IMPORT

        #region Comment OLD For Practise
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString);

        //OleDbConnection Econ;

        //private void ExcelConn(string filepath)
        //{
        //    string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);

        //    Econ = new OleDbConnection(constr);

        //}

        //private void InsertExceldata(string filepath, string filename)
        //{

        //    string fullpath = Server.MapPath("/ExcelFile/") + filename;

        //    ExcelConn(fullpath);

        //    string query = string.Format("Select * from [{0}]", "Sheet1$");

        //    OleDbCommand Ecom = new OleDbCommand(query, Econ);

        //    Econ.Open();

        //    DataSet ds = new DataSet();

        //    OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);

        //    Econ.Close();

        //    oda.Fill(ds);

        //    DataTable dt = ds.Tables[0];

        //    SqlBulkCopy objbulk = new SqlBulkCopy(con);

        //    objbulk.DestinationTableName = "tblIQCList";

        //    objbulk.ColumnMappings.Add("IQCName", "IQCName");

        //    objbulk.ColumnMappings.Add("Remarks", "Remarks");

        //    objbulk.ColumnMappings.Add("IsActive", "IsActive");

        //    objbulk.ColumnMappings.Add("OrganizationId", "OrganizationId");

        //    objbulk.ColumnMappings.Add("EUserId", "EUserId");

        //    objbulk.ColumnMappings.Add("EntryDate", "EntryDate");

        //    objbulk.ColumnMappings.Add("UpUserId", "UpUserId");

        //    objbulk.ColumnMappings.Add("UpdateDate", "UpdateDate");

        //    con.Open();

        //    objbulk.WriteToServer(dt);

        //    con.Close();

        //}
        // GET:  
        //public ActionResult Index()
        //{
        //    return PartialView();
        //}
        //[HttpPost]
        //public ActionResult Index(HttpPostedFileBase file)

        //{

        //    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

        //    string filepath = "/ExcelFile/" + filename;

        //    file.SaveAs(Path.Combine(Server.MapPath("/ExcelFile"), filename));

        //    InsertExceldata(filepath, filename);

        //    if (System.IO.File.Exists(filepath))
        //    {
        //        System.IO.File.Delete(filepath);
        //    }

        //    return View();

        //}
        #endregion

        public ActionResult CreateExcel()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadExcel(WarehouseStockDetailDTO models, HttpPostedFileBase FileUpload)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/ExcelFile/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var linqToExcel = (from a in excelFile.Worksheet<WarehouseStockDetailDTO>(sheetName) select a).ToList();

                    List<WarehouseStockDetailDTO> warehouseStocks = new List<WarehouseStockDetailDTO>();
                    
                    //    catch (DbEntityValidationException ex)
                    //    {
                    //        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    //        {

                    //            foreach (var validationError in entityValidationErrors.ValidationErrors)
                    //            {

                    //                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                    //            }

                    //        }
                    //    }

                    foreach (var item in linqToExcel)
                    {
                        if (item.WarehouseId > 0 && item.ItemTypeId > 0 && item.ItemId > 0 && item.EUserId > 0 && item.SupplierId > 0 && item.DescriptionId > 0 && item.Quantity >= 0 && item.OrderQty >= 0) {
                            warehouseStocks.Add(item);
                        }
                        else
                        {
                            executionState.text += item.ItemId + " has error" + "</br>";
                        }
                    }
                                              
                    executionState.isSuccess =  (warehouseStocks.Count() > 0 ? _warehouseStockDetailBusiness.SaveWarehouseStockIn(warehouseStocks, User.UserId, User.OrgId) : executionState.isSuccess);
                    
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                }
            }
            return Json(executionState);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}