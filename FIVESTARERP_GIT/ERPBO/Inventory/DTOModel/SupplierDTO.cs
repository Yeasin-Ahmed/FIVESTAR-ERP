using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
    public class SupplierDTO
    {
        public long SupplierId { get; set; }
        [StringLength(150)]
        public string SupplierName { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
