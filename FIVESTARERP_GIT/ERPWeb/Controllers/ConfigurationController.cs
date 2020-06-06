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
        private readonly ICustomerBusiness _customerBusiness;
        private readonly ITechnicalServiceBusiness _technicalServiceBusiness;
        private readonly ICustomerServiceBusiness _customerServiceBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;

        public ConfigurationController(IAccessoriesBusiness accessoriesBusiness,IClientProblemBusiness clientProblemBusiness,IMobilePartBusiness mobilePartBusiness,ICustomerBusiness customerBusiness,ITechnicalServiceBusiness technicalServiceBusiness,ICustomerServiceBusiness customerServiceBusiness,IServicesWarehouseBusiness servicesWarehouseBusiness,IMobilePartStockInfoBusiness mobilePartStockInfoBusiness,IMobilePartStockDetailBusiness mobilePartStockDetailBusiness)
        {
            this._accessoriesBusiness = accessoriesBusiness;
            this._clientProblemBusiness = clientProblemBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._customerBusiness = customerBusiness;
            this._technicalServiceBusiness = technicalServiceBusiness;
            this._customerServiceBusiness = customerServiceBusiness;
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
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

        #region tblCustomers
        public ActionResult CustomerList()
        {
            IEnumerable<CustomerDTO> customerDTO = _customerBusiness.GetAllCustomerByOrgId(1).Select(cus => new CustomerDTO
            {
                CustomerId = cus.CustomerId,
                CustomerName = cus.CustomerName,
                CustomerAddress = cus.CustomerAddress,
                CustomerPhone = cus.CustomerPhone,
                Remarks = cus.Remarks,
                OrganizationId = cus.OrganizationId,
                EUserId = cus.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = cus.UpUserId,
                UpdateDate = DateTime.Now,
            }).ToList();
            List<CustomerViewModel> viewModel = new List<CustomerViewModel>();
            AutoMapper.Mapper.Map(customerDTO, viewModel);
            return View(viewModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveCustomer(CustomerViewModel customerViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    CustomerDTO dto = new CustomerDTO();
                    AutoMapper.Mapper.Map(customerViewModel, dto);
                    isSuccess = _customerBusiness.SaveCustomer(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteCustomer(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _customerBusiness.DeleteCustomer(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region T.S.
        public ActionResult TechnicalServiceEngList()
        {
            IEnumerable<TechnicalServiceEngDTO> engDTO = _technicalServiceBusiness.GetAllTechnicalServiceByOrgId(1).Select(ts => new TechnicalServiceEngDTO
            {
                EngId = ts.EngId,
                Name = ts.Name,
                Address = ts.Address,
                PhoneNumber = ts.PhoneNumber,
                Remarks = ts.Remarks,
                OrganizationId = ts.OrganizationId,
                EUserId = ts.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = ts.UpUserId,
                UpdateDate = DateTime.Now,
            }).ToList();
            List<TechnicalServiceEngViewModel> viewModel = new List<TechnicalServiceEngViewModel>();
            AutoMapper.Mapper.Map(engDTO, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTechnicalService(TechnicalServiceEngViewModel technicalServiceEngViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    TechnicalServiceEngDTO dto = new TechnicalServiceEngDTO();
                    AutoMapper.Mapper.Map(technicalServiceEngViewModel, dto);
                    isSuccess = _technicalServiceBusiness.SaveTechnicalService(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteTechnicalService(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _technicalServiceBusiness.DeleteTechnicalServiceEng(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblCustomerServices
        public ActionResult CustomerServiceList()
        {
            IEnumerable<CustomerServiceDTO> customerServiceDTO = _customerServiceBusiness.GetAllCustomerServiceByOrgId(1).Select(service => new CustomerServiceDTO
            {
                CsId = service.CsId,
                Name = service.Name,
                Address = service.Address,
                PhoneNumber = service.PhoneNumber,
                Remarks = service.Remarks,
                OrganizationId = service.OrganizationId,
                EUserId = service.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = service.UpUserId,
                UpdateDate = DateTime.Now,
            }).ToList();
            List<CustomerServiceViewModel> viewModel = new List<CustomerServiceViewModel>();
            AutoMapper.Mapper.Map(customerServiceDTO, viewModel);
            return View(viewModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveCustomerService(CustomerServiceViewModel customerServiceViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    CustomerServiceDTO dto = new CustomerServiceDTO();
                    AutoMapper.Mapper.Map(customerServiceViewModel, dto);
                    isSuccess = _customerServiceBusiness.SaveCustomerService(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteCustomerService(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _customerServiceBusiness.DeleteCustomerService(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblServicesWarehouse
        public ActionResult ServicesWarehouseList()
        {
            IEnumerable<ServicesWarehouseDTO> servicesWarehouseDTO = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(1).Select(ware => new ServicesWarehouseDTO
            {
                SWarehouseId = ware.SWarehouseId,
                ServicesWarehouseName = ware.ServicesWarehouseName,
                Remarks = ware.Remarks,
                OrganizationId = ware.OrganizationId,
                EUserId = ware.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = ware.UpUserId,
                UpdateDate = DateTime.Now,
            }).ToList();
            List<ServicesWarehouseViewModel> viewModel = new List<ServicesWarehouseViewModel>();
            AutoMapper.Mapper.Map(servicesWarehouseDTO, viewModel);
            return View(viewModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveServicesWarehouse(ServicesWarehouseViewModel servicesWarehouseViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ServicesWarehouseDTO dto = new ServicesWarehouseDTO();
                    AutoMapper.Mapper.Map(servicesWarehouseViewModel, dto);
                    isSuccess = _servicesWarehouseBusiness.SaveServiceWarehouse(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteServicesWarehouse(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _servicesWarehouseBusiness.DeleteServicesWarehouse(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region MobilePartStock
        public ActionResult MobilePartStockInfoList()
        {
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            return View();
        }

        public ActionResult CreateMobilePartStock()
        {
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            return View();
        }
        public ActionResult MobilePartStockInfoPartialList()
        {
            IEnumerable<MobilePartStockInfoDTO> partStockInfoDTO = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId).Select(info => new MobilePartStockInfoDTO
            {
                MobilePartStockInfoId = info.MobilePartStockInfoId,
                SWarehouseId=info.SWarehouseId,
                ServicesWarehouseName= (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId).ServicesWarehouseName),
                MobilePartId=info.MobilePartId,
                MobilePartName= (_mobilePartBusiness.GetMobilePartOneByOrgId(info.MobilePartId.Value, User.OrgId).MobilePartName),
                StockInQty = info.StockInQty,
                StockOutQty = info.StockOutQty,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId
            }).ToList();
            List<MobilePartStockInfoViewModel> warehouseStockInfoViews = new List<MobilePartStockInfoViewModel>();
            AutoMapper.Mapper.Map(partStockInfoDTO, warehouseStockInfoViews);
            return PartialView("_MobilePartStockInfoList",warehouseStockInfoViews);
        }

        [HttpPost]
        public ActionResult SaveMobilePartStockIn(List<MobilePartStockDetailViewModel> models)
        {
            bool isSuccess = false;
            var pre = UserPrivilege("Configuration", "MobilePartStockInfoList");
            var permission = ((pre.Add) || (pre.Edit));
            if (ModelState.IsValid && models.Count > 0 && permission)
            {
                try
                {
                    List<MobilePartStockDetailDTO> dtos = new List<MobilePartStockDetailDTO>();
                    AutoMapper.Mapper.Map(models, dtos);
                    isSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockIn(dtos, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        public ActionResult MobilePartStockDetailList()
        {
            return View();
        }
        #endregion
    }
}