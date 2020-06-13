using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class QualityControlDTO
    {
        public long QCId { get; set; }
        [StringLength(100)]
        public string QCName { get; set; }
        public bool IsActive { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long ProductionLineId { get; set; }
        // Custom Property
        public string LineNumber { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
