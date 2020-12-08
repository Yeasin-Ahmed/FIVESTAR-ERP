using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
    public class StockTransferInfoModelToModelDTO
    {
        public long TransferInfoModelToModelId { get; set; }
        public string TransferCode { get; set; }
        public long? DescriptionId { get; set; }
        public long? ToDescriptionId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ToWarehouseId { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<StockTransferDetailModelToModel> StockTransferDetailModelToModels { get; set; }
        //custom p
        public string SWarehouseName { get; set; }
        public string BranchName { get; set; }
        public string FromModelName { get; set; }
        public string ToModelName { get; set; }
    }
}
