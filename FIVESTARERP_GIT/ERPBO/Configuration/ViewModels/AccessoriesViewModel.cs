using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
   public class AccessoriesViewModel
    {
        public long AccessoriesId { get; set; }
        [StringLength(100)]
        public string AccessoriesName { get; set; }
        public string AccessoriesCode { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom Property
        [StringLength(10)]
        public string StateStatus { get; set; }
    }
}
