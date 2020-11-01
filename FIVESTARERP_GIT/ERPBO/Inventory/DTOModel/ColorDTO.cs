using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
    public class ColorDTO
    {
        public long ColorId { get; set; }
        [StringLength(100)]
        public string ColorName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom
        [StringLength(10)]
        public string StateStatus { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
