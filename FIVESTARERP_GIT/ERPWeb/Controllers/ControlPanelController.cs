﻿using ERPBLL.ControlPanel.Interface;
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
        private readonly IManiMenuBusiness _maniMenuBusiness;
        private readonly ISubMenuBusiness _subMenuBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;

        public ControlPanelController(IOrganizationBusiness organizationBusiness, IBranchBusiness branchBusiness, IRoleBusiness roleBusiness,IAppUserBusiness appUserBusiness, IModuleBusiness moduleBusiness,IManiMenuBusiness maniMenuBusiness,ISubMenuBusiness subMenuBusiness)
        {
            this._organizationBusiness = organizationBusiness;
            this._branchBusiness = branchBusiness;
            this._roleBusiness = roleBusiness;
            this._appUserBusiness = appUserBusiness;
            this._moduleBusiness = moduleBusiness;
            this._maniMenuBusiness = maniMenuBusiness;
            this._subMenuBusiness = subMenuBusiness;
        }
        // GET: ControlPanel
        #region Organization
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

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveOrganization(OrganizationViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                OrganizationDTO dto = new OrganizationDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _organizationBusiness.SaveOrganization(dto, UserId);
            }
            return Json(IsSuccess);
        }
        #endregion

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

        #region MainMenu
        public ActionResult GetMainMenuList()
        {
            ViewBag.ddlModuleName = _moduleBusiness.GetAllModules().Select(br => new SelectListItem { Text = br.ModuleName, Value = br.MId.ToString() });

            IEnumerable<MainMenuDTO> mainMenuDTO = _maniMenuBusiness.GetAllMainMenu().Select(mainmenu => new MainMenuDTO
            {
                MMId = mainmenu.MMId,
                MenuName=mainmenu.MenuName,
                MId=mainmenu.MId,
                ModuleName=(_moduleBusiness.GetModuleOneById(mainmenu.MId).ModuleName),
            }).ToList();
            IEnumerable<MainMenuViewModel> mainMenuViewModels = new List<MainMenuViewModel>();
            AutoMapper.Mapper.Map(mainMenuDTO, mainMenuViewModels);
            return View(mainMenuViewModels);
        }
        public ActionResult SaveMainMenu(MainMenuViewModel mainMenuViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    MainMenuDTO dto = new MainMenuDTO();
                    AutoMapper.Mapper.Map(mainMenuViewModel, dto);
                    isSuccess = _maniMenuBusiness.SaveMainMenu(dto, UserId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region SubMenu
        public ActionResult GetSubMenuList()
        {
            ViewBag.ddlMainMenu = _maniMenuBusiness.GetAllMainMenu().Select(br => new SelectListItem { Text = br.MenuName, Value = br.MMId.ToString() });

            ViewBag.ddlParentSubMenu = _subMenuBusiness.GetAllSubMenu().Select(br => new SelectListItem { Text = br.SubMenuName, Value = br.SubMenuName.ToString() });

            IEnumerable<SubMenuDTO> subMenuDTO = _subMenuBusiness.GetAllSubMenu().Select(sub => new SubMenuDTO
            {
                SubMenuId=sub.SubMenuId,
                SubMenuName=sub.SubMenuName,
                ControllerName=sub.ControllerName,
                ActionName=sub.ActionName,
                IconClass=sub.IconClass,
                IsViewableStatus= (sub.IsViewable == true ? "Active" : "Inactive"),
                IsActAsParentStatus = (sub.IsActAsParent == true ? "Active" : "Inactive"),
                ParentSubMenuId = sub.ParentSubMenuId,
                MMId =sub.MMId,
                MenuName=(_maniMenuBusiness.GetMainMenuOneById(sub.MMId).MenuName),
            }).ToList();
            IEnumerable<SubMenuViewModel> subMenuViewModels = new List<SubMenuViewModel>();
            AutoMapper.Mapper.Map(subMenuDTO, subMenuViewModels);
            return View(subMenuViewModels);
        }
        public ActionResult SaveSubMenu(SubMenuViewModel subMenuViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    SubMenuDTO dto = new SubMenuDTO();
                    AutoMapper.Mapper.Map(subMenuViewModel, dto);
                    isSuccess = _subMenuBusiness.SaveSubMenu(dto, UserId);
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