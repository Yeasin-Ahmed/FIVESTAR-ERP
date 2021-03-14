using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
   public class ModelSSDTO
    {
        public long ModelId { get; set; }
        public string ModelName { get; set; }
        public long BrandId { get; set; }
        public string Remarks { get; set; }
        public string Flag { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string BrandName { get; set; }
        public string EntryUser { get; set; }
    }
}
