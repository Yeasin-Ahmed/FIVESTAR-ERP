using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPWeb.Filters;
using System.Linq;
using System.Web.Mvc;
using ERPBO.Inventory.DTOModel;
using ERPBLL.ControlPanel.Interface;
using System.Collections.Generic;

namespace ERPWeb.Controllers
{

    public class CommonController : BaseController
    {
        private readonly IWarehouseBusiness _warehouseBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;
        private readonly IAppUserBusiness _appUserBusiness;
        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IBranchBusiness _branchBusiness;
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;
        public CommonController(IWarehouseBusiness warehouseBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionLineBusiness productionLineBusiness, IProductionStockInfoBusiness productionStockInfoBusiness, IAppUserBusiness appUserBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness,IRoleBusiness roleBusiness, IBranchBusiness branchBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness)
        {
            this._warehouseBusiness = warehouseBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._itemBusiness = itemBusiness;
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._appUserBusiness = appUserBusiness;
            this._warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            this._roleBusiness = roleBusiness;
            this._branchBusiness = branchBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
        }

        #region Validation Action Methods
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateWarehouseName(string warehouseName, long id)
        {
            bool isExist = _warehouseBusiness.IsDuplicateWarehouseName(warehouseName, id, OrgId);
            return Json(isExist);
        }
        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateItemTypeName(string itemTypeName, long id, long warehouseId)
        {
            bool isExist = _itemTypeBusiness.IsDuplicateItemTypeName(itemTypeName, id, OrgId, warehouseId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateUnitName(string unitName, long id)
        {
            bool isExist = _unitBusiness.IsDuplicateUnitName(unitName, id, OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateItemName(string itemName, long id)
        {
            bool isExist = _itemBusiness.IsDuplicateItemName(itemName, id, OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateLineNumber(string lineNumber, long id)
        {
            bool isExist = _productionLineBusiness.IsDuplicateLineNumber(lineNumber, id, OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateEmployeeId(string employeeId, long id)
        {
            bool isExist = _appUserBusiness.IsDuplicateEmployeeId(employeeId, id, OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsUserExist(string userName,long id)
        {
           bool isUserExist = _appUserBusiness.GetAllAppUsers().Where(u => u.UserName.ToLower() == userName.ToLower() && u.UserId != id).FirstOrDefault() != null;

            return Json(isUserExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsEmailExist(string email,long id)
        {
            bool isEmailExist = _appUserBusiness.GetAllAppUsers().Where(u => u.Email.ToLower() == email.ToLower() && u.UserId != id).FirstOrDefault() != null;

            return Json(isEmailExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetUnitByItemId(long itemId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, OrgId);
            UnitDomainDTO unitDTO = new UnitDomainDTO();
            unitDTO.UnitId = unit.UnitId;
            unitDTO.UnitName = unit.UnitName;
            unitDTO.UnitSymbol = unit.UnitSymbol;
            return Json(unitDTO);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemUnitAndPDNStockQtyByLineId(long itemId, long lineId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, OrgId);
            var productionStock = _productionStockInfoBusiness.GetAllProductionStockInfoByItemLineId(OrgId, itemId, lineId);
            var itemStock = 0;
            if (productionStock != null)
            {
                itemStock = (productionStock.StockInQty - productionStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemUnitAndPDNStockQtyByLineAndModelId(long itemId, long lineId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, OrgId);
            var productionStock = _productionStockInfoBusiness.GetAllProductionStockInfoByLineAndModelId(OrgId, itemId, lineId, modelId);
            var itemStock = 0;
            if (productionStock != null)
            {
                itemStock = (productionStock.StockInQty - productionStock.StockOutQty).Value;
            }
            return Json(new { unitid = unit.UnitId, unitName = unit.UnitName, unitSymbol = unit.UnitSymbol, stockQty = itemStock });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemUnitAndFGStockQty(long lineId,long warehouseId,long itemId, long modelId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, OrgId);
            var finishGoodsStock = _finishGoodsStockInfoBusiness.GetFinishGoodsStockInfoByAll(OrgId, lineId,warehouseId,itemId,modelId);
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
            //var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId.Value, OrgId).ToList();
            //var warehouseStock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(OrgId);
            //var items = _itemBusiness.GetAllItemByOrgId(OrgId).ToList();

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
             var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId.Value, OrgId).ToList();
            var warehouseStock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(OrgId);
            var items = _itemBusiness.GetAllItemByOrgId(OrgId).ToList();
            stateWithText.isSuccess = true;
            foreach (var item in reqDetail)
            {
                var w = warehouseStock.Where(wr => wr.ItemId == item.ItemId).FirstOrDefault();
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

        #region Dropdown List
        [HttpPost]
        public ActionResult GetItemsByLine(long lineId)
        {
           var data = _itemBusiness.GetAllItemsInProductionStockByLineId(lineId, OrgId).Select(i => new Dropdown
            {
                value = i.ItemId.ToString(),
                text = i.ItemName
            }).ToList();

            return Json(data);
        }

        [HttpPost]
        public ActionResult GetItemTypeForDDL(long warehouseId)
        {
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId).AsEnumerable();
            var dropDown = itemTypes.Where(i => i.WarehouseId == warehouseId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost]
        public ActionResult GetItemForDDL(long itemTypeId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(OrgId).AsEnumerable();
            var dropDown = items.Where(i => i.ItemTypeId == itemTypeId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost]
        public ActionResult GetRolesByOrgId(long orgId)
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>();
            if(orgId > 0)
            {
                dropdowns=_roleBusiness.GetAllRoleByOrgId(orgId).Select(r => new Dropdown {text= r.RoleName,value=r.RoleId.ToString() }).ToList();
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
            var data =_itemBusiness.GetItemsByWarehouseId(warehouseId, OrgId).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetWarehouseByProductionLineId(long lineId)
        {
            var data = _warehouseBusiness.GetAllWarehouseByProductionLineId(OrgId, lineId).Select(w=> new Dropdown {
                text = w.WarehouseName,
                value = w.Id.ToString()
            }).ToList();
            return Json(data);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}