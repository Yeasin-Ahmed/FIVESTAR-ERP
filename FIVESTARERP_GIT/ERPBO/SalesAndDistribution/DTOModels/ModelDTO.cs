using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.DTOModels
{
    public class DescriptionDTO
    {
        public long DescriptionId { get; set; }
        [StringLength(100)]
        public string DescriptionName { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        // Custom Properties
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public List<long> Colors { get; set; }
        public IEnumerable<ModelColorDTO> ModelColors { get; set; }
    }

    public class ModelColorDTO
    {
        public long ColorId { get; set; }
        public string ColorName { get; set; }
    }
}
