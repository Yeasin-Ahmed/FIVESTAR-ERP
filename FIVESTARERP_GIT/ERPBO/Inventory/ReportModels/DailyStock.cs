using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ReportModels
{
    // Item Wise / Model & Item Wise
    public class DailyStock
    {
        public string ReceiveDate { get; set; }
        public string ModelName { get; set; }
        public string ItemName { get; set; }
        public int StockInQty { get; set; }
        public int StockOutQty { get; set; }
        public int OpeningStock { get; set; }
        public int PackagingBomQty { get; set; }
        public int PhysicalReceivedQty { get; set; }
        public int ReceivedAsShortageQty { get; set; }
        public int ReceivedAsExcessQty { get; set; }
        public int TotalReceive { get; set; }
        public long ProductionIssueQty { get; set; }
        public long RepairIssueQty { get; set; }
        public int LabOrQcIssueQty { get; set; }
        public int HeadOfficeIssueQty { get; set; }
        public int GoodReturnQtyFromAssembly { get; set; }
        public int GoodReturnQtyFromOthers { get; set; }
        public int FaultyReturnQty { get; set; }
    }
}
