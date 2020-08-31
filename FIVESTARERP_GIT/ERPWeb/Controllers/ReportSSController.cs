using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.ReportSS.Interface;
using ERPBO.Configuration.DTOModels;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ReportModels;
using ERPWeb.Filters;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class ReportSSController : BaseController
    {
        public readonly IJobOrderReportBusiness _jobOrderReportBusiness;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly IInvoiceInfoBusiness _invoiceInfoBusiness;
        private readonly IInvoiceDetailBusiness _invoiceDetailBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly ITsStockReturnDetailsBusiness _tsStockReturnDetailsBusiness;
        private readonly ITechnicalServicesStockBusiness _technicalServicesStockBusiness;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IJobOrderReturnDetailBusiness _jobOrderReturnDetailBusiness;
        // GET: ReportSS
        public ReportSSController(IJobOrderReportBusiness jobOrderReportBusiness, IJobOrderBusiness jobOrderBusiness, IInvoiceInfoBusiness invoiceInfoBusiness, IInvoiceDetailBusiness invoiceDetailBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IMobilePartBusiness mobilePartBusiness, ITsStockReturnDetailsBusiness tsStockReturnDetailsBusiness, ITechnicalServicesStockBusiness technicalServicesStockBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IJobOrderReturnDetailBusiness jobOrderReturnDetailBusiness)
        {
            this._jobOrderReportBusiness = jobOrderReportBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
            this._invoiceInfoBusiness = invoiceInfoBusiness;
            this._invoiceDetailBusiness = invoiceDetailBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._tsStockReturnDetailsBusiness = tsStockReturnDetailsBusiness;
            this._technicalServicesStockBusiness = technicalServicesStockBusiness;
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._jobOrderReturnDetailBusiness = jobOrderReturnDetailBusiness;
        }

        #region JobOrderList
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetJobOrderReport(string mobileNo, long? modelId, string jobstatus, long? jobOrderId, string jobCode, string iMEI, string iMEI2, string fromDate, string toDate,string rptType)
        {
            bool IsSuccess = false;
            IEnumerable<JobOrderDTO> reportData = _jobOrderReportBusiness.GetJobOrdersReport(mobileNo, modelId, jobstatus, jobOrderId, jobCode, iMEI, iMEI2, User.OrgId, User.BranchId, fromDate, toDate).ToList();

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);

            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptJobOrder.rdlc");
            string id = string.Empty;
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobOrder", reportData);
                ReportDataSource dataSource2 = new ReportDataSource("ReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "List";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
                //var base64 = Convert.ToBase64String(renderedBytes);
                //var fs = String.Format("data:application/pdf;base64,{0}", base64);
                //IsSuccess = true;
                //return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "JobOrder_" + localReport.DisplayName });
            }
            //return Json(new { IsSuccess = IsSuccess, Id = 0 },JsonRequestBehavior.AllowGet);
            return new EmptyResult();
        }
        #endregion

        #region JobOrderDeliveryReceipt
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetJobOrderReceiptReport(long jobOrderId)
        {
            bool IsSuccess = false;
            JobOrderDTO reportData = _jobOrderBusiness.GetJobOrderReceipt(jobOrderId, User.UserId, User.OrgId, User.BranchId);
            List<JobOrderDTO> servicesreportData = new List<JobOrderDTO>();
            servicesreportData.Add(reportData);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptJobOrderReceipt.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobOrderReceipt", servicesreportData);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Receipt";
                string mimeType;
                string encoding;
                string fileNameExtension = ".pdf";
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    "Pdf",
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                var base64 = Convert.ToBase64String(renderedBytes);
                var fs = String.Format("data:application/pdf;base64,{0}", base64);
                IsSuccess = true;
                return Json(new { IsSuccess = IsSuccess, File = fs, FileName = reportData.JobOrderCode });
            }
            return Json(new { IsSuccess = IsSuccess, Id = jobOrderId });
        }
        #endregion

        #region JobOrderCreateReceipt
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetJobOrderCreateReceiptReport(long jobOrderId)
        {
            bool IsSuccess = false;
            IEnumerable<JobOrderDTO> jobOrderDetails = _jobOrderBusiness.GetJobCreateReceipt(jobOrderId, User.OrgId, User.BranchId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptJobOrderCreateReceipt.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobCreateReceipt", jobOrderDetails);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Receipt";

                string mimeType;
                string encoding;
                string fileNameExtension = ".pdf";
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    "Pdf",
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                var base64 = Convert.ToBase64String(renderedBytes);
                var fs = String.Format("data:application/pdf;base64,{0}", base64);
                IsSuccess = true;
                return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "JobOrder_" + localReport.DisplayName });
            }
            return Json(new { IsSuccess = IsSuccess, Id = 0 });

        }
        #endregion

        #region InvoiceReceipt
        public ActionResult InvoiceReport(long infoId)
        {
            var infodata = _invoiceInfoBusiness.InvoiceInfoReport(infoId, User.OrgId, User.BranchId);

            // var detailsdata = _invoiceDetailBusiness.InvoiceDetailsReport(infoId,User.OrgId, User.BranchId);
            IEnumerable<InvoiceDetailDTO> detailsdata = _invoiceDetailBusiness.InvoiceDetailsReport(infoId, User.OrgId, User.BranchId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptInvoiceReceipt.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
            }
            ReportDataSource dataSource1 = new ReportDataSource("InvoiceInfo", infodata);
            ReportDataSource dataSource2 = new ReportDataSource("InvoiceDetails", detailsdata);
            ReportDataSource dataSource3 = new ReportDataSource("ReportHead", servicesReportHeads);
            localReport.DataSources.Add(dataSource1);
            localReport.DataSources.Add(dataSource2);
            localReport.DataSources.Add(dataSource3);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            var renderedBytes = localReport.Render(
                reportType,
                "",
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
                );
            return File(renderedBytes, mimeType);
        }
        #endregion

        #region WarehouseCurrentStock
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetCurrentStock(string rptType)
        {
            bool IsSuccess = false;
            IEnumerable<MobilePartStockInfoDTO> stockInfo = _mobilePartStockInfoBusiness.GetCurrentStock(User.OrgId, User.BranchId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptCurrentStockReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("CurrentStock", stockInfo);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Stock";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            return new EmptyResult();
            //    var base64 = Convert.ToBase64String(renderedBytes);
            //    var fs = String.Format("data:application/pdf;base64,{0}", base64);
            //    IsSuccess = true;
            //    return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "Current_" + localReport.DisplayName });
            //}
            //return Json(new { IsSuccess = IsSuccess, Id = 0 });
        }
        #endregion

        #region PartsReturnReport
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult PartsReturnReport(long? ddlMobileParts,long? ddlTechnicalServicesName, string fromDate, string toDate,string rptType)
        {
            bool IsSuccess = false;
            IEnumerable<TsStockReturnDetailDTO> dto = _tsStockReturnDetailsBusiness.GetAllTsStockReturn(User.OrgId, User.BranchId).Select(ret => new TsStockReturnDetailDTO
            {
                RequsitionCode = ret.RequsitionCode,
                PartsId = ret.PartsId,
                PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(ret.PartsId, User.OrgId).MobilePartName),
                PartsCode = (_mobilePartBusiness.GetMobilePartOneByOrgId(ret.PartsId, User.OrgId).MobilePartCode),
                Quantity = ret.Quantity,
                EntryDate = ret.EntryDate,
                EUserId=ret.EUserId,
                EntryUser = UserForEachRecord(ret.EUserId.Value).UserName,
            }).AsEnumerable();
            dto = dto.Where(f => 1 == 1 && (ddlMobileParts == null || ddlMobileParts <= 0 || f.PartsId == ddlMobileParts) && (ddlTechnicalServicesName == null || ddlTechnicalServicesName <= 0 || f.EUserId == ddlTechnicalServicesName) &&
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

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptPartsReturnStock.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("ReturnStock", dto);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Stock";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
                //var base64 = Convert.ToBase64String(renderedBytes);
                //var fs = String.Format("data:application/pdf;base64,{0}", base64);
                //IsSuccess = true;
                //return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "Return_" + localReport.DisplayName });
            }
            return new EmptyResult();
        }
        #endregion

        #region UsedPartsReport
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult UsedPartsReport(long? ddlMobileParts,long? ddlTechnicalServicesName, string fromDate, string toDate,string rptType)
        {
            bool IsSuccess = false;

            IEnumerable<TechnicalServicesStockDTO> dto = _technicalServicesStockBusiness.GetUsedParts(ddlMobileParts, ddlTechnicalServicesName, User.OrgId, User.BranchId, fromDate, toDate);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptUsedPartsReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("UsedParts", dto);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Parts";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
                //var base64 = Convert.ToBase64String(renderedBytes);
                //var fs = String.Format("data:application/pdf;base64,{0}", base64);
                //IsSuccess = true;
                //return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "Used_" + localReport.DisplayName });
            }
            //return Json(new { IsSuccess = IsSuccess, Id = 0 });
            return new EmptyResult();
        }
        #endregion

        #region SellsReport
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SellsReport(string fromDate, string toDate,string rptType)
        {
            bool IsSuccess = false;

            IEnumerable<InvoiceInfoDTO> dto = _invoiceInfoBusiness.GetSellsReport(User.OrgId, User.BranchId, fromDate, toDate);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptSellsReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("Sells", dto);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "List";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
                //var base64 = Convert.ToBase64String(renderedBytes);
                //var fs = String.Format("data:application/pdf;base64,{0}", base64);
                //IsSuccess = true;
                //return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "Sells_" + localReport.DisplayName });
            }
            //return Json(new { IsSuccess = IsSuccess, Id = 0 });
            return new EmptyResult();
        }
        #endregion

        #region TSRequsitionReport
        public ActionResult TSRequsitionReport(string reqCode, long? ddlWarehouseName, long? ddlTechnicalServicesName, string reqStatus, string fromDate, string toDate, string rptType)
        {
            IEnumerable<RequsitionInfoForJobOrderDTO> requsitionInfoForJobOrderDTO = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJob(User.OrgId, User.BranchId).Where(req =>
                (reqCode == null || reqCode.Trim() == "" || req.RequsitionCode.Contains(reqCode))
                &&
                (ddlWarehouseName == null || ddlWarehouseName <= 0 || req.SWarehouseId == ddlWarehouseName)
                &&
                (ddlTechnicalServicesName == null || ddlTechnicalServicesName <= 0 || req.EUserId == ddlTechnicalServicesName)
                &&
                (reqStatus == null || reqStatus.Trim() == "" || req.StateStatus == reqStatus.Trim())
                &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )).Select(info => new RequsitionInfoForJobOrderDTO
                {
                    RequsitionInfoForJobOrderId = info.RequsitionInfoForJobOrderId,
                    RequsitionCode = info.RequsitionCode,
                    SWarehouseId = info.SWarehouseId,
                    SWarehouseName = (_servicesWarehouseBusiness.GetServiceWarehouseOneByOrgId(info.SWarehouseId.Value, User.OrgId, User.BranchId).ServicesWarehouseName),
                    StateStatus = info.StateStatus,
                    JobOrderId = info.JobOrderId,
                    JobOrderCode = info.JobOrderCode,
                    Remarks = info.Remarks,
                    BranchId = info.BranchId,
                    OrganizationId = info.OrganizationId,
                    EUserId = info.EUserId,
                    Requestby = UserForEachRecord(info.EUserId.Value).UserName,
                    EntryDate = info.EntryDate
                }).ToList();
            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptTSRequsitionReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("TSRequsition", requsitionInfoForJobOrderDTO);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Stock";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
            return new EmptyResult();
        }
        #endregion

        #region OtherBranch Repair Job
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult OtherBranchRepairJob(long? ddlBranchName, string fromDate, string toDate, string rptType)
        {

            IEnumerable<JobOrderReturnDetailDTO> dto = _jobOrderReturnDetailBusiness.RepairOtherBranchJob(User.BranchId,ddlBranchName, User.OrgId, fromDate, toDate);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/rptOtherBranchRepairJob.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("OtherBranchJob", dto);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "Parts";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
            return new EmptyResult();
        }
        #endregion
    }
}