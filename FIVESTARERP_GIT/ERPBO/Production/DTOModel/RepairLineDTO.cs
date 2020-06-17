using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{    
    public class RepairLineDTO
    {
        public long RepairLineId { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long ProductionLineId { get; set; }
        [StringLength(100)]
        public string ProductionLineName { get; set; }
        // Custom Property
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
