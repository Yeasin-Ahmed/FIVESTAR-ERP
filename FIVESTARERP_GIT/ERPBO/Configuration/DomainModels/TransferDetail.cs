using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblTransferDetails")]
   public class TransferDetail
    {
        [Key]
        public long TransferDetailId { get; set; }
        public long? PartsId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Navigation P
        [ForeignKey("TransferInfo")]
        public long TransferInfoId { get; set; }
        public TransferInfo TransferInfo { get; set; }
        //
        public long? BranchTo { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        //Nishad
        public long? DescriptionId { get; set; }
    }
}
