using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class FinishGoodsSendToWarehouseInfoDTO
    {
        public long SendId { get; set; }
        public long LineId { get; set; }
        public long WarehouseId { get; set; }
        public long DescriptionId { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(150)]
        public string Flag { get; set; }
        [StringLength(150)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string RefferenceNumber { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        // Custom Property 
        [StringLength(100)]
        public string LineNumber { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        public string ModelName { get; set; }
        public int? ItemCount { get; set; }
    }
}
