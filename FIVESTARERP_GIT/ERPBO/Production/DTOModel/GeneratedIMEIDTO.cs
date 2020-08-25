using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class GeneratedIMEIDTO
    {
        public long Id { get; set; }
        public long CodeId { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        public long DescriptionId { get; set; }
        public string DescriptionName { get; set; }
        public long? FloorId { get; set; }
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        public long? AssemblyLineId { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        public long? PackagingLineId { get; set; }
        [StringLength(100)]
        public string PackagingLineName { get; set; }
        public string IMEI { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Properties //
        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
    }
}
