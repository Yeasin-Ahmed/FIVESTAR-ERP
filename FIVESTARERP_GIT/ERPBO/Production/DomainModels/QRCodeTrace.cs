﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblQRCodeTrace")]
    public class QRCodeTrace
    {
        [Key]
        public long CodeId { get; set; }
        [StringLength(200)]
        public string CodeNo { get; set; }
        public byte?[] CodeImage { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? DescriptionId { get; set; }
        public long? ColorId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        [StringLength(150)]
        public string ColorName { get; set; }
        [StringLength(200)]
        public string ReferenceNumber { get; set; }
        public string ReferenceId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}