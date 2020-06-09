using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory.Interface;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class FrontDeskController : BaseController
    {
        // Configuation
        private readonly IAccessoriesBusiness _accessoriesBusiness;
        private readonly IClientProblemBusiness _clientProblemBusiness;
        private readonly ITechnicalServiceBusiness _technicalServiceBusiness;
        // Warehouse
        private readonly IDescriptionBusiness _descriptionBusiness;

        // Front-Desk
        private readonly IJobOrderBusiness _jobOrderBusiness;

        public FrontDeskController(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IDescriptionBusiness descriptionBusiness, IJobOrderBusiness jobOrderBusiness, ITechnicalServiceBusiness technicalServiceBusiness)
        {
            this._accessoriesBusiness = accessoriesBusiness;
            this._clientProblemBusiness = clientProblemBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
            this._technicalServiceBusiness = technicalServiceBusiness;
        }

        [HttpGet]
        public ActionResult GetJobOrders(string flag, long? modelId, long? jobOrderId, string mobileNo="", string status="",string jobCode="", int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem { Text = d.DescriptionName, Value = d.DescriptionId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfJobOrderStatus().Select(r => new SelectListItem {Text=r.text,Value=r.value }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag == "view" || flag == "search" || flag == "Detail" || flag== "Assign"))
            {
                var dto = _jobOrderBusiness.GetJobOrders(mobileNo.Trim(), modelId, status.Trim(), jobOrderId, jobCode, User.OrgId);
                
                IEnumerable<JobOrderViewModel> viewModels = new List<JobOrderViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                if(flag == "view" || flag  == "search")
                {
                    // Pagination //
                    ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                    dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    //-----------------//

                    return PartialView("_GetJobOrders", viewModels);
                }
                else if(flag == "Detail")// Flag = Detail
                {
                    ViewBag.ddlJobOrderType = Utility.ListOfJobOrderType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
                    return PartialView("_GetJobOrderDetail", viewModels.FirstOrDefault());
                }
                else
                {
                    // Flag= Assign
                    ViewBag.ddlTSName = _technicalServiceBusiness.GetAllTechnicalServiceByOrgId(User.OrgId).Select(r => new SelectListItem { Text = r.Name, Value = r.EngId.ToString() }).ToList();
                    return PartialView("_GetJobOrderAssing", viewModels.FirstOrDefault());
                }
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            ViewBag.ddlDescriptions = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(d => new SelectListItem {Text =d.DescriptionName, Value=d.DescriptionId.ToString() }).ToList();

            ViewBag.ddlAccessories = _accessoriesBusiness.GetAllAccessoriesByOrgId(User.OrgId).Select(a => new SelectListItem { Text = a.AccessoriesName, Value = a.AccessoriesId.ToString() }).ToList();

            ViewBag.ddlProblems = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Select(p => new SelectListItem { Text = p.ProblemName, Value = p.ProblemId.ToString() }).ToList();
            return View();
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveJobOrder(JobOrderViewModel jobOrder, List<JobOrderAccessoriesViewModel> jobOrderAccessories, List<JobOrderProblemViewModel> jobOrderProblems)
        {
            bool IsSuccess = false;
            if(ModelState.IsValid && jobOrderProblems.Count > 0)
            {
                JobOrderDTO jobOrderDTO = new JobOrderDTO();
                List<JobOrderAccessoriesDTO> listJobOrderAccessoriesDTO = new List<JobOrderAccessoriesDTO>();
                List<JobOrderProblemDTO> listJobOrderProblemDTO = new List<JobOrderProblemDTO>();

                AutoMapper.Mapper.Map(jobOrder,jobOrderDTO);
                AutoMapper.Mapper.Map(jobOrderAccessories,listJobOrderAccessoriesDTO);
                AutoMapper.Mapper.Map(jobOrderProblems, listJobOrderProblemDTO);

                IsSuccess=_jobOrderBusiness.SaveJobOrder(jobOrderDTO, listJobOrderAccessoriesDTO, listJobOrderProblemDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult UpdateJobOrderStatus(long jobOrderId, string status, string type)
        {
            bool IsSuccess = false;
            if(jobOrderId > 0 && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(type))
            {
                IsSuccess= _jobOrderBusiness.UpdateJobOrderStatus(jobOrderId, status, type, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult AssignTSForJobOrder(long jobOrderId, long tsId)
        {
            bool IsSuccess = false;
            if (jobOrderId > 0 && tsId> 0)
            {
                IsSuccess = _jobOrderBusiness.AssignTSForJobOrder(jobOrderId, tsId, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
    }
}