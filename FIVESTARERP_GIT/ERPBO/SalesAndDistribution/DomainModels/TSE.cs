using ERPBO.SalesAndDistribution.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.DomainModels
{
    [Table("tblTSE")]
    public class TSE: SalesHierarchyCommonModel
    {
        [Key]
        public long TSEID { get; set; }
        public long ASMId { get; set; }
        public long ASMUserId { get; set; }
    }
}
