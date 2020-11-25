using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.Inventory.Interface;
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
        private readonly IBranchBusiness2 _branchBusiness;
        private readonly ITransferInfoBusiness _transferInfoBusiness;
        private readonly ITransferDetailBusiness _transferDetailBusiness;
        private readonly IBranchBusiness _branchBusinesss;
        private readonly IFaultBusiness _faultBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IWorkShopBusiness _workShopBusiness;
        private readonly IRepairBusiness _repairBusiness;

        // Inventory //
        private readonly IDescriptionBusiness _descriptionBusiness;

        public ConfigurationController(IAccessoriesBusiness accessoriesBusiness, IClientProblemBusiness clientProblemBusiness, IMobilePartBusiness mobilePartBusiness, ICustomerBusiness customerBusiness, ITechnicalServiceBusiness technicalServiceBusiness, ICustomerServiceBusiness customerServiceBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, IBranchBusiness2 branchBusiness, ITransferInfoBusiness transferInfoBusiness, ITransferDetailBusiness transferDetailBusiness, IBranchBusiness branchBusinesss, IFaultBusiness faultBusiness, IServiceBusiness serviceBusiness, IWorkShopBusiness workShopBusiness, IRepairBusiness repairBusiness, IDescriptionBusiness descriptionBusiness)
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
            this._branchBusiness = branchBusiness;
            this._transferInfoBusiness = transferInfoBusiness;
            this._transferDetailBusiness = transferDetailBusiness;
            this._branchBusinesss = branchBusinesss;
            this._faultBusiness = faultBusiness;
            this._serviceBusiness = serviceBusiness;
            this._workShopBusiness = workShopBusiness;
            this._repairBusiness = repairBusiness;

            #region Inventory
            this._descriptionBusiness = descriptionBusiness;
            #endregion
        }
        #region tblAccessories
        public ActionResult AccessoriesList(string flag, string name, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                //ViewBag.UserPrivilege = UserPrivilege("Configuration", "AccessoriesList");
                //return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "accessories")
            {
                IEnumerable<AccessoriesDTO> accessoriesDTO = _accessoriesBusiness.GetAllAccessoriesByOrgId(User.OrgId).Where(s => (name == "" || name == null) || (s.AccessoriesName.Contains(name) || s.AccessoriesCode.Contains(name))).Select(access => new AccessoriesDTO
                {
                    AccessoriesId = access.AccessoriesId,
                    AccessoriesName = access.AccessoriesName,
                    AccessoriesCode = access.AccessoriesCode,
                    Remarks = access.Remarks,
                    StateStatus = (access.IsActive == true ? "Active" : "Inactive"),
                    OrganizationId = access.OrganizationId,
                    EUserId = access.EUserId,
                    EntryDate = access.EntryDate,
                    UpUserId = access.UpUserId,
                    UpdateDate = access.UpdateDate,
                    EntryUser = UserForEachRecord(access.EUserId.Value).UserName,
                }).ToList();
                List<AccessoriesViewModel> viewModel = new List<AccessoriesViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(accessoriesDTO.Count(), 10, page);
                accessoriesDTO = accessoriesDTO.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(accessoriesDTO, viewModel);
                return PartialView("_AccessoriesList", viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "symptom")
            {
                IEnumerable<ClientProblemDTO> clientDTO = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Where(s => (name == "" || name == null) || (s.ProblemName.Contains(name))).Select(client => new ClientProblemDTO
                {
                    ProblemId = client.ProblemId,
                    ProblemName = client.ProblemName,
                    Remarks = client.Remarks,
                    OrganizationId = client.OrganizationId,
                    EUserId = client.EUserId,
                    EntryDate = client.EntryDate,
                    UpUserId = client.UpUserId,
                    UpdateDate = client.UpdateDate,
                    EntryUser = UserForEachRecord(client.EUserId.Value).UserName,
                }).ToList();
                List<ClientProblemViewModel> viewModel = new List<ClientProblemViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(clientDTO.Count(), 10, page);
                clientDTO = clientDTO.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(clientDTO, viewModel);
                return View("_HandsetSymptom", viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "parts")
            {
                IEnumerable<MobilePartDTO> mobilePartDTO = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).Where(s => (name == "" || name == null) || (s.MobilePartName.Contains(name) || s.MobilePartCode.Contains(name))).Select(part => new MobilePartDTO
                {
                    MobilePartId = part.MobilePartId,
                    MobilePartName = part.MobilePartName,
                    MobilePartCode = part.MobilePartCode,
                    Remarks = part.Remarks,
                    OrganizationId = part.OrganizationId,
                    EUserId = part.EUserId,
                    EntryDate = part.EntryDate,
                    UpUserId = part.UpUserId,
                    UpdateDate = part.UpdateDate,
                    EntryUser = UserForEachRecord(part.EUserId.Value).UserName,
                }).ToList();
                List<MobilePartViewModel> viewModel = new List<MobilePartViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(mobilePartDTO.Count(), 10, page);
                mobilePartDTO = mobilePartDTO.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(mobilePartDTO, viewModel);
                return PartialView("_MobileParts", viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "fault")
            {
                IEnumerable<FaultDTO> faultDTO = _faultBusiness.GetAllFaultByOrgId(User.OrgId).Where(s => (name == "" || name == null) || (s.FaultName.Contains(name) || s.FaultCode.Contains(name))).Select(fault => new FaultDTO
                {
                    FaultId = fault.FaultId,
                    FaultName = fault.FaultName,
                    FaultCode = fault.FaultCode,
                    Remarks = fault.Remarks,
                    OrganizationId = fault.OrganizationId,
                    EUserId = fault.EUserId,
                    EntryDate = fault.EntryDate,
                    UpUserId = fault.UpUserId,
                    UpdateDate = fault.UpdateDate,
                    EntryUser = UserForEachRecord(fault.EUserId.Value).UserName,
                }).ToList();
                List<FaultViewModel> viewModel = new List<FaultViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(faultDTO.Count(), 10, page);
                faultDTO = faultDTO.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(faultDTO, viewModel);
                return PartialView("_Fault", viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "services")
            {
                IEnumerable<ServiceDTO> serviceDTO = _serviceBusiness.GetAllServiceByOrgId(User.OrgId).Where(s => (name == "" || name == null) || (s.ServiceName.Contains(name) || s.ServiceCode.Contains(name))).Select(services => new ServiceDTO
                {
                    ServiceId = services.ServiceId,
                    ServiceName = services.ServiceName,
                    ServiceCode = services.ServiceCode,
                    Remarks = services.Remarks,
                    OrganizationId = services.OrganizationId,
                    EUserId = services.EUserId,
                    EntryDate = services.EntryDate,
                    UpUserId = services.UpUserId,
                    UpdateDate = services.UpdateDate,
                    EntryUser = UserForEachRecord(services.EUserId.Value).UserName,
                }).ToList();
                List<ServiceViewModel> viewModel = new List<ServiceViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(serviceDTO.Count(), 10, page);
                serviceDTO = serviceDTO.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(serviceDTO, viewModel);
                return PartialView("_Services", viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "repair")
            {
                IEnumerable<RepairDTO> repairDTO = _repairBusiness.GetAllRepairByOrgId(User.OrgId).Where(r => (name == "" || name == null) || (r.RepairName.Contains(name) || r.RepairCode.Contains(name))).Select(repair => new RepairDTO
                {
                    RepairId = repair.RepairId,
                    RepairName = repair.RepairName,
                    RepairCode = repair.RepairCode,
                    Remarks = repair.Remarks,
                    OrganizationId = repair.OrganizationId,
                    EUserId = repair.EUserId,
                    EntryDate = repair.EntryDate,
                    UpUserId = repair.UpUserId,
                    UpdateDate = repair.UpdateDate,
                    EntryUser = UserForEachRecord(repair.EUserId.Value).UserName,
                }).ToList();
                List<RepairViewModel> viewModels = new List<RepairViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(repairDTO.Count(), 10, page);
                repairDTO = repairDTO.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(repairDTO, viewModels);
                return PartialView("_Repair", viewModels);
            }
            return View();
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

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteAccessories(long id)
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
            //IEnumerable<ClientProblemDTO> clientDTO = _clientProblemBusiness.GetAllClientProblemByOrgId(User.OrgId).Select(client => new ClientProblemDTO
            //{
            //    ProblemId = client.ProblemId,
            //    ProblemName = client.ProblemName,
            //    Remarks = client.Remarks,
            //    OrganizationId = client.OrganizationId,
            //    EUserId = client.EUserId,
            //    EntryDate = client.EntryDate,
            //    UpUserId = client.UpUserId,
            //    UpdateDate = client.UpdateDate,
            //}).ToList();
            var clientDTO = _clientProblemBusiness.GetClientProblemByOrgId(User.OrgId);
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
            //IEnumerable<MobilePartDTO> mobilePartDTO = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).Select(part => new MobilePartDTO
            //{
            //    MobilePartId = part.MobilePartId,
            //    MobilePartName = part.MobilePartName,
            //    MobilePartCode = part.MobilePartCode,
            //    Remarks = part.Remarks,
            //    OrganizationId = part.OrganizationId,
            //    EUserId = part.EUserId,
            //    EntryDate = part.EntryDate,
            //    UpUserId = part.UpUserId,
            //    UpdateDate = part.UpdateDate,
            //}).ToList();
            var mobilePartDTO = _mobilePartBusiness.GetMobilePartByOrgId(User.OrgId);
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
            IEnumerable<CustomerDTO> customerDTO = _customerBusiness.GetAllCustomerByOrgId(User.OrgId, User.BranchId).Select(cus => new CustomerDTO
            {
                CustomerId = cus.CustomerId,
                CustomerName = cus.CustomerName,
                CustomerAddress = cus.CustomerAddress,
                CustomerPhone = cus.CustomerPhone,
                Remarks = cus.Remarks,
                OrganizationId = cus.OrganizationId,
                EUserId = cus.EUserId,
                EntryDate = cus.EntryDate,
                UpUserId = cus.UpUserId,
                UpdateDate = cus.UpdateDate,
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
                    isSuccess = _customerBusiness.SaveCustomer(dto, User.UserId, User.OrgId, User.BranchId);
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
                    isSuccess = _customerBusiness.DeleteCustomer(id, User.OrgId, User.BranchId);
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
            IEnumerable<TechnicalServiceEngDTO> engDTO = _technicalServiceBusiness.GetAllTechnicalServiceByOrgId(User.OrgId, User.BranchId).Select(ts => new TechnicalServiceEngDTO
            {
                EngId = ts.EngId,
                Name = ts.Name,
                TsCode = ts.TsCode,
                Address = ts.Address,
                PhoneNumber = ts.PhoneNumber,
                UserName = ts.UserName,
                Password = ts.Password,
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
                    isSuccess = _technicalServiceBusiness.SaveTechnicalService(dto, User.UserId, User.OrgId, User.BranchId);
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
                    isSuccess = _technicalServiceBusiness.DeleteTechnicalServiceEng(id, User.OrgId, User.BranchId);
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
            IEnumerable<CustomerServiceDTO> customerServiceDTO = _customerServiceBusiness.GetAllCustomerServiceByOrgId(User.OrgId).Select(service => new CustomerServiceDTO
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
            //IEnumerable<ServicesWarehouseDTO> servicesWarehouseDTO = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(ware => new ServicesWarehouseDTO
            //{
            //    SWarehouseId = ware.SWarehouseId,
            //    ServicesWarehouseName = ware.ServicesWarehouseName,
            //    Remarks = ware.Remarks,
            //    OrganizationId = ware.OrganizationId,
            //    BranchId = ware.BranchId,
            //    EUserId = ware.EUserId,
            //    EntryDate = DateTime.Now,
            //    UpUserId = ware.UpUserId,
            //    UpdateDate = DateTime.Now,
            //}).ToList();
            var servicesWarehouseDTO = _servicesWarehouseBusiness.GetServiceWarehouseByOrgId(User.OrgId, User.BranchId);
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
                    isSuccess = _servicesWarehouseBusiness.SaveServiceWarehouse(dto, User.UserId, User.OrgId, User.BranchId);
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
                    isSuccess = _servicesWarehouseBusiness.DeleteServicesWarehouse(id, User.OrgId, User.BranchId);
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
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            ViewBag.ddlModels = new SelectList(_descriptionBusiness.GetDescriptionByOrgId(User.OrgId), "DescriptionId", "DescriptionName");

            return View();
        }

        public ActionResult CreateMobilePartStock()
        {
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            ViewBag.ddlModels = new SelectList(_descriptionBusiness.GetDescriptionByOrgId(User.OrgId), "DescriptionId", "DescriptionName");

            return View();
        }
        public ActionResult MobilePartStockInfoPartialList(long? SwerehouseId, long? MobilePartId,long? modelId, string lessOrEq, int page = 1)
        {
            //IEnumerable<MobilePartStockInfoDTO> partStockInfoDTO = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId, User.BranchId).Select(info => new MobilePartStockInfoDTO
            //{
            //    MobilePartStockInfoId = info.MobilePartStockInfoId,
            //    SWarehouseId = info.SWarehouseId,
            //    ServicesWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
            //    MobilePartId = info.MobilePartId,
            //    MobilePartName = (_mobilePartBusiness.GetMobilePartOneByOrgId(info.MobilePartId.Value, User.OrgId).MobilePartName),
            //    PartsCode = (_mobilePartBusiness.GetMobilePartOneByOrgId(info.MobilePartId.Value, User.OrgId).MobilePartCode),
            //    CostPrice = info.CostPrice,
            //    SellPrice = info.SellPrice,
            //    StockInQty = info.StockInQty,
            //    StockOutQty = info.StockOutQty,
            //    Remarks = info.Remarks,
            //    OrganizationId = info.OrganizationId
            //}).AsEnumerable();
            //partStockInfoDTO = partStockInfoDTO.Where(s => (SwerehouseId == null || SwerehouseId == 0 || s.SWarehouseId == SwerehouseId) && (MobilePartId == null || MobilePartId == 0 || s.MobilePartId == MobilePartId) && (string.IsNullOrEmpty(lessOrEq) || (s.StockInQty - s.StockOutQty) <= Convert.ToInt32(lessOrEq))).ToList();

            IEnumerable<MobilePartStockInfoDTO> partStockInfoDTO = _mobilePartStockInfoBusiness.GetMobilePartsStockInformations(SwerehouseId ?? 0,  modelId ?? 0,MobilePartId ?? 0, lessOrEq, User.OrgId);

            List<MobilePartStockInfoViewModel> warehouseStockInfoViews = new List<MobilePartStockInfoViewModel>();
            // Pagination //
            ViewBag.PagerData = GetPagerData(partStockInfoDTO.Count(), 10, page);
            partStockInfoDTO = partStockInfoDTO.Skip((page - 1) * 10).Take(10).ToList();
            //-----------------//
            AutoMapper.Mapper.Map(partStockInfoDTO, warehouseStockInfoViews);
            return PartialView("_MobilePartStockInfoList", warehouseStockInfoViews);
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
                    isSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockIn(dtos, User.UserId, User.OrgId, User.BranchId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        public ActionResult MobilePartStockDetailList(string flag, long? swarehouseId, long? mobilePartId, string stockStatus, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();

                ViewBag.ddlMobileParts = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();
                return View();
            }
            else
            {
                IEnumerable<MobilePartStockDetailDTO> partStockDetailDTO = _mobilePartStockDetailBusiness.GelAllMobilePartStockDetailByOrgId(User.OrgId, User.BranchId).Select(detail => new MobilePartStockDetailDTO
                {
                    MobilePartStockDetailId = detail.MobilePartStockDetailId,
                    SWarehouseId = detail.SWarehouseId,
                    ServicesWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(detail.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
                    MobilePartId = detail.MobilePartId,
                    MobilePartName = (_mobilePartBusiness.GetMobilePartOneByOrgId(detail.MobilePartId.Value, User.OrgId).MobilePartName),
                    PartsCode = (_mobilePartBusiness.GetMobilePartOneByOrgId(detail.MobilePartId.Value, User.OrgId).MobilePartCode),
                    CostPrice = detail.CostPrice,
                    SellPrice = detail.SellPrice,
                    Quantity = detail.Quantity,
                    StockStatus = detail.StockStatus,
                    Remarks = detail.Remarks,
                    EUserId = detail.EUserId,
                    EntryDate = detail.EntryDate,

                }).OrderByDescending(e => e.EntryDate).AsEnumerable();
                // Search start from here..
                partStockDetailDTO = partStockDetailDTO.Where(f => 1 == 1 &&
                         (swarehouseId == null || swarehouseId <= 0 || f.SWarehouseId == swarehouseId) &&
                         (stockStatus == null || stockStatus.Trim() == "" || f.StockStatus == stockStatus.Trim()) &&
                         (mobilePartId == null || mobilePartId <= 0 || f.MobilePartId == mobilePartId) &&
                         (
                             (fromDate == null && toDate == null)
                             ||
                              (fromDate == "" && toDate == "")
                             ||
                             (fromDate.Trim() != "" && toDate.Trim() != "" &&

                                 f.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                                 f.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                             ||
                             (fromDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                             ||
                             (toDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                         )
                     );
                List<MobilePartStockDetailViewModel> mobilePartStockDetailViewModels = new List<MobilePartStockDetailViewModel>();
                // Pagination //
                ViewBag.PagerData = GetPagerData(partStockDetailDTO.Count(), 10, page);
                partStockDetailDTO = partStockDetailDTO.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(partStockDetailDTO, mobilePartStockDetailViewModels);
                return PartialView("_MobilePartStockDetailList", mobilePartStockDetailViewModels);
            }
        }

        [HttpPost]
        public ActionResult SaveReturnPartsStockIn(List<MobilePartStockDetailViewModel> models, long returnInfoId, string status)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && models.Count > 0)
            {
                try
                {
                    List<MobilePartStockDetailDTO> dtos = new List<MobilePartStockDetailDTO>();
                    AutoMapper.Mapper.Map(models, dtos);
                    isSuccess = _mobilePartStockDetailBusiness.SaveReturnPartsStockIn(dtos, returnInfoId, status, User.UserId, User.OrgId, User.BranchId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblBranch
        public ActionResult BranchList()
        {
            IEnumerable<BranchDTO> branchDTO = _branchBusiness.GetAllBranchByOrgId(User.OrgId).Select(branch => new BranchDTO
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                BranchAddress = branch.BranchAddress,
                Remarks = branch.Remarks,
                OrganizationId = branch.OrganizationId,
                EUserId = branch.EUserId,
                EntryDate = DateTime.Now,
                UpUserId = branch.UpUserId,
                UpdateDate = branch.UpdateDate,
            }).ToList();
            List<BranchViewModel> viewModel = new List<BranchViewModel>();
            AutoMapper.Mapper.Map(branchDTO, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
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

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteBranch(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _branchBusiness.DeleteBranch(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblPartsTransfer B-B
        public ActionResult TransferInfoList()
        {
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();
            return View();
        }
        public ActionResult StockTransferInfoPartialList(long? sWerehouseId)
        {
            IEnumerable<TransferInfoDTO> transferInfoDTO = _transferInfoBusiness.GetAllStockTransferByOrgIdAndBranch(User.OrgId, User.BranchId).Select(trans => new TransferInfoDTO
            {
                TransferInfoId = trans.TransferInfoId,
                TransferCode = trans.TransferCode,
                BranchTo = trans.BranchTo.Value,
                BranchId = trans.BranchId,
                BranchName = (_branchBusinesss.GetBranchOneByOrgId(trans.BranchId.Value, User.OrgId).BranchName),
                SWarehouseId = trans.WarehouseId,
                SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(trans.WarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
                Remarks = trans.Remarks,
                OrganizationId = trans.OrganizationId,
                StateStatus = trans.StateStatus
            }).AsEnumerable();

            transferInfoDTO = transferInfoDTO.Where(s => (sWerehouseId == null || sWerehouseId == 0 || s.SWarehouseId == sWerehouseId)).ToList();

            List<TransferInfoViewModel> transferInfoViewModels = new List<TransferInfoViewModel>();
            AutoMapper.Mapper.Map(transferInfoDTO, transferInfoViewModels);

            return PartialView("_StockTransferInfoPartialList", transferInfoViewModels);
        }
        public ActionResult CreateStockTransfer()
        {
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();

            ViewBag.ddlBranchName = _branchBusinesss.GetBranchByOrgId(User.OrgId).Where(b => b.BranchId != User.BranchId).Select(branch => new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() }).ToList();

            ViewBag.ddlMobileParts = _mobilePartBusiness.GetAllMobilePartByOrgId(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();

            ViewBag.ddlCostPrice = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId, User.BranchId).Select(mobile => new SelectListItem { Text = mobile.CostPrice.ToString(), Value = mobile.MobilePartStockInfoId.ToString() }).ToList();

            //ViewBag.ddlSellPrice = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(User.OrgId, User.BranchId).Select(mobile => new SelectListItem { Text = mobile.SellPrice.ToString(), Value = mobile.MobilePartStockInfoId.ToString() }).ToList();

            return View();
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTransferStockInfo(TransferInfoViewModel info, List<TransferDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count() > 0)
            {
                TransferInfoDTO dtoInfo = new TransferInfoDTO();
                List<TransferDetailDTO> dtoDetail = new List<TransferDetailDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _transferInfoBusiness.SaveTransferStockInfo(dtoInfo, dtoDetail, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSuccess);
        }
        public ActionResult TransferStockDetails(long transferId)
        {
            var info = _transferInfoBusiness.GetStockTransferInfoById(transferId, User.OrgId);
            IEnumerable<TransferDetailDTO> infoDTO = _transferDetailBusiness.GetAllTransferDetailByInfoId(transferId, User.OrgId).Select(details => new TransferDetailDTO
            {
                TransferDetailId = details.TransferDetailId,
                PartsId = details.PartsId,
                PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(details.PartsId.Value, User.OrgId).MobilePartName),
                CostPrice = details.CostPrice,
                SellPrice = details.SellPrice,
                Quantity = details.Quantity,
                Remarks = details.Remarks,
                OrganizationId = details.OrganizationId,
                EUserId = details.EUserId,
                EntryDate = details.EntryDate,
            }).ToList();

            //ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, info.BranchTo.Value).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            List<TransferDetailViewModel> viewModel = new List<TransferDetailViewModel>();
            AutoMapper.Mapper.Map(infoDTO, viewModel);
            return PartialView("_TransferStockDetails", viewModel);
        }


        //Stock Recive Part
        [HttpGet]
        public ActionResult RecevieStockFromTransfer()
        {
            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, User.BranchId).Select(services => new SelectListItem { Text = services.ServicesWarehouseName, Value = services.SWarehouseId.ToString() }).ToList();
            return View();
        }
        public ActionResult RecevieStockFromTransferInfoPartialList(long? sWerehouseId)
        {
            var data = _transferInfoBusiness.GetAllStockTransferByOrgId(User.OrgId).Where(s => s.BranchTo == User.BranchId).ToList();

            IEnumerable<TransferInfoDTO> transferInfoDTO = data
            .Select(trans => new TransferInfoDTO
            {
                TransferInfoId = trans.TransferInfoId,
                TransferCode = trans.TransferCode,
                BranchTo = trans.BranchTo.Value,
                BranchToName = (_branchBusinesss.GetBranchOneByOrgId(trans.BranchTo.Value, User.OrgId).BranchName),
                BranchId = trans.BranchId,
                BranchName = (_branchBusinesss.GetBranchOneByOrgId(trans.BranchId.Value, User.OrgId).BranchName),
                StateStatus = trans.StateStatus,
                //SWarehouseId = trans.WarehouseId,
                SWarehouseName = trans.StateStatus == RequisitionStatus.Accepted ? (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(trans.WarehouseIdTo.Value, User.OrgId, trans.BranchTo.Value).ServicesWarehouseName) : "",
                Remarks = trans.Remarks,
                OrganizationId = trans.OrganizationId,
                ItemCount = _transferDetailBusiness.GetAllTransferDetailByInfoId(trans.TransferInfoId, User.OrgId, User.BranchId).Count()
            }).AsEnumerable();

            transferInfoDTO = transferInfoDTO.Where(s => (sWerehouseId == null || sWerehouseId == 0 || s.SWarehouseId == sWerehouseId)).ToList();

            List<TransferInfoViewModel> transferInfoViewModels = new List<TransferInfoViewModel>();
            AutoMapper.Mapper.Map(transferInfoDTO, transferInfoViewModels);

            return PartialView(transferInfoViewModels);
        }
        public ActionResult TransferStockReciveDetails(long transferId)
        {
            var info = _transferInfoBusiness.GetStockTransferInfoById(transferId, User.OrgId);
            ViewBag.StateStatus = info.StateStatus;
            IEnumerable<TransferDetailDTO> infoDTO = _transferDetailBusiness.GetAllTransferDetailByInfoId(transferId, User.OrgId).Select(details => new TransferDetailDTO
            {
                TransferDetailId = details.TransferDetailId,
                PartsId = details.PartsId,
                PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(details.PartsId.Value, User.OrgId).MobilePartName),
                CostPrice = details.CostPrice,
                SellPrice = details.SellPrice,
                Quantity = details.Quantity,
                Remarks = details.Remarks,
                OrganizationId = details.OrganizationId,
                EUserId = details.EUserId,
                EntryDate = details.EntryDate,
            }).ToList();

            ViewBag.ddlServicesWarehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(User.OrgId, info.BranchTo.Value).Select(ware => new SelectListItem { Text = ware.ServicesWarehouseName, Value = ware.SWarehouseId.ToString() }).ToList();

            List<TransferDetailViewModel> viewModel = new List<TransferDetailViewModel>();
            AutoMapper.Mapper.Map(infoDTO, viewModel);
            return PartialView("_TransferStockReciveDetails", viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTranferStockStatus(long transferId, long swarehouse, string status)
        {
            bool IsSucess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
            {
                IsSucess = _transferInfoBusiness.SaveTransferInfoStateStatus(transferId, swarehouse, status, User.UserId, User.OrgId, User.BranchId);
            }
            return Json(IsSucess);
        }
        #endregion

        #region tblFault
        public ActionResult GetFaultList()
        {
            IEnumerable<FaultDTO> faultDTO = _faultBusiness.GetAllFaultByOrgId(User.OrgId).Select(fault => new FaultDTO
            {
                FaultId = fault.FaultId,
                FaultName = fault.FaultName,
                FaultCode = fault.FaultCode,
                Remarks = fault.Remarks,
                OrganizationId = fault.OrganizationId,
                EUserId = fault.EUserId,
                EntryDate = fault.EntryDate,
                UpUserId = fault.UpUserId,
                UpdateDate = fault.UpdateDate,
            }).ToList();
            List<FaultViewModel> viewModel = new List<FaultViewModel>();
            AutoMapper.Mapper.Map(faultDTO, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFault(FaultViewModel faultViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    FaultDTO dto = new FaultDTO();
                    AutoMapper.Mapper.Map(faultViewModel, dto);
                    isSuccess = _faultBusiness.SaveFault(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteFault(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _faultBusiness.DeleteFault(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblService
        public ActionResult GetServiceList()
        {
            IEnumerable<ServiceDTO> serviceDTO = _serviceBusiness.GetAllServiceByOrgId(User.OrgId).Select(services => new ServiceDTO
            {
                ServiceId = services.ServiceId,
                ServiceName = services.ServiceName,
                ServiceCode = services.ServiceCode,
                Remarks = services.Remarks,
                OrganizationId = services.OrganizationId,
                EUserId = services.EUserId,
                EntryDate = services.EntryDate,
                UpUserId = services.UpUserId,
                UpdateDate = services.UpdateDate,
            }).ToList();
            List<ServiceViewModel> viewModel = new List<ServiceViewModel>();
            AutoMapper.Mapper.Map(serviceDTO, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveService(ServiceViewModel serviceViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ServiceDTO dto = new ServiceDTO();
                    AutoMapper.Mapper.Map(serviceViewModel, dto);
                    isSuccess = _serviceBusiness.SaveService(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteService(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _serviceBusiness.DeleteService(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblWorkShop
        public ActionResult GetWorkShopList()
        {
            IEnumerable<WorkShopDTO> workShopDTO = _workShopBusiness.GetAllWorkShopByOrgId(User.OrgId, User.BranchId).Select(shop => new WorkShopDTO
            {
                WorkShopId = shop.WorkShopId,
                WorkShopName = shop.WorkShopName,
                Remarks = shop.Remarks,
                OrganizationId = shop.OrganizationId,
                BranchId = shop.BranchId,
                EUserId = shop.EUserId,
                EntryDate = shop.EntryDate
            }).ToList();
            List<WorkShopViewModel> workShopViewModels = new List<WorkShopViewModel>();
            AutoMapper.Mapper.Map(workShopDTO, workShopViewModels);
            return View(workShopViewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveWorkShop(WorkShopViewModel workShopViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    WorkShopDTO dto = new WorkShopDTO();
                    AutoMapper.Mapper.Map(workShopViewModel, dto);
                    isSuccess = _workShopBusiness.SaveWorkShop(dto, User.UserId, User.OrgId, User.BranchId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteWorkShop(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _workShopBusiness.DeleteWorkShop(id, User.OrgId, User.BranchId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region tblRepairCode
        public ActionResult GetRepairList()
        {
            IEnumerable<RepairDTO> repairDTOs = _repairBusiness.GetAllRepairByOrgId(User.OrgId).Select(services => new RepairDTO
            {
                RepairId = services.RepairId,
                RepairName = services.RepairName,
                RepairCode = services.RepairCode,
                Remarks = services.Remarks,
                OrganizationId = services.OrganizationId,
                EUserId = services.EUserId,
                EntryDate = services.EntryDate,
                UpUserId = services.UpUserId,
                UpdateDate = services.UpdateDate,
            }).ToList();
            List<RepairViewModel> viewModel = new List<RepairViewModel>();
            AutoMapper.Mapper.Map(repairDTOs, viewModel);
            return View(viewModel);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepair(RepairViewModel repairViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    RepairDTO dto = new RepairDTO();
                    AutoMapper.Mapper.Map(repairViewModel, dto);
                    isSuccess = _repairBusiness.SaveRepair(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteRepair(long id)
        {
            bool isSuccess = false;
            if (id > 0)
            {
                try
                {
                    isSuccess = _repairBusiness.DeleteRepair(id, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Reports

        [HttpGet]
        public ActionResult GetCurrentStockReport(string flag, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlMobilePart = _mobilePartBusiness.GetAllMobilePartAndCode(User.OrgId).Select(mobile => new SelectListItem { Text = mobile.MobilePartName, Value = mobile.MobilePartId.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _mobilePartStockInfoBusiness.GetCurrentStock(User.OrgId, User.BranchId);
                IEnumerable<MobilePartStockInfoViewModel> viewModels = new List<MobilePartStockInfoViewModel>();
                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), 10, page);
                //dto = dto.Skip((page - 1) * 10).Take(10).ToList();
                //-----------------//
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetCurrentStockList", viewModels);
            }
        }
        #endregion
    }
}