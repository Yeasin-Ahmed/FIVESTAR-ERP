using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class DealerRequisitionInfoViewModel
    {
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
        public List<DealerRequisitionDetailViewModel> DealerRequisitionDetails { get; set; }
        // Custom Properties
        public string DealerName { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Zone { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ApprovedUser { get; set; }
        [Required]
        public string Flag { get; set; }
    }
    public class DealerRequisitionDetailViewModel
    {
        public long DREQDetailId { get; set; }
        [Range(1,long.MaxValue)]
        public long CategoryId { get; set; }
        [Range(1, long.MaxValue)]
        public long BrandId { get; set; }
        [Range(1, long.MaxValue)]
        public long ModelId { get; set; }
        [Range(1, long.MaxValue)]
        public long ColorId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public int IssueQuantity { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public long DREQInfoId { get; set; }
        // Custom Properties
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string ColorName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ApprovedUser { get; set; }
    }
}
