using ERPBO.Inventory.DTOModels;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPBO.ControlPanel.ViewModels;
using ERPBLL.Common;
using ERPBLL.ControlPanel.Interface;
using System.Threading.Tasks;
using ERPBO.Common;
using ERPWeb.Filters;
using System.Web.Security;

namespace ERPWeb.Controllers
{
    public class AccessController:BaseController
    {
        private readonly IAppUserBusiness _appUserBusiness;
        private readonly IUserAuthorizationBusiness _userAuthorizationBusiness;
        private readonly ISubMenuBusiness _subMenuBusiness;
        public AccessController(IAppUserBusiness appUserBusiness, IUserAuthorizationBusiness userAuthorizationBusiness, ISubMenuBusiness subMenuBusiness)
        {
            this._appUserBusiness = appUserBusiness;
            this._userAuthorizationBusiness = userAuthorizationBusiness;
            this._subMenuBusiness = subMenuBusiness;
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(UserLogInViewModel loginModel, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                string password = Utility.Encrypt(loginModel.Password);
                loginModel.Password = password;
                var userInformation = await _appUserBusiness.GetUserInformation(loginModel);
                if(userInformation != null)
                {
                    if(userInformation.IsOrgActive == true)
                    {
                        if (userInformation.IsActive == true)
                        {
                            var submenus = _subMenuBusiness.GetAllSubMenu().Where(sub => sub.ParentSubMenuId == 0).ToList();
                            //SubMenuViewModel subMenuViewModel = new SubMenuViewModel();
                            //AutoMapper.Mapper.Map(submenus, subMenuViewModel);
                            Session["AllSubmenus"] = submenus;

                            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();

                            #region Serialize Object
                            serializeModel.UserId = userInformation.UserId;
                            serializeModel.UserName = userInformation.UserName;
                            serializeModel.FullName = userInformation.FullName;
                            serializeModel.Address = userInformation.Address;
                            serializeModel.MobileNo = userInformation.MobileNo;
                            serializeModel.EmployeeId = userInformation.EmployeeId;
                            serializeModel.Email = userInformation.Email;
                            serializeModel.Designation = userInformation.Designation;
                            serializeModel.IsUserActive = userInformation.IsActive;

                            serializeModel.OrgId = userInformation.OrganizationId;
                            serializeModel.OrgName = userInformation.OrganizationName;
                            serializeModel.OrgLogo = Utility.GetImage(userInformation.OrgLogoPath);
                            serializeModel.IsOrgActive = userInformation.IsOrgActive;
                            serializeModel.HeaderLogo = Utility.GetImage(userInformation.ReportLogoPath);

                            serializeModel.LogInTime = DateTime.Now;
                            serializeModel.MacID = "";

                            serializeModel.RoleId = userInformation.RoleId;
                            serializeModel.IsRoleActive = userInformation.IsRoleActive;
                            serializeModel.RoleName = userInformation.RoleName;

                            string[] LogoPaths = new string[2];

                            LogoPaths[0] = userInformation.OrgLogoPath == null ? "" : userInformation.OrgLogoPath;
                            LogoPaths[1] = userInformation.ReportLogoPath == null ? "" : userInformation.ReportLogoPath;

                            serializeModel.LogoPaths = LogoPaths;

                            string[] roleArray = new string[2];
                            roleArray[0] = userInformation.RoleName;
                            serializeModel.roles = roleArray; 
                            #endregion

                            Session["UserDetail"] = serializeModel;

                            if (userInformation.IsRoleActive == false)
                            {
                                // Custom Auth..
                                var userAuth =_userAuthorizationBusiness.GetUserAuthorizeMenus(userInformation.UserId, userInformation.OrganizationId);
                                IEnumerable<UserAuthorizeMenusViewModels> userCustomMenus = new List<UserAuthorizeMenusViewModels>();
                                AutoMapper.Mapper.Map(userAuth, userCustomMenus);
                                Session["UserAuthorizeMenus"] = userCustomMenus;
                            }
                            else
                            {
                                // Role Auth..
                            }

                            if(userInformation.RoleName == UserType.SystemAdmin)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "User");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Inactive User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your Organization is Inactive");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName/Password");
                }
            }
            return View(loginModel);
        }

        [CustomAuthorize]
        public ActionResult LogOut()
        {
            Session["UserDetail"] = null;
            Session["AllSubmenus"] = null;
            if (User.IsRoleActive)
            {
                Session["RoleAuthorizeMenus"] = null;
            }
            else
            {
                Session["UserAuthorizeMenus"] = null;
            }
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            System.Web.HttpContext.Current.Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn", "Access", null);
        }

        public ActionResult Token()
        {
            return PartialView("_token");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}