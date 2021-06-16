using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
    public class HandSetStockDTO
    {
        public long HandSetStockId { get; set; }
        public string IMEI { get; set; }
        public string IMEI1 { get; set; }
        public long DescriptionId { get; set; }
        public long ColorId { get; set; }
        public string StockType { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public string Remarks { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string StateStatus { get; set; }

        //Custom
        public string ModelName { get; set; }
        public string ColorName { get; set; }
        public string ModelId { get; set; }
        public string Flag { get; set; }

    }
}
