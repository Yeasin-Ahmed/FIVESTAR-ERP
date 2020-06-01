using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DTOModels;
using ERPBO.Configuration.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class ConfigurationController : BaseController
    {
        // GET: Configuration
        private readonly IAccessoriesBusiness _accessoriesBusiness;
        private readonly IClientProblemBusiness _clientProblemBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;

        public ConfigurationController(IAccessoriesBusiness accessoriesBusiness,IClientProblemBusiness clientProblemBusiness,IMobilePartBusiness mobilePartBusiness)
        {
            this._accessoriesBusiness = accessoriesBusiness;
            this._clientProblemBusiness = clientProblemBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
        }
        #region tblAccessories
        public ActionResult AccessoriesList()
        {
            IEnumerable<AccessoriesDTO> accessoriesDTO = _accessoriesBusiness.GetAllAccessoriesByOrgId(1).Select(access => new AccessoriesDTO
            {
                AccessoriesId = access.AccessoriesId,
                AccessoriesName = access.AccessoriesName,
                Remarks = access.Remarks,
                StateStatus = (access.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = access.OrganizationId,
                EUserId = access.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = access.UpUserId,
                UpdateDate = access.UpdateDate,
            }).ToList();
            List<AccessoriesViewModel> viewModel = new List<AccessoriesViewModel>();
            AutoMapper.Mapper.Map(accessoriesDTO, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAccessories(AccessoriesViewModel accessoriesViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    AccessoriesDTO dto = new AccessoriesDTO();
                    AutoMapper.Mapper.Map(accessoriesViewModel, dto);
                    isSuccess = _accessoriesBusiness.SaveAccessories(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult DeleteAccessories(long  id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _accessoriesBusiness.DeleteAccessories(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblClientProblem
        public ActionResult CilentProblemList()
        {
            IEnumerable<ClientProblemDTO> clientDTO = _clientProblemBusiness.GetAllClientProblemByOrgId(1).Select(client => new ClientProblemDTO
            {
                ProblemId = client.ProblemId,
                ProblemName = client.ProblemName,
                Remarks = client.Remarks,
                OrganizationId = client.OrganizationId,
                EUserId = client.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = client.UpUserId,
                UpdateDate = client.UpdateDate,
            }).ToList();
            List<ClientProblemViewModel> viewModel = new List<ClientProblemViewModel>();
            AutoMapper.Mapper.Map(clientDTO, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveClientProblem(ClientProblemViewModel clientProblemViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ClientProblemDTO dto = new ClientProblemDTO();
                    AutoMapper.Mapper.Map(clientProblemViewModel, dto);
                    isSuccess = _clientProblemBusiness.SaveClientProblem(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteClientProblem(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _clientProblemBusiness.DeleteClientProblem(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblMobilePart
        public ActionResult MobilePartList()
        {
            IEnumerable<MobilePartDTO> mobilePartDTO = _mobilePartBusiness.GetAllMobilePartByOrgId(1).Select(part => new MobilePartDTO
            {
                MobilePartId = part.MobilePartId,
                MobilePartName = part.MobilePartName,
                Remarks = part.Remarks,
                OrganizationId = part.OrganizationId,
                EUserId = part.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = part.UpUserId,
                UpdateDate = part.UpdateDate,
            }).ToList();
            List<MobilePartViewModel> viewModel = new List<MobilePartViewModel>();
            AutoMapper.Mapper.Map(mobilePartDTO, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveMobilePart(MobilePartViewModel mobilePartViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    MobilePartDTO dto = new MobilePartDTO();
                    AutoMapper.Mapper.Map(mobilePartViewModel, dto);
                    isSuccess = _mobilePartBusiness.SaveMobile(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteMobilePart(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _mobilePartBusiness.DeleteMobilePart(id, User.OrgId);
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