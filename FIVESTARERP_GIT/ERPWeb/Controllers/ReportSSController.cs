using ERPBLL.Common;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.ReportSS.Interface;
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
        // GET: ReportSS
        public ReportSSController(IJobOrderReportBusiness jobOrderReportBusiness, IJobOrderBusiness jobOrderBusiness)
        {
            this._jobOrderReportBusiness = jobOrderReportBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
        }
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetJobOrderReport(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2)
        {
            bool IsSuccess = false;
            IEnumerable<JobOrderDTO> reportData = _jobOrderReportBusiness.GetJobOrdersReport(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, User.OrgId, User.BranchId);
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
                localReport.DisplayName = "Mostofas";

                //string deviceInfo =
                //            "<DeviceInfo>" +
                //            "<OutputFormat>Excel</OutputFormat>" +
                //            "</DeviceInfo>";
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

                //string deviceInfo =
                //            "<DeviceInfo>" +
                //            "<OutputFormat>Excel</OutputFormat>" +
                //            "</DeviceInfo>";
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
    }
}