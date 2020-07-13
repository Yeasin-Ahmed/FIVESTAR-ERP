using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
    public class JobOrderDTO
    {
        public long JodOrderId { get; set; }
        [StringLength(100)]
        public string CustomerName { get; set; }
        [StringLength(15)]
        public string MobileNo { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        public long DescriptionId { get; set; }
        public bool IsWarrantyAvailable { get; set; }
        public bool IsWarrantyPaperEnclosed { get; set; }
        [StringLength(20)]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string JobOrderType { get; set; }
        public long? CustomerId { get; set; }
        public long? TSId { get; set; }
        [StringLength(100)]
        public string JobOrderCode { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        // Custom Properties
        public string ModelName { get; set; }
        public string AccessoriesNames { get; set; }
        public string  TSName { get; set; }
        public string Problems { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public long? BranchId { get; set; }

        //
        public string IMEI { get; set; }
        public string Type { get; set; }
        public string ModelColor { get; set; }
        public Nullable<DateTime> WarrantyDate { get; set; }
        public string Remarks { get; set; }
        public string ReferenceNumber { get; set; }
        public string IMEI2 { get; set; }
        public Nullable<DateTime> WarrantyEndDate { get; set; }
        public string TSRemarks { get; set; }
    }
}
