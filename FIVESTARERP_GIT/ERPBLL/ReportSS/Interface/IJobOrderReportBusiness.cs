using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.ReportSS.Interface
{
   public interface IJobOrderReportBusiness
    {
        IEnumerable<JobOrderDTO> GetJobOrdersReport(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId);
        ServicesReportHead GetBranchInformation(long orgId, long branchId);
        JobOrderDTO GetReceiptForJobOrder(long jobOrderId, long orgId, long branchId);
    }
}
