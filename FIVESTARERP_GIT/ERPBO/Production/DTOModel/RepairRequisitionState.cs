using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class RepairRequisitionInfoStateDTO
    {
        public long RequistionId { get; set; }
        [StringLength(100)]
        public string RequisitionCode { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
        public List<RepairRequisitionDetailStateDTO> Details { get; set; }
    }

    public class RepairRequisitionDetailStateDTO
    {
        public long RSRDetailId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public int RequestQty { get; set; }
        public string UnitName { get; set; }
        public int IssueQty { get; set; }
    }
}
