using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
    public class IQCDTO
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string IQCName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(50)]
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom
        [StringLength(10)]
        public string StateStatus { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
