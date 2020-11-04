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
    [Table("tblRSM")]
    public class RSM : SalesHierarchyCommonModel
    {
        [Key]
        public long RSMID { get; set; }
    }
}
