using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class BrandCategoriesViewModel
    {
        public long BrandId { get; set; }
        public string BranchName { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long? EUserId { get; set; }
        [StringLength(100)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }

    public class BrandAndCategoriesViewModel
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
