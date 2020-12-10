using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblScrapStockDetail")]
    public class ScrapStockDetail
    {
        [Key]
        public long ScrapStockDetailId { get; set; }
        public long FaultyStockAssignTSId { get; set; }
        public long? FaultyStockInfoId { get; set; }
        public long? DescriptionId { get; set; }
        public long? PartsId { get; set; }
        public long? TSId { get; set; }
        public int Quantity { get; set; }
        public int RepairedQuantity { get; set; }
        public String StockStatus { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long ScrapStockInfoId { get; set; }
    }
}
