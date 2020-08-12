using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{    
    public class MiniStockTransferInfoViewModel
    {
        public long MSTInfoId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        [Range(1,long.MaxValue)]
        public long FloorId { get; set; }
        [Range(1, long.MaxValue)]
        public long PackagingLineId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public List<MiniStockTransferDetailViewModel> MiniStockTransferDetails { get; set; }
        // Custom Properties //
        [StringLength(100)]
        public string FloorName { get; set; }
        [StringLength(100)]
        public string PackagingLineName { get; set; }
        [StringLength(200)]
        public string ModelNames { get; set; }
        [StringLength(100)]
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }
    }
}
