using ERPBLL.Common;
using ERPBLL.Report.Interface;
using ERPBO.Production.ReportModels;
using ERPWeb.Filters;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IProductionReportBusiness _productionReportBusiness; // Production
        public ReportController(IProductionReportBusiness productionReportBusiness)
        {
            this._productionReportBusiness = productionReportBusiness;
        }

        #region Production Report
        //[HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetProductionRequisitionReport(long reqInfoId)
        {
            bool IsSuccess = false;
            //string 
            IEnumerable<ProductionRequisitionReport> reportData = _productionReportBusiness.GetProductionRequisitionReport(reqInfoId);
            reportData.FirstOrDefault().OrganizationName = User.OrgName;
            reportData.FirstOrDefault().ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/rptProductionRequisition.rdlc");
            string id = string.Empty;
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource = new ReportDataSource("ProductionRequisition", reportData);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource);
                localReport.Refresh();
                localReport.DisplayName = reportData.FirstOrDefault().ReqInfoCode;

                //string deviceInfo =
                //            "<DeviceInfo>" +
                //            "<OutputFormat>Excel</OutputFormat>" +
                //            "</DeviceInfo>";
                string mimeType;
                string encoding;
                string fileNameExtension= ".pdf";
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
                //MemoryStream stream = new MemoryStream(renderedBytes);
                //id= Guid.NewGuid().ToString();
                //TempData[id] = stream.ToArray();

                var base64 = Convert.ToBase64String(renderedBytes);
                var fs = String.Format("data:application/pdf;base64,{0}", base64);
                IsSuccess = true;
                return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "ProductionRequisition_"+ localReport.DisplayName });

                //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ProductionRequisition.xlsx");
                //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //return File(renderedBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductionRequisition.xlsx");
            }
            return Json(new { IsSuccess= IsSuccess,Id=id });
        }

        public ActionResult GetYourReport(string id, string type="",string reportName="")
        {
            if(TempData[id] != null)
            {
                //MemoryStream stream = (MemoryStream)TempData[id];
                byte[] byteInfo = TempData[id] as byte[];
                //stream.Write(byteInfo, 0, byteInfo.Length);
                //stream.Position = 0;
                //Response.AddHeader("content-disposition", "attachment; filename=ProductionRequisition.xlsx");
                //return File(byteInfo, "ProductionRequisition.xlsx");

                //return new FileStreamResult()
            }
            return Content("");
        }
        #endregion
    }
}