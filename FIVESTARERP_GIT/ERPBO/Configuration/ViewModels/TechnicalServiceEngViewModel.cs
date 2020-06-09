using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
   public class TechnicalServiceEngViewModel
    {
        public long EngId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
       
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        
        public string TsCode { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
