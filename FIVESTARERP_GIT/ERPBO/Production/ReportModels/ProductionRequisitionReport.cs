using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ReportModels
{
    public class ProductionRequisitionReport
    {
        public string ReqInfoCode { get; set; }
        public string RequisitionType { get; set; }
        public string ModelName { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public string RequisitionBy { get; set; }
        public string LineNumber { get; set; }
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public long Quantity { get; set; }
        public string UnitName { get; set; }
        public string Remarks { get; set; }
        public string StateStatus { get; set; }
        public string OrganizationName { get; set; }
        public byte[] ReportImage { get; set; }
    }
}
