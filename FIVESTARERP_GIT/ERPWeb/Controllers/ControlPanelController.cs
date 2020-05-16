using ERPBLL.Common;
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
    [CustomAuthorize]
    public class ControlPanelController : BaseController
    {
        private readonly IOrganizationBusiness _organizationBusiness;
        private readonly IBranchBusiness _branchBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IAppUserBusiness _appUserBusiness;
        private readonly IModuleBusiness _moduleBusiness;
        private readonly IManiMenuBusiness _maniMenuBusiness;
        private readonly ISubMenuBusiness _subMenuBusiness;
        private readonly IOrganizationAuthBusiness _organizationAuthBusiness;
        private readonly IUserAuthorizationBusiness _userAuthorizationBusiness;
        private readonly IRoleAuthorizationBusiness _roleAuthorizationBusiness;

        public ControlPanelController(IOrganizationBusiness organizationBusiness, IBranchBusiness branchBusiness, IRoleBusiness roleBusiness, IAppUserBusiness appUserBusiness, IModuleBusiness moduleBusiness, IManiMenuBusiness maniMenuBusiness, ISubMenuBusiness subMenuBusiness, IOrganizationAuthBusiness organizationAuthBusiness, IUserAuthorizationBusiness userAuthorizationBusiness, IRoleAuthorizationBusiness roleAuthorizationBusiness)
        {
            this._organizationBusiness = organizationBusiness;
            this._branchBusiness = branchBusiness;
            this._roleBusiness = roleBusiness;
            this._appUserBusiness = appUserBusiness;
            this._moduleBusiness = moduleBusiness;
            this._maniMenuBusiness = maniMenuBusiness;
            this._subMenuBusiness = subMenuBusiness;
            this._organizationAuthBusiness= organizationAuthBusiness;
            this._userAuthorizationBusiness = userAuthorizationBusiness;
            this._roleAuthorizationBusiness = roleAuthorizationBusiness;
        }
        // GET: ControlPanel

        #region Organization
        [HttpGet]
        public ActionResult GetOrganizations()
        {
            IEnumerable<OrganizationDTO> organizationDTOs = _organizationBusiness.GetAllOrganizations().Select(org => new OrganizationDTO
            {
                OrgId =org.OrganizationId,
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
                organizationViewModel.OrgLogoPath = Org.OrgLogoPath;
                organizationViewModel.ReportLogoPath = Org.ReportLogoPath;
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
                IsSuccess = _organizationBusiness.SaveOrganization(dto, User.UserId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Branch
        public ActionResult GetBranchList(int? page)
        {
            ViewBag.ddlOrganizationName = _organizationBusiness.GetAllOrganizations().Select(br => new SelectListItem { Text = br.OrganizationName, Value = br.OrganizationId.ToString() });

            IPagedList<BranchViewModel> branchViewModels = _branchBusiness.GetBranchByOrgId(User.OrgId).Select(br => new BranchViewModel
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
                OrganizationName = (_organizationBusiness.GetOrganizationById(User.OrgId).OrganizationName),
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
                    isSuccess = _branchBusiness.SaveBranch(dto, User.UserId, User.OrgId);
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

            IPagedList<RoleViewModel> roleViewModels = _roleBusiness.GetAllRoleByOrgId(User.OrgId).Select(role => new RoleViewModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                OrganizationId = role.OrganizationId,
                OrganizationName = (_organizationBusiness.GetOrganizationById(role.OrganizationId).OrganizationName),
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
                    isSuccess = _roleBusiness.SaveRole(dto, User.UserId, User.OrgId);
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

            //ViewBag.ddlRoleName = _roleBusiness.GetAllRoleByOrgId(User.OrgId).Select(br => new SelectListItem { Text = br.RoleName, Value = br.RoleId.ToString() });

            //ViewBag.ddlBranchName = _branchBusiness.GetBranchByOrgId(User.OrgId).Select(br => new SelectListItem { Text = br.BranchName, Value = br.BranchId.ToString() });

            IEnumerable<AppUserViewModel> appUserViewModels = _appUserBusiness.GetAllAppUserByOrgId(User.OrgId).Select(user => new AppUserViewModel
            {
                UserId = user.UserId,
                EmployeeId = user.EmployeeId,
                FullName = user.FullName,
                MobileNo = user.MobileNo,
                Address = user.Address,
                Email = user.Email,
                Desigation = user.Desigation,
                UserName = user.UserName,
                Password = Utility.Decrypt(user.Password),
                StateStatus = (user.IsActive == true ? "Active" : "Inactive"),
                StateStatusRole = (user.IsRoleActive == true ? "Active" : "Inactive"),
                OrganizationId = user.OrganizationId,
                OrganizationName = (_organizationBusiness.GetOrganizationById(user.OrganizationId).OrganizationName),
                BranchId = user.BranchId,
                BranchName = (_branchBusiness.GetBranchOneByOrgId(user.BranchId, user.OrganizationId).BranchName),
                RoleId = user.RoleId,
                RoleName = (_roleBusiness.GetRoleOneById(user.RoleId, user.OrganizationId).RoleName),
            }).OrderBy(user => user.UserId).ToList();
            return View(appUserViewModels);
        }
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveAppUser(AppUserViewModel appUserViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AppUserDTO dto = new AppUserDTO();
                    appUserViewModel.Password = Utility.Encrypt(appUserViewModel.Password);
                    AutoMapper.Mapper.Map(appUserViewModel, dto);
                    isSuccess = _appUserBusiness.SaveAppUser(dto, User.UserId, User.OrgId);
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
                    isSuccess = _moduleBusiness.SaveModule(dto, User.UserId);
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
                MenuName = mainmenu.MenuName,
                MId = mainmenu.MId,
                ModuleName = (_moduleBusiness.GetModuleOneById(mainmenu.MId).ModuleName),
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
                    isSuccess = _maniMenuBusiness.SaveMainMenu(dto, User.UserId);
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

            ViewBag.ddlParentSubMenu = _subMenuBusiness.GetAllSubMenu().Where(s => s.IsActAsParent == true).Select(s => new SelectListItem
            {

                Text = s.SubMenuName + ((s.MMId > 0) ? " (" + (_maniMenuBusiness.GetMainMenuOneById(s.MMId).MenuName) + ")" : ""),
                Value = s.SubMenuId.ToString()

            });

            IEnumerable<SubMenuDTO> subMenuDTO = _subMenuBusiness.GetAllSubMenu().Select(sub => new SubMenuDTO
            {
                SubMenuId = sub.SubMenuId,
                SubMenuName = sub.SubMenuName,
                ControllerName = sub.ControllerName,
                ActionName = sub.ActionName,
                IconClass = sub.IconClass,
                IsViewable = sub.IsViewable,
                IsActAsParent = sub.IsActAsParent,
                IsViewableStatus = (sub.IsViewable == true ? "Yes" : "No"),
                IsActAsParentStatus = (sub.IsActAsParent == true ? "Yes" : "No"),
                ParentSubMenuId = sub.ParentSubMenuId,
                ParentSubmenuName = (sub.ParentSubMenuId > 0 ? _subMenuBusiness.GetSubMenuOneById(sub.ParentSubMenuId.Value).SubMenuName : ""),
                MMId = sub.MMId,
                MenuName = (_maniMenuBusiness.GetMainMenuOneById(sub.MMId).MenuName),
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
                    isSuccess = _subMenuBusiness.SaveSubMenu(dto, User.UserId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Orgainzation Auth
        public ActionResult GetMainMenusForOrgAuth(string flag,long? orgId)
        {
            if (string.IsNullOrEmpty(flag))
            {
                var allmainmenus = _organizationAuthBusiness.GetMainMenusForOrgAuth();
                List<ModuleUIViewModel> viewModels = new List<ModuleUIViewModel>();
                var modules = allmainmenus.Select(s => s.MId).Distinct().ToList();
                foreach (var item in modules)
                {
                    ModuleUIViewModel module = new ModuleUIViewModel();
                    var mainmenu = allmainmenus.Where(m => m.MId == item).ToList();
                    if (mainmenu.Count > 0)
                    {
                        module.MId = item;
                        module.ModuleName = mainmenu.FirstOrDefault().ModuleName;
                        List<MainMenuUIVIewModel> mainMenuUIVIews = new List<MainMenuUIVIewModel>();
                        foreach (var menu in mainmenu)
                        {
                            MainMenuUIVIewModel m = new MainMenuUIVIewModel();
                            m.MMId = menu.MMId;
                            m.MainmenuName = menu.MenuName;
                            mainMenuUIVIews.Add(m);
                        }
                        module.MainMenus = mainMenuUIVIews;
                    }
                    viewModels.Add(module);
                }

                ViewBag.ddlOrganization = _organizationBusiness.GetAllOrganizations().Select(s => new SelectListItem
                {
                    Text = s.OrganizationName,
                    Value = s.OrganizationId.ToString()
                }).ToList();
                return View(viewModels);
            }
            else
            {
                var allmainmenusByOrg = _organizationAuthBusiness.GetMainMenusForOrgAuthById(orgId.Value);
                return Json(allmainmenusByOrg);
            }
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveOrganizationAuthMenus(OrgAuthMenusViewModels viewModels)
        {
            bool IsSuccess = false;
            if(ModelState.IsValid && viewModels.Menus.Count > 0)
            {
                OrgAuthMenusDTO dto = new OrgAuthMenusDTO();
                AutoMapper.Mapper.Map(viewModels, dto);
                IsSuccess = this._organizationAuthBusiness.SaveOrganizationAuthMenus(dto, User.UserId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region User Custom Authorization
        public ActionResult SetUserCustomAuthorization(string flag, long? userId, long? orgId)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlOrganization = _organizationBusiness.GetAllOrganizations().Select(o => new SelectListItem
                {
                    Text = o.OrganizationName,
                    Value = o.OrganizationId.ToString()
                }).ToList();
                return View();
            }
            else
            {
                List<VmUserModule> listVmUserModule = new List<VmUserModule>();
                IEnumerable<UserCustomMenusDTO> userCustomMenus = _userAuthorizationBusiness.GetUserCustomMenus(userId.Value, orgId.Value);
                var userDto = _appUserBusiness.GetUserDetail(userId.Value, orgId.Value);
                UserDetaildViewModel userDetaildViewModel = new UserDetaildViewModel();
                AutoMapper.Mapper.Map(userDto, userDetaildViewModel);
                if (userCustomMenus.Count() > 0)
                {
                    var modules = (from m in userCustomMenus
                                   select new { MId = m.ModuleId, ModuleName = m.ModuleName }).Distinct().ToList();
                    foreach (var item in modules)
                    {
                        VmUserModule vmUserModule = new VmUserModule();
                        vmUserModule.ModuleId = item.MId;
                        vmUserModule.ModuleName = item.ModuleName;
                        List<VmUserMenu> vmUserMenus = new List<VmUserMenu>();
                        var mainmenu = (from m in userCustomMenus
                                        where m.ModuleId == item.MId
                                        select new { MenuId = m.MainmenuId, MenuName = m.MainmenuName }).Distinct().ToList();
                        foreach (var mm in mainmenu)
                        {
                            VmUserMenu vmUserMenu = new VmUserMenu();
                            vmUserMenu.MenuId = mm.MenuId;
                            vmUserMenu.MenuName = mm.MenuName;
                            vmUserMenu.SubMenus = userCustomMenus.Where(s => s.MainmenuId == mm.MenuId).Select(s => new VmUserSubmenu
                            {
                                SubmenuId = s.SubmenuId,
                                SubMenuName = s.SubMenuName,
                                ParentSubMenuId = s.ParentSubMenuId,
                                Add = s.Add,
                                Edit = s.Edit,
                                Detail = s.Detail,
                                Delete = s.Delete,
                                Approval = s.Approval,
                                Report = s.Report,
                                TaskId = s.TaskId
                            }).ToList();
                            vmUserMenus.Add(vmUserMenu);
                        }

                        vmUserModule.Menus = vmUserMenus;
                        listVmUserModule.Add(vmUserModule);
                    }
                }
                return Json(new { userDetail = userDetaildViewModel, menuDetail = listVmUserModule });
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveUserAuthorization(List<UserAuthorizationViewModel> models)
        {
            bool IsSuccess = false;
            if (models.Count > 0)
            {
                List<UserAuthorizationDTO> userAuthorizationDTOs = new List<UserAuthorizationDTO>();
                AutoMapper.Mapper.Map(models, userAuthorizationDTOs);
                IsSuccess = _userAuthorizationBusiness.SaveUserAuthorization(userAuthorizationDTOs, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region User Role Authorization
        public ActionResult SetUserRoleAuthorization(string flag, long? roleId, long? orgId)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlOrganization = _organizationBusiness.GetAllOrganizations().Select(o => new SelectListItem
                {
                    Text = o.OrganizationName,
                    Value = o.OrganizationId.ToString()
                }).ToList();
                return View();
            }
            else
            {
                List<VmUserModule> listVmUserModule = new List<VmUserModule>();
                IEnumerable<UserCustomMenusDTO> userCustomMenus = _roleAuthorizationBusiness.GetUserRoleMenus(roleId.Value, orgId.Value);
                if (userCustomMenus.Count() > 0)
                {
                    var modules = (from m in userCustomMenus
                                   select new { MId = m.ModuleId, ModuleName = m.ModuleName }).Distinct().ToList();
                    foreach (var item in modules)
                    {
                        VmUserModule vmUserModule = new VmUserModule();
                        vmUserModule.ModuleId = item.MId;
                        vmUserModule.ModuleName = item.ModuleName;
                        List<VmUserMenu> vmUserMenus = new List<VmUserMenu>();
                        var mainmenu = (from m in userCustomMenus
                                        where m.ModuleId == item.MId
                                        select new { MenuId = m.MainmenuId, MenuName = m.MainmenuName }).Distinct().ToList();
                        foreach (var mm in mainmenu)
                        {
                            VmUserMenu vmUserMenu = new VmUserMenu();
                            vmUserMenu.MenuId = mm.MenuId;
                            vmUserMenu.MenuName = mm.MenuName;
                            vmUserMenu.SubMenus = userCustomMenus.Where(s => s.MainmenuId == mm.MenuId).Select(s => new VmUserSubmenu
                            {
                                SubmenuId = s.SubmenuId,
                                SubMenuName = s.SubMenuName,
                                ParentSubMenuId = s.ParentSubMenuId,
                                Add = s.Add,
                                Edit = s.Edit,
                                Detail = s.Detail,
                                Delete = s.Delete,
                                Approval = s.Approval,
                                Report = s.Report,
                                TaskId = s.TaskId
                            }).ToList();
                            vmUserMenus.Add(vmUserMenu);
                        }

                        vmUserModule.Menus = vmUserMenus;
                        listVmUserModule.Add(vmUserModule);
                    }
                }
                return Json(new {menuDetail = listVmUserModule });
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRoleAuthorization(List<RoleAuthorizationViewModel> models)
        {
            bool IsSuccess = false;
            if (models.Count > 0)
            {
                List<RoleAuthorizationDTO> roleAuthorizationDTOs = new List<RoleAuthorizationDTO>();
                AutoMapper.Mapper.Map(models, roleAuthorizationDTOs);
                IsSuccess = _roleAuthorizationBusiness.SaveRoleAuthorization(roleAuthorizationDTOs, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion


        #region Used By Client
        public ActionResult GetClientUsers()
        {
            IEnumerable<AppUserDTO> appUserDto = _appUserBusiness.GetAllAppUserByOrgId(User.OrgId).Select(user => new AppUserDTO
            {
                UserId = user.UserId,
                EmployeeId = user.EmployeeId,
                FullName = user.FullName,
                MobileNo = user.MobileNo,
                Address = user.Address,
                Email = user.Email,
                Desigation = user.Desigation,
                UserName = user.UserName,
                Password = Utility.Decrypt(user.Password),
                StateStatus = (user.IsActive == true ? "Active" : "Inactive"),
                StateStatusRole = (user.IsRoleActive == true ? "Active" : "Inactive"),
                OrganizationId = user.OrganizationId,
                OrganizationName = (_organizationBusiness.GetOrganizationById(User.OrgId).OrganizationName),
                BranchId = user.BranchId,
                BranchName = (_branchBusiness.GetBranchOneByOrgId(user.BranchId, User.OrgId).BranchName),
                RoleId = user.RoleId,
                RoleName = (_roleBusiness.GetRoleOneById(user.RoleId, User.OrgId).RoleName),
            }).OrderBy(user => user.UserId).ToList();
            IEnumerable<AppUserViewModel> appUserViewModels = new List<AppUserViewModel>();
            AutoMapper.Mapper.Map(appUserDto, appUserViewModels);
            return View(appUserViewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveClientUser(AppUserViewModel appUserViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AppUserDTO dto = new AppUserDTO();
                    appUserViewModel.OrganizationId = User.OrgId;
                    appUserViewModel.Password = Utility.Encrypt(appUserViewModel.Password);
                    AutoMapper.Mapper.Map(appUserViewModel, dto);
                    isSuccess = _appUserBusiness.SaveAppUser(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}