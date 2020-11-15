using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.CommonModels
{
    public class HandSetItemInfo
    {
        public long ItemId { get; set; }
        public long CategoryId { get; set; }
        public long BrandId { get; set; }
        public long ModelId { get; set; }
        public long ColorId { get; set; }
    }
}
