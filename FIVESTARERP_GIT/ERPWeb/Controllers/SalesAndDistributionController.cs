using ERPBLL.Common;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.Common;
using ERPBO.SalesAndDistribution.CommonModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPBO.SalesAndDistribution.ViewModels;
using ERPWeb.Filters;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class SalesAndDistributionController : BaseController
    {
        #region Sale & Distribution
        private readonly IDealerBusiness _dealerBusiness;
        private readonly IBTRCApprovedIMEIBusiness _bTRCApprovedIMEIBusiness;
        private readonly IItemStockBusiness _itemStockBusiness;

        /// <summary>
        /// Sales & Distribution's own category,brand,model,color object..
        /// these will be needed when this module does not have any relation with *Inventory Module*
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IBrandBusiness _brandBusiness;
        private readonly IBrandCategoriesBusiness _brandCategoryBusiness;
        private readonly IModelBusiness _modelBusiness;
        private readonly IColorBusiness _colorBusiness;
        private readonly IModelColorBusiness _modelColorBusiness;
        private readonly IDivisionBusiness _divisionBusiness;
        private readonly IDistrictBusiness _districtBusiness;
        private readonly IZoneBusiness _zoneBusiness;
        private readonly IRSMBusiness _rSMBusiness;
        private readonly IASMBusiness _aSMBusiness;
        private readonly ITSEBusiness _tSEBusiness;
        #endregion

        #region Inventory
        private readonly ERPBLL.Inventory.Interface.ICategoryBusiness _invCategoryBusiness;
        private readonly ERPBLL.Inventory.Interface.IBrandBusiness _invBrandBusiness;
        private readonly ERPBLL.Inventory.Interface.IBrandCategoriesBusiness _invBrandCategoryBusiness;
        private readonly ERPBLL.Inventory.Interface.IDescriptionBusiness _invModelBusiness;
        private readonly ERPBLL.Inventory.Interface.IColorBusiness _invColorBusiness;
        #endregion
        public SalesAndDistributionController(IDealerBusiness dealerBusiness, IBTRCApprovedIMEIBusiness bTRCApprovedIMEIBusiness, IItemStockBusiness itemStockBusiness, ERPBLL.Inventory.Interface.ICategoryBusiness invCategoryBusiness, ERPBLL.Inventory.Interface.IBrandBusiness invBrandBusiness, ERPBLL.Inventory.Interface.IBrandCategoriesBusiness invBrandCategoryBusiness, ERPBLL.Inventory.Interface.IDescriptionBusiness invModelBusiness, ERPBLL.Inventory.Interface.IColorBusiness invColorBusiness, ICategoryBusiness categoryBusiness, IBrandBusiness brandBusiness, IBrandCategoriesBusiness brandCategoryBusiness, IModelBusiness modelBusiness, IColorBusiness colorBusiness, IModelColorBusiness modelColorBusiness, IDivisionBusiness divisionBusiness, IDistrictBusiness districtBusiness, IZoneBusiness zoneBusiness, IRSMBusiness rSMBusiness, IASMBusiness aSMBusiness, ITSEBusiness tSEBusiness)
        {
            #region Sales & Distribution
            this._dealerBusiness = dealerBusiness;
            this._bTRCApprovedIMEIBusiness = bTRCApprovedIMEIBusiness;
            this._itemStockBusiness = itemStockBusiness;

            //ICategoryBusiness categoryBusiness, IBrandBusiness brandBusiness, IBrandCategoriesBusiness brandCategoryBusiness, IModelBusiness modelBusiness, IColorBusiness colorBusiness, IModelColorBusiness modelColorBusiness
            this._categoryBusiness = categoryBusiness;
            this._brandBusiness = brandBusiness;
            this._brandCategoryBusiness = brandCategoryBusiness;
            this._modelBusiness = modelBusiness;
            this._colorBusiness = colorBusiness;
            this._modelColorBusiness = modelColorBusiness;
            this._divisionBusiness = divisionBusiness;
            this._districtBusiness = districtBusiness;
            this._zoneBusiness = zoneBusiness;
            this._rSMBusiness = rSMBusiness;
            this._aSMBusiness = aSMBusiness;
            this._tSEBusiness = tSEBusiness;
            #endregion

            #region Inventory
            this._invCategoryBusiness = invCategoryBusiness;
            this._invBrandBusiness = invBrandBusiness;
            this._invBrandCategoryBusiness = invBrandCategoryBusiness;
            this._invModelBusiness = invModelBusiness;
            this._invColorBusiness = invColorBusiness;
            #endregion
        }

        public ActionResult WorkStartUp(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlCategories = new SelectList(_categoryBusiness.GetCategories(User.OrgId), "CategoryId", "CategoryName");
                ViewBag.ddlColors = new SelectList(_colorBusiness.GetColors(User.OrgId), "ColorId", "ColorName");
                ViewBag.ddlBrand = new SelectList(_brandBusiness.GetBrands(User.OrgId), "BrandId", "BrandName");
                ViewBag.ddlDivision = new SelectList(_divisionBusiness.GetDivisions(User.OrgId), "DivisionId", "DivisionName");
                ViewBag.ddlDistrictWithDivision = new SelectList(_districtBusiness.GetDistrictWithDivision(User.OrgId), "value", "text");
                ViewBag.ddlDealerRepresentative = new SelectList(_dealerBusiness.GetDealerRepresentatives(User.OrgId), "value", "text");
                ViewBag.ddlZoneWithDistrictAndDivision = new SelectList(_zoneBusiness.GetZoneWithDistrictAndDivision(User.OrgId), "value", "text");
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "dealer")
            {
                var dto = _dealerBusiness.GetDealerInformations(User.OrgId);
                IEnumerable<DealerViewModel> viewModels = new List<DealerViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_dealer", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "category")
            {
                var dto = this._categoryBusiness.GetCategories(User.OrgId).Select(s => new CategoryDTO
                {
                    CategoryId = s.CategoryId,
                    CategoryName = s.CategoryName,
                    Remarks = s.Remarks,
                    IsActive = s.IsActive,
                    EUserId = s.EUserId,
                    EntryDate = DateTime.Now,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName
                }).ToList();
                IEnumerable<CategoryViewModel> viewModels = new List<CategoryViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_category", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "brand")
            {
                var dto = this._brandBusiness.GetBrands(User.OrgId).Select(s => new BrandDTO
                {
                    BrandId = s.BrandId,
                    BranchId = s.BranchId,
                    BrandName = s.BrandName,
                    Remarks = s.Remarks,
                    IsActive = s.IsActive,
                    EUserId = s.EUserId,
                    EntryDate = DateTime.Now,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    BrandAndCategories = _brandCategoryBusiness.GetBrandAndCategories(s.BrandId, s.OrganizationId)
                }).ToList();
                IEnumerable<BrandViewModel> viewModels = new List<BrandViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_brand", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "color")
            {
                var dto = this._colorBusiness.GetColors(User.OrgId).Select(s => new ColorDTO
                {
                    ColorId = s.ColorId,
                    ColorName = s.ColorName,
                    Remarks = s.Remarks,
                    IsActive = s.IsActive,
                    EUserId = s.EUserId,
                    EntryDate = DateTime.Now,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName
                }).ToList();
                IEnumerable<ColorViewModel> viewModels = new List<ColorViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_color", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "model")
            {
                var dto = this._modelBusiness.GetModels(User.OrgId).Select(s => new DescriptionDTO
                {
                    DescriptionId = s.DescriptionId,
                    DescriptionName = s.DescriptionName,
                    Remarks = s.Remarks,
                    IsActive = s.IsActive,
                    EUserId = s.EUserId,
                    EntryDate = DateTime.Now,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    CategoryId = s.CategoryId,
                    CategoryName = (s.CategoryId == null ? "" : _categoryBusiness.GetCategoryById(s.CategoryId.Value, User.OrgId).CategoryName),
                    BrandId = s.BrandId,
                    BrandName = ((s.BrandId == null || s.BrandId == 0) ? "" : _brandBusiness.GetBrandById(s.BrandId.Value, User.OrgId).BrandName),
                    ModelColors = _modelColorBusiness.GetModelColorsByModel(s.DescriptionId, User.OrgId)
                }).ToList();
                IEnumerable<DescriptionViewModel> viewModels = new List<DescriptionViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_model", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "division")
            {
                var dto = _divisionBusiness.GetDivisions(User.OrgId).Select(s => new DivisionDTO()
                {
                    DivisionId = s.DivisionId,
                    DivisionName = s.DivisionName,
                    OrganizationId = s.OrganizationId,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    IsActive = s.IsActive,
                    Remarks = s.Remarks
                }).ToList();
                IEnumerable<DivisionViewModel> viewModels = new List<DivisionViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_division", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "district")
            {
                var dto = _districtBusiness.GetDistricts(User.OrgId).Select(s => new DistrictDTO()
                {
                    DistrictId = s.DistrictId,
                    DistrictName = s.DistrictName,
                    DivisionId = s.DivisionId,
                    DivisionName = _divisionBusiness.GetDivisionById(s.DivisionId,User.OrgId).DivisionName,
                    OrganizationId = s.OrganizationId,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    IsActive = s.IsActive,
                    Remarks = s.Remarks
                }).ToList();
                IEnumerable<DistrictViewModel> viewModels = new List<DistrictViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_district", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "zone")
            {
                var dto = _zoneBusiness.GetZones(User.OrgId).Select(s => new ZoneDTO()
                {
                    ZoneId = s.ZoneId,
                    ZoneName = s.ZoneName,
                    DistrictId = s.DistrictId,
                    DistrictName = _districtBusiness.GetDistrictById(s.DistrictId, User.OrgId).DistrictName,
                    DivisionId =s.DivisionId,
                    DivisionName = (s.DivisionId != null && s.DivisionId.Value > 0) ? _divisionBusiness.GetDivisionById(s.DivisionId.Value, User.OrgId).DivisionName : "",
                    OrganizationId = s.OrganizationId,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    IsActive = s.IsActive,
                    Remarks = s.Remarks
                }).ToList();
                IEnumerable<ZoneViewModel> viewModels = new List<ZoneViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_zone", viewModels);
            }
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveDealer(DealerViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                DealerDTO dto = new DealerDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _dealerBusiness.SaveDealer(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveCategory(CategoryViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                CategoryDTO dto = new CategoryDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _categoryBusiness.SaveCategory(dto, User.UserId, User.BranchId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveBrand(BrandViewModel model, long[] categories)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                BrandDTO brand = new BrandDTO();
                AutoMapper.Mapper.Map(model, brand);
                IsSuccess = _brandBusiness.SaveBrand(brand, categories, User.OrgId, User.BranchId, User.UserId);
            }

            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveColor(ColorViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                ColorDTO dto = new ColorDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _colorBusiness.SaveColor(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveModel(DescriptionViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                DescriptionDTO dto = new DescriptionDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _modelBusiness.SaveModel(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveDivision(DivisionViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                DivisionDTO dto = new DivisionDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _divisionBusiness.SaveDivision(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveDistrict(DistrictViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                DistrictDTO dto = new DistrictDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _districtBusiness.SaveDistrict(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveZone(ZoneViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                ZoneDTO dto = new ZoneDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _zoneBusiness.SaveZone(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetBTRCIMEI(string flag, string status, long? warehouseId, long? itemTypeId, long? itemId, long? colorId, string imei, long modelId = 0, string fromDate = "", string toDate = "")
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlCategories = new SelectList(_invCategoryBusiness.GetCategories(User.OrgId), "CategoryId", "CategoryName");
                ViewBag.ddlMobileBrand = new SelectList(_invBrandBusiness.GetBrands(User.OrgId), "BrandId", "BrandName");
                ViewBag.ddlModels = new SelectList(_invModelBusiness.GetDescriptionByOrgId(User.OrgId), "DescriptionId", "DescriptionName");
                ViewBag.ddlColors = new SelectList(_invColorBusiness.GetAllColorByOrgId(User.OrgId), "ColorId", "ColorName");
                ViewBag.ddlStockStatus = new SelectList(Utility.ListOfStockStatus(), "value", "text");
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "list")
            {
                var dto = _bTRCApprovedIMEIBusiness.GetBTRCApprovedIMEIs(User.OrgId, status, modelId, fromDate, toDate);
                List<BTRCApprovedIMEIViewModel> viewModels = new List<BTRCApprovedIMEIViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_BTRCIMEIList", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "stock")
            {
                //var dto = _itemStockBusiness.GetItemStocks(User.BranchId, User.OrgId, modelId, warehouseId, itemTypeId, itemId, colorId, status, imei, fromDate, toDate);
                IEnumerable<ItemStockViewModel> viewModels = new List<ItemStockViewModel>();
                //AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_itemStock", viewModels);
            }
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveBTRCApprovedIMEI(HttpPostedFileBase FileUpload)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
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
                    var approvedIMEIList = (from a in excelFile.Worksheet<ApprovedIMEI>(sheetName) select a).ToList();

                    if (approvedIMEIList.Count > 0)
                    {
                        executionState = _bTRCApprovedIMEIBusiness.UploadIMEI(approvedIMEIList, User.UserId, User.OrgId);
                    }
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                }
            }
            return Json(executionState);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IMEIProcess()
        {
            var rows = _itemStockBusiness.RunProcess(User.OrgId, User.UserId, User.BranchId);
            return Json(new { rows = rows });
        }

        public ActionResult SRHierarchy(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlDistrictWithDivision = new SelectList(_districtBusiness.GetDistrictWithDivision(User.OrgId), "value", "text");
                ViewBag.ddlRSM = new SelectList(_rSMBusiness.GetRSMByOrg(User.OrgId), "RSMID", "FullName");
                ViewBag.ddlZoneWithDistrictAndDivision = new SelectList(_zoneBusiness.GetZoneWithDistrictAndDivision(User.OrgId), "value", "text");
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() =="rsm")
            {
                var dto =_rSMBusiness.GetRSMInformations(User.OrgId,User.UserId);
                List<RSMViewModel> viewModels = new List<RSMViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_rsm", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "asm")
            {
                var dto = _aSMBusiness.GetASMInformations(User.OrgId,User.UserId);
                List<ASMViewModel> viewModels = new List<ASMViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_asm", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "tse")
            {
                var dto = _tSEBusiness.GetTSEInformations(User.OrgId, User.UserId);
                List<TSEViewModel> viewModels = new List<TSEViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_tse", viewModels);
            }
            return View();
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveRSM(RSMViewModel model, SRUser sRUser)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                RSMDTO dto = new RSMDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _rSMBusiness.SaveRSM(dto,sRUser,User.UserId,User.BranchId,User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveASM(ASMViewModel model, SRUser sRUser)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                ASMDTO dto = new ASMDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _aSMBusiness.SaveASM(dto, sRUser, User.UserId, User.BranchId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTSE(TSEViewModel model, SRUser sRUser)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                TSEDTO dto = new TSEDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _tSEBusiness.SaveTSE(dto, sRUser, User.UserId, User.BranchId, User.OrgId);
            }
            return Json(IsSuccess);
        }
    }
}