using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPWeb.Filters;
using System.Linq;
using System.Web.Mvc;
using ERPBO.Inventory.DTOModel;
using ERPBLL.ControlPanel.Interface;
using System.Collections.Generic;
using ERPBO.ControlPanel.ViewModels;
using ERPBLL.Configuration.Interface;
using ERPBLL.Common;
using ERPBO.Inventory.ViewModels;
using System.Threading.Tasks;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class CommonController : BaseController
    {
        #region Inventory
        private readonly IWarehouseBusiness _warehouseBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly ISupplierBusiness _supplierBusiness;
        private readonly IIQCBusiness _iQCBusiness;
        private readonly IIQCItemReqDetailList _iQCItemReqDetailList;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly IColorBusiness _colorBusiness;
        #endregion

        #region Production
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IAssemblyLineBusiness _assemblyLineBusiness;
        private readonly IQualityControlBusiness _qualityControlBusiness;
        private readonly IAssemblyLineStockInfoBusiness _assemblyLineStockInfoBusiness;
        private readonly IRepairLineBusiness _repairLineBusiness;
        private readonly IPackagingLineBusiness _packagingLineBusiness;
        private readonly IQCLineStockInfoBusiness _qCLineStockInfoBusiness;
        private readonly IPackagingLineStockInfoBusiness _packagingLineStockInfoBusiness;
        private readonly IRepairLineStockInfoBusiness _repairLineStockInfoBusiness;
        private readonly IQRCodeTraceBusiness _qRCodeTraceBusiness;
        private readonly IFaultyItemStockInfoBusiness _faultyItemStockInfoBusiness;
        private readonly IRepairItemStockInfoBusiness _repairItemStockInfoBusiness;
        private readonly IQCItemStockInfoBusiness _qCItemStockInfoBusiness;
        private readonly IProductionAssembleStockInfoBusiness _productionAssembleStockInfoBusiness;
        private readonly IFaultyCaseBusiness _faultyCaseBusiness;
        private readonly IFaultyItemStockDetailBusiness _faultyItemStockDetailBusiness;
        private readonly IQRCodeTransferToRepairInfoBusiness _qRCodeTransferToRepairInfoBusiness;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly IIMEITransferToRepairInfoBusiness _iMEITransferToRepairInfoBusiness;
        private readonly IPackagingFaultyStockInfoBusiness _packagingFaultyStockInfoBusiness;
        private readonly IPackagingFaultyStockDetailBusiness _packagingFaultyStockDetailBusiness;
        #endregion

        #region ControlPanel
        private readonly IAppUserBusiness _appUserBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IBranchBusiness _branchBusiness;
        private readonly IOrganizationBusiness _organizationBusiness;
        private readonly IUserAuthorizationBusiness _userAuthorizationBusiness;
        private readonly IModuleBusiness _moduleBusiness;
        private readonly IManiMenuBusiness _maniMenuBusiness;
        private readonly ISubMenuBusiness _subMenuBusiness;
        #endregion

        
        public CommonController(IWarehouseBusiness warehouseBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionLineBusiness productionLineBusiness, IProductionStockInfoBusiness productionStockInfoBusiness, IAppUserBusiness appUserBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IRoleBusiness roleBusiness, IBranchBusiness branchBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IOrganizationBusiness organizationBusiness, IUserAuthorizationBusiness userAuthorizationBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IMobilePartBusiness mobilePartBusiness, ICustomerBusiness customerBusiness, ITechnicalServiceBusiness technicalServiceBusiness, IAssemblyLineBusiness assemblyLineBusiness, IQualityControlBusiness qualityControlBusiness, ISupplierBusiness supplierBusiness, IAssemblyLineStockInfoBusiness assemblyLineStockInfoBusiness, IRepairLineBusiness repairLineBusiness, IPackagingLineBusiness packagingLineBusiness, IQCLineStockInfoBusiness qCLineStockInfoBusiness, IPackagingLineStockInfoBusiness packagingLineStockInfoBusiness, IRepairLineStockInfoBusiness repairLineStockInfoBusiness, IQRCodeTraceBusiness qRCodeTraceBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IFaultyItemStockInfoBusiness faultyItemStockInfoBusiness, IRepairItemStockInfoBusiness repairItemStockInfoBusiness, IQCItemStockInfoBusiness qCItemStockInfoBusiness, IProductionAssembleStockInfoBusiness productionAssembleStockInfoBusiness, IFaultyCaseBusiness faultyCaseBusiness, IFaultyItemStockDetailBusiness faultyItemStockDetailBusiness, IQRCodeTransferToRepairInfoBusiness qRCodeTransferToRepairInfoBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IIQCBusiness iQCBusiness, IIQCItemReqDetailList iQCItemReqDetailList, IIMEITransferToRepairInfoBusiness iMEITransferToRepairInfoBusiness, IPackagingFaultyStockInfoBusiness packagingFaultyStockInfoBusiness,IPackagingFaultyStockDetailBusiness packagingFaultyStockDetailBusiness, IDescriptionBusiness descriptionBusiness, IModuleBusiness moduleBusiness, IManiMenuBusiness maniMenuBusiness, ISubMenuBusiness subMenuBusiness, IColorBusiness colorBusiness)
        {
            #region Inventory Module
            this._warehouseBusiness = warehouseBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._itemBusiness = itemBusiness;
            this._warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._supplierBusiness = supplierBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._iQCBusiness = iQCBusiness;
            this._iQCItemReqDetailList = iQCItemReqDetailList;
            this._descriptionBusiness = descriptionBusiness;
            this._colorBusiness = colorBusiness;
            #endregion

            #region Production Module
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._assemblyLineBusiness = assemblyLineBusiness;
            this._qualityControlBusiness = qualityControlBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._assemblyLineStockInfoBusiness = assemblyLineStockInfoBusiness;
            this._repairLineBusiness = repairLineBusiness;
            this._packagingLineBusiness = packagingLineBusiness;
            this._qCLineStockInfoBusiness = qCLineStockInfoBusiness;
            this._packagingLineStockInfoBusiness = packagingLineStockInfoBusiness;
            this._repairLineStockInfoBusiness = repairLineStockInfoBusiness;
            this._qRCodeTraceBusiness = qRCodeTraceBusiness;
            this._faultyItemStockInfoBusiness = faultyItemStockInfoBusiness;
            this._repairItemStockInfoBusiness = repairItemStockInfoBusiness;
            this._qCItemStockInfoBusiness = qCItemStockInfoBusiness;
            this._productionAssembleStockInfoBusiness = productionAssembleStockInfoBusiness;
            this._faultyCaseBusiness = faultyCaseBusiness;
            this._faultyItemStockDetailBusiness = faultyItemStockDetailBusiness;
            this._qRCodeTransferToRepairInfoBusiness = qRCodeTransferToRepairInfoBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._iMEITransferToRepairInfoBusiness = iMEITransferToRepairInfoBusiness;
            this._packagingFaultyStockInfoBusiness = packagingFaultyStockInfoBusiness;
            this._packagingFaultyStockDetailBusiness = packagingFaultyStockDetailBusiness;
            #endregion

            #region ControlPanel
            this._appUserBusiness = appUserBusiness;
            this._roleBusiness = roleBusiness;
            this._branchBusiness = branchBusiness;
            this._organizationBusiness = organizationBusiness;
            this._userAuthorizationBusiness = userAuthorizationBusiness;
            this._moduleBusiness = moduleBusiness;
            this._maniMenuBusiness = maniMenuBusiness;
            this._subMenuBusiness = subMenuBusiness;
            #endregion
        }

        #region Control Panel
        [HttpPost]
        public ActionResult GetModules()
        {
            var modules =_moduleBusiness.GetAllModules().Select(s => new Dropdown { text = s.ModuleName, value = s.MId.ToString() }).ToList();
            return Json(modules);
        }
        [HttpPost]
        public ActionResult GetMainmenus()
        {
            var mainmenus = _maniMenuBusiness.GetAllMainMenu().Select(s => new Dropdown { text = s.MenuName, value = s.MMId.ToString() }).ToList();
            return Json(mainmenus);
        }
        [HttpPost]
        public ActionResult GetSubmenus()
        {
            var submenus = _subMenuBusiness.GetAllSubMenu().Select(s => new Dropdown { text = s.SubMenuName, value = s.SubMenuId.ToString() }).ToList();
            return Json(submenus);
        }
        [HttpPost]
        public ActionResult GetParentSubmenus()
        {
            var submenus = _subMenuBusiness.GetAllSubMenu().Where(s=> s.IsActAsParent).Select(s => new Dropdown { text = s.SubMenuName, value = s.SubMenuId.ToString() }).ToList();
            return Json(submenus);
        }
        #endregion

        #region User Menus
        public ActionResult GetUserMenus()
        {
            // This is a three level menu //
            List<UserMainMenuViewModel> listOfUserMainMenuViewModel = new List<UserMainMenuViewModel>();
            if (User.UserId > 0 && User.OrgId > 0 && User.IsUserActive == true)
            {
                var userAllMenus = (List<UserAuthorizeMenusViewModels>)Session["UserAuthorizeMenus"];
                var menus = (from mm in userAllMenus
                             select new { MainmenuId = mm.MainmenuId, MainmenuName = mm.MainmenuName }).OrderBy(s=>s.MainmenuId).Distinct().ToList();

                foreach (var mm in menus)
                {
                    UserMainMenuViewModel userMainMenuViewModel = new UserMainMenuViewModel();
                    userMainMenuViewModel.MainmenuId = mm.MainmenuId;
                    userMainMenuViewModel.MainmenuName = mm.MainmenuName;

                    List<UserSubmenuViewModel> listOfSubmenus = new List<UserSubmenuViewModel>();
                    var submenuWithParent = (from sub in userAllMenus
                                             where sub.MainmenuId == mm.MainmenuId && sub.ParentSubMenuId > 0
                                             select new { ParentSubMenuId = sub.ParentSubMenuId, ParentSubmenuName = sub.ParentSubmenuName }).Distinct().ToList();

                    var submenuWithoutParent = (from sub in userAllMenus
                                                where sub.MainmenuId == mm.MainmenuId && sub.ParentSubMenuId == 0 && sub.IsViewable == true
                                                select new { SubMenuId = sub.SubmenuId, SubmenuName = sub.SubMenuName, ControllerName = sub.ControllerName, ActionName = sub.ActionName }).Distinct().ToList();

                    foreach (var submenu in submenuWithoutParent)
                    {
                        UserSubmenuViewModel userSubmenu = new UserSubmenuViewModel();
                        userSubmenu.SubmenuId = submenu.SubMenuId;
                        userSubmenu.SubmenuName = submenu.SubmenuName;
                        userSubmenu.ControllerName = submenu.ControllerName;
                        userSubmenu.ActionName = submenu.ActionName;
                        userSubmenu.IsParent = false;
                        userSubmenu.UserSubSubmenus = new List<UserSubSubmenuViewModel>();
                        listOfSubmenus.Add(userSubmenu);
                    }
                    foreach (var submenu in submenuWithParent)
                    {
                        UserSubmenuViewModel userSubmenu = new UserSubmenuViewModel();
                        userSubmenu.SubmenuId = submenu.ParentSubMenuId;
                        userSubmenu.SubmenuName = submenu.ParentSubmenuName;
                        userSubmenu.ControllerName = string.Empty;
                        userSubmenu.ActionName = string.Empty;
                        userSubmenu.IsParent = true;

                        // Subsubmenu
                        List<UserSubSubmenuViewModel> listOfSubSubmenu = new List<UserSubSubmenuViewModel>();
                        var subsubmenuItems = (from sub in userAllMenus
                                               where sub.ParentSubMenuId == submenu.ParentSubMenuId && sub.IsViewable == true
                                               select new { SubmenuName = sub.SubMenuName, SubmenuId = sub.SubmenuId, ControllerName = sub.ControllerName, ActionName = sub.ActionName }).ToList();

                        foreach (var item in subsubmenuItems)
                        {
                            UserSubSubmenuViewModel subSubmenuViewModel = new UserSubSubmenuViewModel();
                            subSubmenuViewModel.ControllerName = item.ControllerName;
                            subSubmenuViewModel.ActionName = item.ActionName;
                            subSubmenuViewModel.SubsubmenuId = item.SubmenuId;
                            subSubmenuViewModel.SubsubmenuName = item.SubmenuName;
                            listOfSubSubmenu.Add(subSubmenuViewModel);
                        }
                        userSubmenu.UserSubSubmenus = listOfSubSubmenu;
                        listOfSubmenus.Add(userSubmenu);
                    }

                    userMainMenuViewModel.UserSubmenus = listOfSubmenus;
                    listOfUserMainMenuViewModel.Add(userMainMenuViewModel);
                }

            }
            return PartialView("_sidebar", listOfUserMainMenuViewModel);
        }
        #endregion

        #region Validation Action Methods
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateWarehouseName(string warehouseName, long id)
        {
            bool isExist = _warehouseBusiness.IsDuplicateWarehouseName(warehouseName, id, User.OrgId);
            return Json(isExist);
        }
        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateItemTypeName(string itemTypeName, long id, long warehouseId)
        {
            bool isExist = _itemTypeBusiness.IsDuplicateItemTypeName(itemTypeName, id, User.OrgId, warehouseId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateItemTypeShortName(string shortName, long id)
        {
            bool isExist = _itemTypeBusiness.IsDuplicateShortName(shortName, id, User.OrgId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateUnitName(string unitName, long id)
        {
            bool isExist = _unitBusiness.IsDuplicateUnitName(unitName, id, User.OrgId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateItemName(string itemName, long id)
        {
            bool isExist = _itemBusiness.IsDuplicateItemName(itemName, id, User.OrgId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateLineNumber(string lineNumber, long id)
        {
            bool isExist = _productionLineBusiness.IsDuplicateLineNumber(lineNumber, id, User.OrgId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateEmployeeId(string employeeId, long id)
        {
            bool isExist = _appUserBusiness.IsDuplicateEmployeeId(employeeId, id, User.OrgId);
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsUserExist(string userName, long id)
        {
            bool isUserExist = _appUserBusiness.GetAllAppUsers().Where(u => u.UserName.ToLower() == userName.ToLower() && u.UserId != id).FirstOrDefault() != null;

            return Json(isUserExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsEmailExist(string email, long id)
        {
            bool isEmailExist = _appUserBusiness.GetAllAppUsers().Where(u => u.Email.ToLower() == email.ToLower() && u.UserId != id).FirstOrDefault() != null;

            return Json(isEmailExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetUnitByItemId(long itemId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            UnitDomainDTO unitDTO = new UnitDomainDTO();
            unitDTO.UnitId = unit.UnitId;
            unitDTO.UnitName = unit.UnitName;
            unitDTO.UnitSymbol = unit.UnitSymbol;
            return Json(unitDTO);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemUnitAndPDNStockQtyByLineId(long itemId, long lineId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var productionStock = _productionStockInfoBusiness.GetAllProductionStockInfoByItemLineId(User.OrgId, itemId, lineId);
            var itemStock = 0;
            if (productionStock != null)
            {
                itemStock = (productionStock.StockInQty - productionStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemUnitAndPDNStockQtyByLineAndModelId(long itemId, long lineId, long modelId, string stockFor)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var productionStock = _productionStockInfoBusiness.GetAllProductionStockInfoByLineAndModelAndItemId(User.OrgId, itemId, lineId, modelId, stockFor);
            var itemStock = 0;
            if (productionStock != null)
            {
                itemStock = (productionStock.StockInQty - productionStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetAssemblyLineStockInfoByAssemblyAndItemAndModelId(long itemId, long assemblyId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var assemblyStock = _assemblyLineStockInfoBusiness.GetAssemblyLineStockInfoByAssemblyAndItemAndModelId(assemblyId, itemId, modelId, User.OrgId);
            var itemStock = 0;
            if (assemblyStock != null)
            {
                itemStock = (assemblyStock.StockInQty - assemblyStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetQCLineStockInfoByQCAndItemAndModelId(long itemId, long qcId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var qcStock = _qCLineStockInfoBusiness.GetQCLineStockInfoByQCAndItemAndModelId(qcId, itemId, modelId, User.OrgId);
            var itemStock = 0;
            if (qcStock != null)
            {
                itemStock = (qcStock.StockInQty - qcStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetPackagingLineStockInfoByPLAndItemAndModelId(long itemId, long packagingId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var packagingStock = _packagingLineStockInfoBusiness.GetPackagingLineStockInfoByPackagingAndItemAndModelId(packagingId, itemId, modelId, User.OrgId);
            var itemStock = 0;
            if (packagingStock != null)
            {
                itemStock = (packagingStock.StockInQty - packagingStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetRepairLineStockInfoByRepairQCAndItemAndModelId(long itemId, long repairId, long qcId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var packagingStock = _repairLineStockInfoBusiness.GetRepairLineStockInfoByRepairQCAndItemAndModelId(repairId, itemId, qcId, modelId, User.OrgId);
            var itemStock = 0;
            if (packagingStock != null)
            {
                itemStock = (packagingStock.StockInQty - packagingStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetRepairLineStockInfoByRepairAndItemAndModelId(long itemId, long repairId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var repairStock = _repairLineStockInfoBusiness.GetRepairLineStockInfoByRepairAndItemAndModelId(repairId, itemId, modelId, User.OrgId);
            var itemStock = 0;
            if (repairStock != null)
            {
                itemStock = (repairStock.StockInQty - repairStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetProductionAssembleStockInfoByLineAndItemAndModelId(long lineId, long itemId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var packagingStock = _productionAssembleStockInfoBusiness.productionAssembleStockInfoByFloorAndModelAndItem(lineId, modelId, itemId, User.OrgId);
            var itemStock = 0;
            if (packagingStock != null)
            {
                itemStock = (packagingStock.StockInQty - packagingStock.StockOutQty);
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetRepairLineQCRemainQty(long assemblyLineId, long repairLineId, long qcLineId, long modelId, long itemId)
        {
            var transferQty = _repairItemStockInfoBusiness.GetRepairItem(assemblyLineId, qcLineId, repairLineId, modelId, itemId, User.OrgId);
            var remainQty = transferQty.Quantity - transferQty.QCQty;
            return Json(remainQty);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetFaultyItemStockInfoByRepairAndModelAndItem(long itemId, long repairId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var repairStock = _faultyItemStockInfoBusiness.GetFaultyItemStockInfoByRepairAndModelAndItem(repairId, modelId, itemId, User.OrgId);
            var itemStock = 0;
            if (repairStock != null)
            {
                itemStock = (repairStock.StockInQty - repairStock.StockOutQty);
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetFaultyItemStockInfoByRepairAndModelAndItemAndFultyType(long itemId, long repairId, long modelId, bool isChinaFaulty)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var repairStock = _faultyItemStockInfoBusiness.GetFaultyItemStockInfoByRepairAndModelAndItemAndFultyType(repairId, modelId, itemId, isChinaFaulty, User.OrgId);
            var itemStock = 0;
            if (repairStock != null)
            {
                itemStock = (repairStock.StockInQty - repairStock.StockOutQty);
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemUnitAndFGStockQty(long lineId, long warehouseId, long itemId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, User.OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, User.OrgId);
            var finishGoodsStock = _finishGoodsStockInfoBusiness.GetFinishGoodsStockInfoByAll(User.OrgId, lineId, warehouseId, itemId, modelId);
            var itemStock = 0;
            if (finishGoodsStock != null)
            {
                itemStock = (finishGoodsStock.StockInQty - finishGoodsStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsStockAvailableForRequisition(long? reqInfoId)
        {
            //bool isValidExec = true;
            //string isValidTxt = string.Empty;
            //var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId.Value, User.OrgId).ToList();
            //var warehouseStock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(User.OrgId);
            //var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();

            //foreach (var item in reqDetail)
            //{
            //    var w = warehouseStock.FirstOrDefault(wr => wr.ItemId == item.ItemId);
            //    if (w != null)
            //    {
            //        if ((w.StockInQty - w.StockOutQty) < item.Quantity)
            //        {
            //            isValidExec = false;
            //            isValidTxt += items.FirstOrDefault(it => it.ItemId == item.ItemId).ItemName + " does not have enough stock </br>";
            //        }
            //    }
            //    else
            //    {
            //        isValidExec = false;
            //        isValidTxt += items.FirstOrDefault(it => it.ItemId == item.ItemId).ItemName + " does not have enough stock </br>";
            //    }
            //}

            ExecutionStateWithText stateWithText = GetExecutionStockAvailableForRequisition(reqInfoId);
            return Json(stateWithText);
        }

        [NonAction]
        private ExecutionStateWithText GetExecutionStockAvailableForRequisition(long? reqInfoId)
        {
            ExecutionStateWithText stateWithText = new ExecutionStateWithText();
            var descriptionId = _requsitionInfoBusiness.GetRequisitionById(reqInfoId.Value, User.OrgId).DescriptionId;
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId.Value, User.OrgId).ToList();
            var warehouseStock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(User.OrgId);
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();
            stateWithText.isSuccess = true;
            foreach (var item in reqDetail)
            {
                var w = warehouseStock.Where(wr => wr.ItemId == item.ItemId && wr.DescriptionId == descriptionId).FirstOrDefault();
                if (w != null)
                {
                    if ((w.StockInQty - w.StockOutQty) < item.Quantity)
                    {
                        stateWithText.isSuccess = false;
                        stateWithText.text += items.Where(it => it.ItemId == item.ItemId).FirstOrDefault().ItemName + " does not have enough stock </br>";
                    }
                }
                else
                {
                    stateWithText.isSuccess = false;
                    stateWithText.text += items.Where(it => it.ItemId == item.ItemId).FirstOrDefault().ItemName + " does not have enough stock </br>";
                }
            }

            return stateWithText;
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicationItemPreparation(long itemId, long modelId,string type)
        {
            bool IsDuplicate = false;
            IsDuplicate = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndType(type,modelId, itemId, User.OrgId) != null;
            return Json(IsDuplicate);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateRoleName(long roleId, string roleName, long orgId)
        {
            bool IsDuplicate = false;
            IsDuplicate = _roleBusiness.IsDuplicateRoleName(roleName, roleId, orgId);
            return Json(IsDuplicate);
        }

        [HttpPost]
        public ActionResult IsCurrentUserPasswordCorrect(string password)
        {
            bool IsCorrect = false;
            UserLogInViewModel loginModel = new UserLogInViewModel
            {
                UserName = User.UserName
            };
            loginModel.Password = Utility.Encrypt(password);
            var user = _appUserBusiness.GetAppUserOneById(User.UserId, User.OrgId);
            if (user != null)
            {
                IsCorrect = user.Password == Utility.Encrypt(password);
            }
            return Json(IsCorrect);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateAssemblyInLine(long lineId, long id, string assemblyName)
        {
            var assembly = _assemblyLineBusiness.GetAssemblyLines(User.OrgId).FirstOrDefault(a => a.ProductionLineId == lineId && a.AssemblyLineName == assemblyName && a.AssemblyLineId != id) != null;
            return Json(assembly);
        }

        #region Dropdown List
        [HttpPost]
        public ActionResult GetItemsByLine(long lineId)
        {
            var data = _itemBusiness.GetAllItemsInProductionStockByLineId(lineId, User.OrgId).Select(i => new Dropdown
            {
                value = i.ItemId.ToString(),
                text = i.ItemName
            }).ToList();

            return Json(data);
        }

        [HttpPost]
        public ActionResult GetColors()
        {
            var data = _colorBusiness.GetAllColorByOrgId(User.OrgId).Select(s => new Dropdown { value = s.ColorId.ToString(), text = s.ColorName }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetItemTypeForDDL(long warehouseId)
        {
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).AsEnumerable();
            var dropDown = itemTypes.Where(i => i.WarehouseId == warehouseId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost]
        public ActionResult GetItemForDDL(long itemTypeId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).AsEnumerable();
            var dropDown = items.Where(i => i.ItemTypeId == itemTypeId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost]
        public ActionResult GetRolesByOrgId(long orgId)
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>();
            if (orgId > 0)
            {
                dropdowns = _roleBusiness.GetAllRoleByOrgId(orgId).Select(r => new Dropdown { text = r.RoleName, value = r.RoleId.ToString() }).ToList();
            }
            return Json(dropdowns);
        }
        [HttpPost]
        public ActionResult GetBranchesByOrgId(long orgId)
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>();
            if (orgId > 0)
            {
                dropdowns = _branchBusiness.GetBranchByOrgId(orgId).Select(r => new Dropdown { text = r.BranchName, value = r.BranchId.ToString() }).ToList();
            }
            return Json(dropdowns);
        }

        [HttpPost]
        public ActionResult GetItemsByWarehouseId(long warehouseId)
        {
            var data = _itemBusiness.GetItemsByWarehouseId(warehouseId, User.OrgId).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetUsersByOrg(long orgId)
        {
            List<Dropdown> list = new List<Dropdown>();
            if (orgId > 0)
            {
                if (_organizationBusiness.GetOrganizationById(orgId) != null)
                {
                    list = _appUserBusiness.GetAllAppUserByOrgId(orgId).Select(u => new Dropdown
                    {
                        text = u.UserName,
                        value = u.UserId.ToString()
                    }).ToList();

                }
            }
            return Json(list);
        }

        [HttpPost]
        public ActionResult GetWarehouseByProductionLineId(long lineId)
        {
            var data = _warehouseBusiness.GetAllWarehouseByProductionLineId(User.OrgId, lineId).Select(w => new Dropdown
            {
                text = w.WarehouseName,
                value = w.Id.ToString()
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetRolesByOrg(long orgId)
        {
            List<Dropdown> list = new List<Dropdown>();
            if (orgId > 0)
            {
                if (_organizationBusiness.GetOrganizationById(orgId) != null)
                {
                    list = _roleBusiness.GetAllRoleByOrgId(orgId).Select(u => new Dropdown
                    {
                        text = u.RoleName,
                        value = u.RoleId.ToString()
                    }).ToList();

                }
            }
            return Json(list);
        }

        [HttpPost]
        public ActionResult GetAssembliesByLine(long lineId)
        {
            var data = _assemblyLineBusiness.GetAssemblyLines(User.OrgId).Where(a => a.ProductionLineId == lineId).Select(i => new Dropdown
            {
                value = i.AssemblyLineId.ToString(),
                text = i.AssemblyLineName
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetQCByLine(long lineId)
        {
            var data = _qualityControlBusiness.GetQualityControls(User.OrgId).Where(a => a.ProductionLineId == lineId).Select(i => new Dropdown
            {
                value = i.QCId.ToString(),
                text = i.QCName
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetRepairLineByLine(long lineId)
        {
            var data = _repairLineBusiness.GetRepairLinesByOrgId(User.OrgId).Where(a => a.ProductionLineId == lineId).Select(i => new Dropdown
            {
                value = i.RepairLineId.ToString(),
                text = i.RepairLineName
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetPackagingLineByLine(long lineId)
        {
            var data = _packagingLineBusiness.GetPackagingLinesByOrgId(User.OrgId).Where(a => a.ProductionLineId == lineId).Select(i => new Dropdown
            {
                value = i.PackagingLineId.ToString(),
                text = i.PackagingLineName
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetPackagingLineToByLine(long lineId, long packagingId)
        {
            var data = _packagingLineBusiness.GetPackagingLinesByOrgId(User.OrgId).Where(a => a.ProductionLineId == lineId && a.PackagingLineId != packagingId).Select(i => new Dropdown
            {
                value = i.PackagingLineId.ToString(),
                text = i.PackagingLineName
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetLastPackagingLineByProductionId(long lineId)
        {
            var packagingLine = _packagingLineBusiness.GetPackagingLinesByOrgId(User.OrgId).LastOrDefault(a => a.ProductionLineId == lineId);
            List<Dropdown> dropdown = new List<Dropdown>()
            {
                new Dropdown(){text= packagingLine.PackagingLineName,value=packagingLine.PackagingLineId.ToString()}
            };

            return Json(dropdown);
        }

        [HttpPost]
        public ActionResult GetQrCodeDetail(string qrCode)
        {
            var qrCodeInfo = _qRCodeTraceBusiness.GetQRCodeTraceByCode(qrCode, User.OrgId);
            List<Dropdown> dropdowns = new List<Dropdown>();
            if (qrCodeInfo != null)
            {
                dropdowns = _itemBusiness.GetItemPreparationItems(qrCodeInfo.DescriptionId.Value, qrCodeInfo.ItemId.Value, ItemPreparationType.Production, User.OrgId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId }).ToList();
                qrCodeInfo.EUserId = 0;
                qrCodeInfo.EntryDate = null;
                qrCodeInfo.ReferenceId = string.Empty;
                qrCodeInfo.OrganizationId = 0;
            }
            return Json(new { info = qrCodeInfo, items = dropdowns.ToArray() });
        }

        [HttpPost]
        public ActionResult IsQRCodeExistInRepair(string qrCode)
        {
            var IsExist = _qRCodeTransferToRepairInfoBusiness.IsQRCodeExistInTransferWithStatus(qrCode, string.Format(@"'Received'"), User.OrgId);
            return Json(IsExist);
        }

        [HttpPost]
        public ActionResult IsExistInAssembly(string qrCode)
        {
            var IsExist = _tempQRCodeTraceBusiness.IsExistQRCodeWithStatus(qrCode, QRCodeStatus.Assembly, User.OrgId);
            return Json(IsExist);
        }
        [HttpPost]
        public ActionResult IsExistInAssemblyRepair(string qrCode)
        {
            var IsExist = _tempQRCodeTraceBusiness.IsExistQRCodeWithStatus(qrCode, QRCodeStatus.AssemblyRepair, User.OrgId);
            return Json(IsExist);
        }

        [HttpPost]
        public ActionResult IsExistInPackagingLine(string imei)
        {
            var IsExist = _tempQRCodeTraceBusiness.IsExistIMEIWithStatus(imei, QRCodeStatus.Packaging, User.OrgId);
            return Json(IsExist);
        }

        [HttpPost]
        public ActionResult IsExistInPackagingRepair(string imei)
        {
            var IsExist = _iMEITransferToRepairInfoBusiness.IsIMEIExistInTransferWithStatus(imei, string.Format(@"'Received'"), User.OrgId);
            return Json(IsExist);
        }

        [HttpPost]
        public async Task<ActionResult> IMEIinFinishGoods(string imei,long floorId, long packagingId)
        {
            var imeiInfo = await _tempQRCodeTraceBusiness.GetIMEIinQRCode(imei,string.Format(@"'{0}'",QRCodeStatus.Finish),floorId,packagingId, User.OrgId);
            return Json(imeiInfo);
        }

        [HttpPost]
        public async Task<ActionResult> IMEIWithoutThisQRCode(string imei, long codeId)
        {
            var isNotExist = await _tempQRCodeTraceBusiness.GetIMEIWithOutThisQRCode(imei, codeId, User.OrgId) == null ;
            return Json(isNotExist);
        }
        #endregion


        #region Production Module

        #region Validate Checker
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateQCName(long lineId, long id, string qcName)
        {
            var qualityControl = _qualityControlBusiness.GetQualityControls(User.OrgId).FirstOrDefault(qc => qc.ProductionLineId == lineId && qc.QCName == qcName && qc.QCId != id) != null;
            return Json(qualityControl);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateRepairName(long lineId, long id, string rlName)
        {
            var repairLine = _repairLineBusiness.GetRepairLinesByOrgId(User.OrgId).FirstOrDefault(rl => rl.ProductionLineId == lineId && rl.RepairLineName == rlName && rl.RepairLineId != id) != null;
            return Json(repairLine);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicatePackagingLineName(long lineId, long id, string plName)
        {
            var packagingLine = _packagingLineBusiness.GetPackagingLinesByOrgId(User.OrgId).FirstOrDefault(pl => pl.ProductionLineId == lineId && pl.PackagingLineName == plName && pl.PackagingLineId != id) != null;
            return Json(packagingLine);
        }
        [HttpPost]
        public ActionResult GetItemDetail()
        {
            var items = _itemBusiness.GetItemDetails(User.OrgId).Select(d => new Dropdown
            {
                text = d.ItemName.ToString(),
                value = d.ItemId.ToString()
            }).ToList();
            return Json(items);
        }

        [HttpPost]
        public ActionResult GetItemDetailByRepairFaultySection(long floorId, long repairLineId, long modelId)
        {
            var items = _itemBusiness.GetItemDetailByRepairFaultySection(floorId, repairLineId, modelId, User.OrgId).Select(d => new Dropdown
            {
                text = d.ItemName.ToString(),
                value = d.ItemId.ToString()
            }).ToList();
            return Json(items);
        }

        //GetItemsByWarehouseId

        [HttpPost]
        public ActionResult GetItemsByWarehouse(long warehouseId)
        {
            var items = _itemBusiness.GetItemsByWarehouseId(warehouseId, User.OrgId).ToList();
            return Json(items);
        }

        [HttpPost]
        public ActionResult IsItemPreparationExistWithThistype(string type, long modelId, long itemId)
        {
            var preparation = _itemPreparationInfoBusiness.IsItemPreparationExistWithThistype(type, modelId, itemId, User.OrgId);
            return Json(preparation);
        }

        public ActionResult GetQCItemStockQtyByFloorAndQcAndModelAndItem(long floorId, long qcId, long modelId, long itemId)
        {
            var stock = _qCItemStockInfoBusiness.GetQCItemStockInfoByFloorAndQcAndModelAndItem(floorId, 0, qcId, modelId, itemId, User.OrgId);
            return Json(stock.Quantity);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemBundleQtyChecking(long floorId, long qcLineId, long modelId, long itemId, int qty, string type)
        {
            ExecutionStateWithText execution = new ExecutionStateWithText();
            var info = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndType(type, modelId, itemId, User.OrgId);
            execution.text = string.Empty;
            execution.isSuccess = true;

            var preparationDetails = _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoId(info.PreparationInfoId, User.OrgId);

            foreach (var item in preparationDetails)
            {
                string itemName = _itemBusiness.GetItemOneByOrgId(item.ItemId, User.OrgId).ItemName;
                int prepareQty = (item.Quantity * qty);
                // Checking 
                if (qcLineId > 0)
                {
                    // Go TO QC Stcok;
                    var QcStock = _qCLineStockInfoBusiness.GetQCLineStockInfoByQCAndItemAndModelId(qcLineId, item.ItemId, modelId, User.OrgId);
                    int stockInQty = QcStock.StockInQty.HasValue ? QcStock.StockInQty.Value : 0;
                    int stockOutQty = QcStock.StockOutQty.HasValue ? QcStock.StockOutQty.Value : 0;

                    if (prepareQty > (stockInQty - stockOutQty))
                    {
                        execution.text += itemName + " does not have qty <br/>";
                        execution.isSuccess = false;
                    }
                }
            }
            if (!execution.isSuccess)
            {
                execution.text = execution.text.Substring(0, execution.text.Length - 1);
            }
            return Json(execution);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemPreparationBundleByTypeModelItem(string type, long model, long item)
        {
            var preparationInfo = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndType(type, model, item, User.OrgId);
            long infoId = preparationInfo == null ? 0 : preparationInfo.PreparationInfoId;

            var preparationDetail = _itemPreparationDetailBusiness.GetItemPreparationDetailWithInfo(infoId, User.OrgId);

            List<ItemPreparationDetailWithInfoViewModel> viewModels = new List<ItemPreparationDetailWithInfoViewModel>();
            AutoMapper.Mapper.Map(preparationDetail, viewModels);

            return Json(viewModels);
        }

        public ActionResult IsDuplicateBrachName(string branchName, long branchId, long orgId)
        {
            var branches =_branchBusiness.GetBranchByOrgId(orgId);
            var isExist = branches.FirstOrDefault(s => s.BranchName == branchName && s.BranchId != branchId && s.OrganizationId == orgId) != null;
            return Json(isExist);
        }
        #endregion

        #endregion

        #region Inventory Module
        [HttpPost]
        public ActionResult IsSupplierPhoneNumDuplicate(long supId, string phoneNum)
        {
            var supplierInDb = this._supplierBusiness.GetSuppliers(User.OrgId).FirstOrDefault(s => s.PhoneNumber == phoneNum && s.SupplierId != supId) != null;
            return Json(supplierInDb);
        }
        [HttpPost]
        public ActionResult IsSupplierMoblieNumDuplicate(long supId, string mobileNum)
        {
            var supplierInDb = this._supplierBusiness.GetSuppliers(User.OrgId).FirstOrDefault(s => s.MobileNumber == mobileNum && s.SupplierId != supId) != null;
            return Json(supplierInDb);
        }
        [HttpPost]
        public ActionResult IsSupplierEmailDuplicate(long supId, string email)
        {
            var supplierInDb = this._supplierBusiness.GetSuppliers(User.OrgId).FirstOrDefault(s => s.Email == email && s.SupplierId != supId) != null;
            return Json(supplierInDb);
        }

        [HttpPost]
        public ActionResult IsDuplicateIQCName(long iqcId, string iqcName)
        {
            bool isExists = _iQCBusiness.IsDuplicateIQCName(iqcId, User.OrgId, iqcName);
            return Json(isExists);
        }

        #region DropdownList

        [HttpPost]
        public ActionResult GetItemDetailsForDDL()
        {
            var items = _itemBusiness.GetItemDetails(User.OrgId).AsEnumerable();
            var dropDown = items.Select(i => new Dropdown { text = i.ItemName, value = i.ItemId }).ToList();
            return Json(dropDown);
        }

        [HttpPost]

        public ActionResult GetItemByWarehouseId(long warehouseId)
        {
            var item = _itemBusiness.GetItemsByWarehouseId(warehouseId, User.OrgId).AsEnumerable();
            return Json(item);
        }
        [HttpPost]
        public ActionResult GetWarehouseItemAvailableQty(long? itemTypeId,long? itemId,long? modelId)
        {
            var qty = _warehouseStockInfoBusiness.GetWarehouseStock(null, modelId, null, itemTypeId, itemId, User.OrgId);
            return Json(qty);
        }

        [HttpPost]
        public ActionResult GetIQCItemReqIssueQty(long? itemTypeId, long? itemId, long? modelId)
        {
            var qty = _iQCItemReqDetailList.GetIssueQty(modelId, itemTypeId, itemId, User.OrgId);
            return Json(qty);
        }
        #endregion
        #endregion

        #region  Async Action Methods
        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> GetFaultyCasesListAsync()
        {
            var data = await _faultyCaseBusiness.GetFaultyCasesAsync(User.OrgId);
            var faultyCases = data.Select(s => new { CaseId = s.CaseId, ProblemDescription = s.ProblemDescription }).ToList();
            return Json(faultyCases);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> GetQRCodeTraceByCodeAsync(string qrCode)
        {
            var data = await _tempQRCodeTraceBusiness.GetTempQRCodeTraceByCodeAsync(qrCode, User.OrgId);
            var qrCodeData = new { Floor = (data != null ? data.ProductionFloorId : 0), Assembly = (data != null ? data.AssemblyId : 0), Model = (data != null ? data.DescriptionId : 0), Item = (data != null ? data.ItemId : 0), ItemType = (data != null ? data.ItemTypeId : 0), Warehouse = (data != null ? data.WarehouseId : 0) };
            var passedData = qrCodeData.Floor == 0 ? null : qrCodeData;
            return Json(qrCodeData);
        }
        #endregion


        #region Repair Section
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetFaultyItemsByQRCode(string qrCode, long transferId)
        {
            IEnumerable<FaultyItemStockDetailViewModel> faultyItemsViewModel = new List<FaultyItemStockDetailViewModel>();
            var faultyItemsDto = _faultyItemStockDetailBusiness.GetFaultyItemStockDetailsByQrCode(qrCode, transferId, User.OrgId);
            AutoMapper.Mapper.Map(faultyItemsDto, faultyItemsViewModel);
            return Json(new { faultyItems = faultyItemsViewModel });
        }

        public ActionResult CheckingAvailabilityOfSparepartsWithRepairLineStock(long modelId, long itemId, long repairLineId)
        {
            ExecutionStateWithText execution = _qRCodeTransferToRepairInfoBusiness.CheckingAvailabilityOfSparepartsWithRepairLineStock(modelId, itemId, repairLineId, User.OrgId);
            return Json(execution);
        }

        public ActionResult CheckingAvailabilityOfPackagingRepairRawStock(long modelId, long itemId, long packagingLineId)
        {
            ExecutionStateWithText execution = _iMEITransferToRepairInfoBusiness.CheckingAvailabilityOfPackagingRepairRawStock(modelId, itemId,packagingLineId, User.OrgId);
            return Json(execution);
        }

        #endregion

        #region Packaging Repair Section
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetPackagingRepairFaultyItemsByQRCode(string qrCode, long transferId)
        {
            IEnumerable<PackagingFaultyStockDetailViewModel> faultyItemsViewModel = new List<PackagingFaultyStockDetailViewModel>();
            var faultyItemsDto = _packagingFaultyStockDetailBusiness.GetPackagingFaultyItemStockDetailsByQrCode(qrCode, string.Empty,transferId, User.OrgId);
            AutoMapper.Mapper.Map(faultyItemsDto, faultyItemsViewModel);
            return Json(new { faultyItems = faultyItemsViewModel });
        }
        [HttpPost]
        public ActionResult GetDescriptiolnById(long id)
        {
            var data = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).FirstOrDefault(s => s.DescriptionId == id);
            return Json(data);
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}