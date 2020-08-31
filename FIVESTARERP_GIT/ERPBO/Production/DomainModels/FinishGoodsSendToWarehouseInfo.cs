using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblFinishGoodsSendToWarehouseInfo")]
    public class FinishGoodsSendToWarehouseInfo
    {
        [Key]
        public long SendId { get; set; }
        public long LineId { get; set; }
        public long PackagingLineId { get; set; }
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
        public ICollection<FinishGoodsSendToWarehouseDetail> FinishGoodsSendToWarehouseDetails { get; set; }

        // Cartoon Info //
        [StringLength(100)]
        public string CartoonNo { get; set; }
        [StringLength(100)]
        public string Width { get; set; }
        [StringLength(100)]
        public string Height { get; set; }
        [StringLength(150)]
        public string GrossWeight { get; set; }
        [StringLength(150)]
        public string NetWeight { get; set; }
        public int TotalQty { get; set; }
    }
}
