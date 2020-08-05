﻿using System;
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
        public ICollection<JobOrderAccessories> JobOrderAccessories { get; set; }
        public ICollection<JobOrderProblem> JobOrderProblems { get; set; }
        public ICollection<JobOrderFault> JobOrderFaults { get; set; }
        public ICollection<JobOrderService> JobOrderServices { get; set; }
        public long? BranchId { get; set; }
        public ICollection<JobOrderTS> JobOrderTS { get; set; }
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

    }
}
