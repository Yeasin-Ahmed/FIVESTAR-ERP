using ERPBO.Common;
using ERPBO.ControlPanel.DTOModels;
using ERPBO.ControlPanel.ViewModels;
using ERPWeb.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        [NonAction]
        public UserPrivilege UserPrivilege(string controllerName, string ActionName)
        {
            UserPrivilege privilege = new UserPrivilege();
            var data = (List<UserAuthorizeMenusViewModels>)Session["UserAuthorizeMenus"];
            privilege = data.Where(d => d.ControllerName == controllerName && d.ActionName == ActionName).Select(d => new UserPrivilege
            {
                Add = d.Add,
                Edit = d.Edit,
                Detail = d.Detail,
                Approval = d.Approval,
                Delete = d.Delete,
                Report = d.Report
            }).FirstOrDefault();
            return privilege;
        }

        [NonAction]
        public AppUserDTO UserForEachRecord(long userId)
        {
            AppUserDTO entityUser = new AppUserDTO();
            var data = (List<AppUserDTO>)Session["UserList"];
            entityUser = data.FirstOrDefault(u => u.UserId == userId);
            return entityUser;
        }
    }
}