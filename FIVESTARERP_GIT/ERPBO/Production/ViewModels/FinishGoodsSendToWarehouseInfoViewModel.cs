using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class FinishGoodsSendToWarehouseInfoViewModel
    {
        public long SendId { get; set; }
        [Range(1, long.MaxValue)]
        public long LineId { get; set; }
        [Range(1, long.MaxValue)]
        public long PackagingLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
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
        public string PackagingLineName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        public string ModelName { get; set; }
        public int? ItemCount { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }

        public List<FinishGoodsSendToWarehouseDetailViewModel> FinishGoodsSendToWarehouseDetails { get; set; }
        // Cartoon Info //
        [Required,StringLength(100)]
        public string CartoonNo { get; set; }
        [StringLength(100)]
        public string Width { get; set; }
        [StringLength(100)]
        public string Height { get; set; }
        [StringLength(150)]
        public string GrossWeight { get; set; }
        [StringLength(150)]
        public string NetWeight { get; set; }
        [Range(1,int.MaxValue)]
        public int TotalQty { get; set; }
    }
}
