using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class RepairRequisitionInfoStateViewModel
    {
        [Range(1,long.MaxValue)]
        public long RequistionId { get; set; }
        [Required,StringLength(100)]
        public string RequisitionCode { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
        public List<RepairRequisitionDetailStateViewModel> Details { get; set; }
    }

    public class RepairRequisitionDetailStateViewModel
    {
        [Range(1, long.MaxValue)]
        public long RSRDetailId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
        [Range(1, long.MaxValue)]
        public long UnitId { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        [Range(1, long.MaxValue)]
        public int RequestQty { get; set; }
        public string UnitName { get; set; }
        public int IssueQty { get; set; }
    }
}
