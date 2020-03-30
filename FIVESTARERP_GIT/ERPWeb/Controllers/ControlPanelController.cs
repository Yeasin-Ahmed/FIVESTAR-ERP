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
        private readonly long UserId = 1;
        private readonly long OrgId = 1;
        public ControlPanelController(IOrganizationBusiness organizationBusiness, IBranchBusiness branchBusiness, IRoleBusiness roleBusiness)
        {
            this._organizationBusiness = organizationBusiness;
            this._branchBusiness = branchBusiness;
            this._roleBusiness = roleBusiness;
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
    }
}