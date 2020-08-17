using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
    public class IQCItemReqInfoListDTO
    {
        public long IQCItemReqInfoId { get; set; }
        public string IQCReqCode { get; set; }
        public long? IQCId { get; set; }
        public long? WarehouseId { get; set; }
        public long? DescriptionId { get; set; }
        public long? SupplierId { get; set; }
        public string Remarks { get; set; }
        public string StateStatus { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ReturnUserId { get; set; }
        public Nullable<DateTime> ReturnUserDate { get; set; }
        public long? ReturnReaciveUserId { get; set; }
        public Nullable<DateTime> ReturnReaciveUserDate { get; set; }
        public string Warehouse { get; set; }
        public string ModelName { get; set; }
        public string IQCName { get; set; }
        public string Supplier { get; set; }
        public string EntryUser { get; set; }
        public string ReturnUser{ get; set; }
        public string ReturnReaciveUser { get; set; }
        public decimal AvailableQty { get; set; }
        public List<IQCItemReqDetailListDTO> IQCItemReqDetails { get; set; }
    }
}
