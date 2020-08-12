using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ReportModels
{
    public class QRCodesByRef
    {
        public string ReferenceId { get; set; }
        public string ReferenceNumber { get; set; }
        public string CodeNo { get; set; }
        public string ProductionFloorName { get; set; }
        public string AssemblyLineName { get; set; }
        public string ModelName { get; set; }
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
    }
}
