using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class InvoiceInfoViewModel
    {
        public long InvoiceInfoId { get; set; }
        [StringLength(100)]
        public string InvoiceCode { get; set; }
        //[Range(1, long.MaxValue)]
        public long JobOrderId { get; set; }
        [StringLength(100)]
        public string JobOrderCode { get; set; }
        [StringLength(100)]
        public string CustomerName { get; set; }
        [StringLength(100)]
        public string CustomerPhone { get; set; }
        public double TotalSPAmount { get; set; }
        public double LabourCharge { get; set; }
        public double VAT { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public double Total { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        public string WarrentyFor { get; set; }
        [StringLength(100)]
        public string InvoiceType { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
    }
}
