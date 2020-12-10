using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
    public class ScrapStockInfoDTO
    {
        public long ScrapStockInfoId { get; set; }
        public long? DescriptionId { get; set; }
        public long? PartsId { get; set; }
        public int ScrapQuantity { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        //Custom
        public string ModelName { get; set; }
        public string PartsName { get; set; }
    }
}
