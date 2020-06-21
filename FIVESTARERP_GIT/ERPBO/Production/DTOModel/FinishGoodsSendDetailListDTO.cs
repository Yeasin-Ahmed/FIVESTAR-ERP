using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class FinishGoodsSendDetailListDTO
    {
        public long SendDetailId { get; set; }
        public string LineNumber { get; set; }
        public string PackagingLineNumber { get; set; }
        public string WarehouseName { get; set; }
        public string ModelName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public string RefferenceNumber { get; set; }
        public string Remarks { get; set; }
        public string Flag { get; set; }
        public string EntryDate { get; set; }
        public string StateStatus { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
