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
        private readonly IJobOrderTransferDetailBusiness _jobOrderTransferDetailBusiness;
        private readonly IJobOrderTSBusiness _jobOrderTSBusiness;
        // GET: ReportSS
        public ReportSSController(IJobOrderReportBusiness jobOrderReportBusiness, IJobOrderBusiness jobOrderBusiness, IInvoiceInfoBusiness invoiceInfoBusiness, IInvoiceDetailBusiness invoiceDetailBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IMobilePartBusiness mobilePartBusiness, ITsStockReturnDetailsBusiness tsStockReturnDetailsBusiness, ITechnicalServicesStockBusiness technicalServicesStockBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IJobOrderReturnDetailBusiness jobOrderReturnDetailBusiness, IJobOrderTransferDetailBusiness jobOrderTransferDetailBusiness, IJobOrderTSBusiness jobOrderTSBusiness)
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
            this._jobOrderTransferDetailBusiness = jobOrderTransferDetailBusiness;
            this._jobOrderTSBusiness = jobOrderTSBusiness;
        }

        #region JobOrderList
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetJobOrderReport(string mobileNo, long? modelId, string jobstatus, long? jobOrderId, string jobCode, string iMEI, string iMEI2, string fromDate, string toDate,string ddlCustomerType, string rptType)
        {
            bool IsSuccess = false;
            IEnumerable<JobOrderDTO> reportData = _jobOrderBusiness.GetJobOrders(mobileNo, modelId, jobstatus, jobOrderId, jobCode, iMEI, iMEI2, User.OrgId, User.BranchId, fromDate, toDate,ddlCustomerType).ToList();

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);

            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptJobOrder.rdlc");
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
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptJobOrderReceipt.rdlc");
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
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptJobOrderCreateReceipt.rdlc");
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
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptInvoiceReceipt.rdlc");
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
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/Configuration/rptCurrentStockReport.rdlc");
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
        public ActionResult PartsReturnReport(long? ddlMobileParts2,long? ddlTechnicalServicesName2,string jobCode2, string fromDate2, string toDate2,string rptType2)
        {
            var dto = _tsStockReturnDetailsBusiness.GetReturnParts(User.OrgId, User.BranchId, ddlTechnicalServicesName2, ddlMobileParts2, jobCode2, fromDate2, toDate2);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/Configuration/rptPartsReturnStock.rdlc");
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
                    rptType2,
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

        #region UsedPartsReport
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult UsedPartsReport(long? ddlMobileParts,long? ddlTechnicalServicesName, string fromDate, string toDate,string rptType,string jobCode)
        {
            bool IsSuccess = false;

            IEnumerable<TechnicalServicesStockDTO> dto = _technicalServicesStockBusiness.GetUsedParts(ddlMobileParts, ddlTechnicalServicesName, User.OrgId, User.BranchId, fromDate, toDate,jobCode);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/Configuration/rptUsedPartsReport.rdlc");
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
        public ActionResult SellsReport(string fromDate, string toDate,string rptType,string ddlInvoiceTypeStatus,string invoice)
        {
            bool IsSuccess = false;

            IEnumerable<InvoiceInfoDTO> dto = _invoiceInfoBusiness.GetSellsReport(User.OrgId, User.BranchId, fromDate, toDate, ddlInvoiceTypeStatus, invoice);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptSellsReport.rdlc");
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
        public ActionResult TSRequsitionReport(string reqCode, long? ddlWarehouseName, long? ddlTechnicalServicesName, string reqStatus, string fromDate, string toDate, string rptType, string jobCode = "")
        {
            
            var dto = _requsitionInfoForJobOrderBusiness.GetRequsitionInfoData(reqCode, ddlWarehouseName, ddlTechnicalServicesName, reqStatus, fromDate, toDate, User.OrgId, User.BranchId,jobCode);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/Configuration/rptTSRequsitionReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("TSRequsition", dto);
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
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptOtherBranchRepairJob.rdlc");
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

        #region RepairedJobOfOtherBranch
        //[HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult RepairedJobOfOtherBranch(long? ddlBranchName2, string fromDate2, string toDate2, string rptType2)
        {

            IEnumerable<JobOrderReturnDetailDTO> dto = _jobOrderReturnDetailBusiness.RepairedJobOfOtherBranch(User.BranchId, ddlBranchName2, User.OrgId, fromDate2, toDate2);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptRepairedJobOfOtherBranch.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("RepairedJobOtherBranch", dto);
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
                    rptType2,
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

        #region OtherBranchRequsitionReport
        public ActionResult OtherBranchRequsitionReport(string reqCode2, long? ddlWarehouseName2, long? ddlTechnicalServicesName2, string reqStatus2, string fromDate2, string toDate2, string rptType2)
        {
            var dto = _requsitionInfoForJobOrderBusiness.GetRequsitionInfoOtherBranchData(reqCode2, ddlWarehouseName2,ddlTechnicalServicesName2, reqStatus2, fromDate2, toDate2, User.OrgId, User.BranchId);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/Configuration/rptOtherBranchRequsition - Copy.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("OtherBranchRequsition", dto);
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
                    rptType2,
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

        #region JobOrderReceivedReport
        public ActionResult ReceivedJobOrderReport(long? ddlBranchName, string fromDate, string ddlTransferStatus, string toDate,string rptType, string jobCode = "", string transferCode = "")
        {
            var dto = _jobOrderTransferDetailBusiness.GetReceiveJob(User.OrgId, User.BranchId, ddlBranchName, jobCode, transferCode, fromDate, toDate, ddlTransferStatus);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptReceivedJobOrderReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("ReceivedJobOrder", dto);
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

        #region JobOrderReturnReceivedReport
        public ActionResult ReceivedReturnJobOrderReport(long? ddlBranchName2, string fromDate2, string ddlTransferStatus2, string toDate2, string rptType2, string jobCode2 = "", string transferCode2 = "")
        {
            var dto = _jobOrderReturnDetailBusiness.GetReturnJobOrder(User.OrgId, User.BranchId, ddlBranchName2, jobCode2, transferCode2, fromDate2, toDate2, ddlTransferStatus2);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptReceivedReturnJobOrderReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("ReceivedReturnJobOrder", dto);
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
                    rptType2,
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

        #region JobSignIn And Out 
        public ActionResult JobSignInAndOut(long? ddlTechnicalServicesName, string fromDate, string toDate, string rptType, string jobCode = "")
        {
            var dto = _jobOrderTSBusiness.JobSignInAndOut(ddlTechnicalServicesName,jobCode,User.OrgId,User.BranchId,fromDate,toDate);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptJobSignInAndOutReport - Copy.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("JobSignInAndOut", dto);
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

        #region Daily Summary Report 
        public ActionResult DailySummaryReport(string fromDate, string toDate, string rptType)
        {
            var dto = _jobOrderBusiness.DailySummaryReport(User.OrgId, User.BranchId, fromDate, toDate);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptDailySummaryReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("DailySummary", dto);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "DailySummary";

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

        #region ALL Branch Daily Summary Report
        public ActionResult AllBranchDailySummaryReport(string fromDate, string toDate, string rptType)
        {
            var dto = _jobOrderBusiness.AllBranchDailySummaryReport(User.OrgId, fromDate, toDate);

            ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ServiceRpt/FrontDesk/rptAllBranchDailySummaryReport.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("AllBranchDailySummary", dto);
                ReportDataSource dataSource2 = new ReportDataSource("ServicesReportHead", servicesReportHeads);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "DailySummary";

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