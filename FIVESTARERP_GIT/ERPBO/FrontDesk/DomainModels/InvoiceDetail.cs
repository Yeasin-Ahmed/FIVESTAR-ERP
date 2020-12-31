using ERPBO.ControlPanel.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("InvoiceDetails")]
   public class InvoiceDetail
    {
        [Key]
        public long InvoiceDetailId { get; set; }
        public long? PartsId { get; set; }
        public string PartsName { get; set; }
        public int Quantity { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("InvoiceInfo")]
        public long InvoiceInfoId { get; set; }
        public InvoiceInfo InvoiceInfo { get; set; }
        public string SalesType { get; set; }
        public string IMEI { get; set; }
        public long? ModelId { get; set; }
        public string ModelName { get; set; }
    }
}
