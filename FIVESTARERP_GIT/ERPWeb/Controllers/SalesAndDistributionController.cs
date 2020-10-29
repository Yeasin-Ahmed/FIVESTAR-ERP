using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.Common;
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
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IBrandBusiness _brandBusiness;
        private readonly IBrandCategoriesBusiness _brandCategoryBusiness;
        private readonly IModelBusiness _modelBusiness;
        private readonly IColorBusiness _colorBusiness;
        private readonly IModelColorBusiness _modelColorBusiness;
        #endregion
        public SalesAndDistributionController(IDealerBusiness dealerBusiness, IBTRCApprovedIMEIBusiness bTRCApprovedIMEIBusiness, IItemStockBusiness itemStockBusiness, ICategoryBusiness categoryBusiness, IBrandBusiness brandBusiness, IBrandCategoriesBusiness brandCategoryBusiness, IModelBusiness modelBusiness, IColorBusiness colorBusiness, IModelColorBusiness modelColorBusiness)
        {
            #region Sales & Distribution
            this._dealerBusiness = dealerBusiness;
            this._bTRCApprovedIMEIBusiness = bTRCApprovedIMEIBusiness;
            this._itemStockBusiness = itemStockBusiness;
            this._categoryBusiness = categoryBusiness;
            this._brandBusiness = brandBusiness;
            this._brandCategoryBusiness = brandCategoryBusiness;
            this._modelBusiness = modelBusiness;
            this._colorBusiness = colorBusiness;
            this._modelColorBusiness = modelColorBusiness;
            #endregion
        }

        public ActionResult WorkStartUp(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlCategories = new SelectList(_categoryBusiness.GetCategories(User.OrgId), "CategoryId", "CategoryName");
                ViewBag.ddlColors = new SelectList(_colorBusiness.GetColors(User.OrgId), "ColorId", "ColorName");
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag =="dealer")
            {
                var dto = this._dealerBusiness.GetDealers(User.OrgId).Select(s => new DealerDTO
                {
                    DealerId =s.DealerId,
                    DealerName = s.DealerName,
                    Address = s.Address,
                    TelephoneNo = s.TelephoneNo,
                    MobileNo = s.MobileNo,
                    Email = s.Email,
                    ContactPersonName = s.ContactPersonName,
                    ContactPersonMobile = s.ContactPersonMobile,
                    Remarks = s.Remarks,
                    IsActive = s.IsActive,
                    EUserId = s.EUserId,
                    EntryDate = DateTime.Now,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName
                }).ToList();
                IEnumerable<DealerViewModel> viewModels = new List<DealerViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_dealer",viewModels);
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
                    BrandId= s.BrandId,
                    BranchId = s.BranchId,
                    BrandName = s.BrandName,
                    Remarks = s.Remarks,
                    IsActive = s.IsActive,
                    EUserId = s.EUserId,
                    EntryDate = DateTime.Now,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    BrandAndCategories = _brandCategoryBusiness.GetBrandAndCategories(s.BrandId,s.OrganizationId)
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
                var dto = this._modelBusiness.GetModels(User.OrgId).Select(s => new ModelDTO
                {
                    ModelId = s.ModelId,
                    ModelName = s.ModelName,
                    Remarks = s.Remarks,
                    IsActive = s.IsActive,
                    EUserId = s.EUserId,
                    EntryDate = DateTime.Now,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    ModelColors = _modelColorBusiness.GetModelColorsByModel(s.ModelId, User.OrgId)
                }).ToList();
                IEnumerable<ModelViewModel> viewModels = new List<ModelViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_model", viewModels);
            }
            return View();
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveDealer(DealerViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                DealerDTO dto = new DealerDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess =_dealerBusiness.SaveDealer(dto,User.UserId,User.OrgId);
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
                IsSuccess = _categoryBusiness.SaveCategory(dto, User.UserId,User.BranchId, User.OrgId);
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
                IsSuccess =_brandBusiness.SaveBrand(brand, categories, User.OrgId, User.BranchId, User.UserId);
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
        public ActionResult SaveModel(ModelViewModel model,long [] colors)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                ModelDTO dto = new ModelDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _modelBusiness.SaveModel(dto, colors, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetBTRCIMEI(string flag,string status, long? warehouseId, long? itemTypeId, long? itemId, long? colorId, string imei, long modelId=0,string fromDate="",string toDate="")
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim()=="list")
            {
                var dto= _bTRCApprovedIMEIBusiness.GetBTRCApprovedIMEIs(User.OrgId, status, modelId, fromDate, toDate);
                List<BTRCApprovedIMEIViewModel> viewModels = new List<BTRCApprovedIMEIViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_BTRCIMEIList",viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "stock")
            {
                var dto = _itemStockBusiness.GetItemStocks(User.BranchId, User.OrgId, modelId, warehouseId, itemTypeId, itemId, colorId, status, imei, fromDate, toDate);
                IEnumerable<ItemStockViewModel> viewModels = new List<ItemStockViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_itemStock", viewModels);
            }
            return View();
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveBTRCApprovedIMEI(HttpPostedFileBase FileUpload)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            if(FileUpload != null)
            {
                if(FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
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

                    if(approvedIMEIList.Count > 0)
                    {
                        executionState= _bTRCApprovedIMEIBusiness.UploadIMEI(approvedIMEIList, User.UserId, User.OrgId);
                    }
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                }
            }
            return Json(executionState);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult IMEIProcess()
        {
            var rows = _itemStockBusiness.RunProcess(User.OrgId, User.UserId, User.BranchId);
            return Json(new { rows = rows });
        }
       
    }
}