using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
   public class DescriptionDTO
    {
        public long DescriptionId { get; set; }
        public string DescriptionName { get; set; }
        public long? SubCategoryId { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //custom

        public string StateStatus { get; set; }
    }
}
