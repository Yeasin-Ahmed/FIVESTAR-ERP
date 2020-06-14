using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
   public class TransferInfoDTO
    {
        public long TransferInfoId { get; set; }
        public string TransferCode { get; set; }
        public long BranchTo { get; set; }
        public long? SWarehouseId { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //custom p
        public string SWarehouseName { get; set; }
        public string BranchName { get; set; }
    }
}
