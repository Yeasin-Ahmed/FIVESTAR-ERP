using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblFaultyStockDetails")]
    public class FaultyStockDetails
    {
        [Key]
        public long FaultyStockDetailId { get; set; }
        public long? DescriptionId { get; set; }
        public long? JobOrderId { get; set; }
        public long? SWarehouseId { get; set; }
        public long? PartsId { get; set; }
        public int Quantity { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public long TSId { get; set; }
        public long? BranchId { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long FaultyStockInfoId { get; set; }
    }
}
