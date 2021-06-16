using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("tblJobOrders")]
    public class JobOrder
    {
        [Key]
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
        public List<JobOrderAccessories> JobOrderAccessories { get; set; }
        public List<JobOrderProblem> JobOrderProblems { get; set; }
        public ICollection<JobOrderFault> JobOrderFaults { get; set; }
        public ICollection<JobOrderService> JobOrderServices { get; set; }
        public long? BranchId { get; set; }
        public ICollection<JobOrderTS> JobOrderTS { get; set; }
        public long? JobLocation { get; set; }
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
        public string TsRepairStatus { get; set; }
        public Nullable<DateTime> CloseDate { get; set; }
        public long? CUserId { get; set; }
        public long InvoiceInfoId { get; set; }
        public string InvoiceCode { get; set; }
        public string CustomerType { get; set; }
        //courier number---courier Name--Approx Bill
        public string CourierNumber { get; set; }
        public string CourierName { get; set; }
        public string ApproxBill { get; set; }
        public bool? IsTransfer { get; set; }
        public long? TransferBranchId { get; set; }
        public bool? IsReturn { get; set; }
        public bool? IsHandset { get; set; }
        ///Call Center
        public string CustomerApproval { get; set; }
        public string CallCenterRemarks { get; set; }
        //
        public string QCStatus { get; set; }
        public string QCRemarks { get; set; }
        //
        public string MultipleDeliveryCode { get; set; }
        public string QCTransferStatus { get; set; }
        //31-03-2021
        public string CustomerSupportStatus { get; set; }
        public string CSIMEI1 { get; set; }
        public string CSIMEI2 { get; set; }
        public long? CSModel { get; set; }
        public long? CSColor { get; set; }
        public string MultipleJobOrderCode { get; set; }
        public string JobSource { get; set; }
        public Nullable<DateTime> QCPassFailDate { get; set; }

    }
}
