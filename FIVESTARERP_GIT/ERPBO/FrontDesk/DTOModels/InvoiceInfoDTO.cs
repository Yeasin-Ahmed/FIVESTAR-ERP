using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class InvoiceInfoDTO
    {
        public long InvoiceInfoId { get; set; }
        public string InvoiceCode { get; set; }
        public long JobOrderId { get; set; }
        public string JobOrderCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public double TotalSPAmount { get; set; }
        public double LabourCharge { get; set; }
        public double VAT { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public double Total { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? WarrentyFor { get; set; }
        public string InvoiceType { get; set; }
    }
}
