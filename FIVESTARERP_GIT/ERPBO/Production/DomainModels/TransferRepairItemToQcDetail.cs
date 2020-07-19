﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblTransferRepairItemToQcDetail")]
    public class TransferRepairItemToQcDetail
    {
        [Key]
        public long TRQDetailId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int Quantity { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        public long IncomingTransferId { get; set; }
        [StringLength(100)]
        public string IncomingTransferCode { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("TransferRepairItemToQcInfo")]
        public long TRQInfoId { get; set; }
        public TransferRepairItemToQcInfo TransferRepairItemToQcInfo { get; set; }
    }
}
