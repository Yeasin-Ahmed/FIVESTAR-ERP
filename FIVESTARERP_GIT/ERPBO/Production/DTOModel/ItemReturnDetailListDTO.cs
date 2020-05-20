using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class ItemReturnDetailListDTO
    {
        public long IRDetailId { get; set; }
        public string IRCode { get; set; }
        public string ReturnType { get; set; }
        public string FaultyCase { get; set; }
        public string ModelName { get; set; }
        public string LineNumber { get; set; }
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string UnitSymbol { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public string EntryDate { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
