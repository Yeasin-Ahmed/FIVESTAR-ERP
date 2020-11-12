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
    [Table("tblASM")]
    public class ASM: SalesHierarchyCommonModel
    {
        [Key]
        public long ASMID { get; set; }
        public long RSMId { get; set; }
        public long RSMUserId { get; set; }
    }
}
