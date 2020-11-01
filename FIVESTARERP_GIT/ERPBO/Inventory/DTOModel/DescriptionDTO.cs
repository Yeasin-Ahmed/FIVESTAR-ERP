using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        // Newly Added //
        [StringLength(10)]
        public string TAC { get; set; }
        public long EndPoint { get; set; }
        public long StartPoint { get; set; }
        public long? CategoryId { get; set; }
        public long? BrandId { get; set; }
        public string ColorId { get; set; }
        //custom
        public List<long> Color { get; set; }
        public string StateStatus { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public List<ModelColor> Colors { get; set; }
    }

    public class ModelColor
    {
        public long ColorId { get; set; }
        public string ColorName { get; set; }
    }

}
