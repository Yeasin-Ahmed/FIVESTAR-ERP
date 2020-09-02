using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class FaultyCaseViewModel
    {
        public long CaseId { get; set; }
        public long? DescriptionId { get; set; }
        [StringLength(200)]
        public string ProblemDescription { get; set; }
        [StringLength(50)]
        public string FaultyGroup { get; set; }
        [StringLength(50)]
        public string FaultyType { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
