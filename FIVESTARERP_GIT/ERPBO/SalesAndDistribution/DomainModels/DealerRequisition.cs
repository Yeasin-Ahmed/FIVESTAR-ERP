using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.DomainModels
{
    [Table("tblDealerRequisitionInfo")]
    public class DealerRequisitionInfo
    {
        [Key]
        public long DREQInfoId { get; set; }
        public long DealerId { get; set; }
        [StringLength(100)]
        public string RequisitionCode { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public List<DealerRequisitionDetail> DealerRequisitionDetails { get; set; }
    }

    [Table("tblDealerRequisitionDetail")]
    public class DealerRequisitionDetail
    {
        [Key]
        public long DREQDetailId { get; set; }
        public long CategoryId { get; set; }
        public long BrandId { get; set; }
        public long ModelId { get; set; }
        public long ColorId { get; set; }
        public int Quantity { get; set; }
        public int IssueQuantity { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        [ForeignKey("DealerRequisitionInfo")]
        public long DREQInfoId { get; set; }
        public DealerRequisitionInfo DealerRequisitionInfo { get; set; }
    }
}
