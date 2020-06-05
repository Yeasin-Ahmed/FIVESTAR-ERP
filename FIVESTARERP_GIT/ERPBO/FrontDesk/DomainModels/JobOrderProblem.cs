﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("tblJobOrderProblems")]
    public class JobOrderProblem
    {
        [Key]
        public long JobOrderProblemId { get; set; }
        public long ProblemId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("JobOrder")]
        public long JobOrderId { get; set; }
        public JobOrder JobOrder { get; set; }
    }
}
