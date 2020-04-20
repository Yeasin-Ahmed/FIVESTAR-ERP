using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class FinishGoodsSendToWarehouseDetailDTO
    {
        public long SendDetailId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public long UnitId { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(150)]
        public string Flag { get; set; }
        [StringLength(100)]
        public string RefferenceNumber { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        // Custom Property //
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
    }
}
