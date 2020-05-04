using ERPBO.Production.ReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Report.Interface
{
    public interface IProductionReportBusiness
    {
        IEnumerable<ProductionRequisitionReport> GetProductionRequisitionReport(long reqInfoId);
    }
}
