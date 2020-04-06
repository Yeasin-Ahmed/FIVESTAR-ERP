using ERPBLL.ControlPanel.Interface;
using ERPBO.ControlPanel.DTOModels;
using ERPBO.ControlPanel.ViewModels;
using ERPWeb.Filters;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class ControlPanelController : Controller
    {
        private readonly IOrganizationBusiness _organizationBusiness;
        private readonly IBranchBusiness _branchBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IAppUserBusiness _appUserBusiness;
        private readonly IModuleBusiness _moduleBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;

        public ControlPanelController(IOrganizationBusiness organizationBusiness, IBranchBusiness branchBusiness, IRoleBusiness roleBusiness,IAppUserBusiness appUserBusiness, IModuleBusiness moduleBusiness)
        {
            this._organizationBusiness = organizationBusiness;
            this._branchBusiness = branchBusiness;
            this._roleBusiness = roleBusiness;
            this._appUserBusiness = appUserBusiness;
            this._moduleBusiness = moduleBusiness;
        }
        // GET: ControlPanel
        [HttpGet]
        public ActionResult GetOrganizations()
        {
            IEnumerable<OrganizationDTO> organizationDTOs = _organizationBusiness.GetAllOrganizations().Select(org => new OrganizationDTO
            {
                OrganizationName = org.OrganizationName,
                ShortName = org.ShortName,
                Address = org.Address,
                PhoneNumber = org.PhoneNumber,
                MobileNumber = org.MobileNumber,
                Website = org.Website,
                Email = org.Email,
                Fax = org.Fax,
                OrgLogoPath = org.OrgLogoPath,
                ReportLogoPath = org.ReportLogoPath,
                IsActive = org.IsActive,
                StateStatus = org.IsActive ? "Active" : "Inactive",
                EUserId = org.EUserId,
                EntryDate = org.EntryDate
            }).ToList();

            List<OrganizationViewModel> OrganizationViewModels = new List<OrganizationViewModel>();
            AutoMapper.Mapper.Map(organizationDTOs, OrganizationViewModels);
            return View(OrganizationViewModels);
        }

        [HttpGet]                                                
        public ActionResult CreateOrganization(long? oId)
        {
            OrganizationViewModel organizationViewModel = new OrganizationViewModel();
            string pageTitle = string.Empty;
            pageTitle = "Create New Organization";
            if (oId != null && oId > 0)
            {
                var Org = _organizationBusiness.GetOrganizationById(oId.Value);
                organizationViewModel.OrgId = Org.OrganizationId;
                organizationViewModel.OrganizationName = Org.OrganizationName;
                organizationViewModel.Address = Org.Address;
                organizationViewModel.PhoneNumber = Org.PhoneNumber;
                organizationViewModel.MobileNumber = Org.MobileNumber;
                organizationViewModel.Email = Org.Email;
                organizationViewModel.Website = Org.Website;
                organizationViewModel.IsActive = Org.IsActive;
                organizationViewModel.Fax = Org.Fax;
                organizationViewModel.ShortName = Org.ShortName;
                pageTitle = "Update Organization";
            }
            ViewBag.PageTitle = pageTitle;
            return View(organizationViewModel);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveOrganization(OrganizationViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                OrganizationDTO dto = new OrganizationDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _organizationBusiness.SaveOrganization(dto,UserId);
            }
            return Json(IsSuccess);
        }

        #region Branch
        public ActionResult GetBranchList(int? page)
        {
            ViewBag.ddlOrganizationName = _organizationBusiness.GetAllOrganizations().Select(br => new SelectListItem { Text = br.OrganizationName, Value = br.OrganizationId.ToString() });

            IPagedList<BranchViewModel> branchViewModels = _branchBusiness.GetBranchByOrgId(OrgId).Select(br => new BranchViewModel
            {
                BranchId = br.BranchId,
                BranchName = br.BranchName,
                ShortName = br.ShortName,
                MobileNo = br.MobileNo,
                Email = br.Email,
                PhoneNo = br.PhoneNo,
                Fax = br.Fax,
                StateStatus = (br.IsActive == true ? "Active" : "Inactive"),
                Remarks = br.Remarks,
                OrgId = br.OrganizationId,
                OrganizationName = (_organizationBusiness.GetOrganizationById(OrgId).OrganizationName),
            }).OrderBy(br => br.BranchId).ToPagedList(page ?? 1, 15);
            IEnumerable<BranchViewModel> branchViewModelForPage = new List<BranchViewModel>();
            return View(branchViewModels);
        }

        public ActionResult SaveBranch(BranchViewModel branchViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    BranchDTO dto = new BranchDTO();
                    AutoMapper.Mapper.Map(branchViewModel, dto);
                    isSuccess = _branchBusiness.SaveBranch(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Role
        public ActionResult GetRoleList(int? page)
        {
            ViewBag.ddlOrganizationName = _organizationBusiness.GetAllOrganizations().Select(br => new SelectListItem { Text = br.OrganizationName, Value = br.OrganizationId.ToString() });

            IPagedList<RoleViewModel> roleViewModels = _roleBusiness.GetAllRoleByOrgId(OrgId).Select(role => new RoleViewModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                OrganizationId = role.OrganizationId,
                OrganizationName = (_organizationBusiness.GetOrganizationById(OrgId).OrganizationName),
            }).OrderBy(role => role.RoleId).ToPagedList(page ?? 1, 15);
            IEnumerable<RoleViewModel> roleViewModelForPage = new List<RoleViewModel>();
            return View(roleViewModels);
        }

        public ActionResult SaveRole(RoleViewModel roleViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    RoleDTO dto = new RoleDTO();
                    AutoMapper.Mapper.Map(roleViewModel, dto);
                    isSuccess = _roleBusiness.SaveRole(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region AppUser
        public ActionResult GetAppUserList(int? page)
        {
            ViewBag.ddlOrganizationName = _organizationBusiness.GetAllOrganizations().Select(br => new SelectListItem { Text = br.OrganizationName, Value = br.OrganizationId.ToString() });

            ViewBag.ddlRoleName = _roleBusiness.GetAllRoleByOrgId(OrgId).Select(br => new SelectListItem { Text = br.RoleName, Value = br.RoleId.ToString() });

            ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(OrgId).Select(br => new SelectListItem { Text = br.BranchName, Value = br.BranchId.ToString() });

            IPagedList<AppUserViewModel> appUserViewModels = _appUserBusiness.GetAllAppUserByOrgId(OrgId).Select(user => new AppUserViewModel
            {
             UserId=user.UserId,
            EmployeeId = user.EmployeeId,
            FullName = user.FullName,
            MobileNo = user.MobileNo,
            Address = user.Address,
            Email = user.Email,
            Desigation = user.Desigation,
            UserName = user.UserName,
            Password = user.Password,
            ConfirmPassword = user.ConfirmPassword,
            StateStatus = (user.IsActive == true ? "Active" : "Inactive"),
            StateStatusRole = (user.IsRoleActive == true ? "Active" : "Inactive"),
            OrganizationId = user.OrganizationId,
            OrganizationName=(_organizationBusiness.GetOrganizationById(OrgId).OrganizationName),
            BranchId = user.BranchId,
            BranchName=(_branchBusiness.GetBranchOneByOrgId(user.BranchId,OrgId).BranchName),
            RoleId = user.RoleId,
            RoleName=(_roleBusiness.GetRoleOneById(user.RoleId,OrgId).RoleName),
            }).OrderBy(user => user.UserId).ToPagedList(page ?? 1, 15);
            IEnumerable<AppUserViewModel> appUserViewModelForPage = new List<AppUserViewModel>();
            return View(appUserViewModels);
        }
        public ActionResult SaveAppUser(AppUserViewModel appUserViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AppUserDTO dto = new AppUserDTO();
                    AutoMapper.Mapper.Map(appUserViewModel, dto);
                    isSuccess = _appUserBusiness.SaveAppUser(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Module
        public ActionResult GetModuleList()
        {
            IEnumerable<ModuleDTO> moduleDTO = _moduleBusiness.GetAllModules().Select(m => new ModuleDTO
            {
                MId = m.MId,
                ModuleName = m.ModuleName,
                IconName = m.IconName,
                IconColor = m.IconColor,
            }).ToList();
            List<ModuleViewModel> moduleViewModels = new List<ModuleViewModel>();
            AutoMapper.Mapper.Map(moduleDTO, moduleViewModels);
            return View(moduleViewModels);
        }

        public ActionResult SaveModule(ModuleViewModel moduleViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ModuleDTO dto = new ModuleDTO();
                    AutoMapper.Mapper.Map(moduleViewModel, dto);
                    isSuccess = _moduleBusiness.SaveModule(dto, UserId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion
    }
}