using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class DailySummaryReportDTO
    {
        public DateTime EntryDate { get; set; }
        public int? DailyJobOrder { get; set; }
        public int? BillingJob { get; set; }
        public int? WarrentyJob { get; set; }
        public double? CASH { get; set; }
        public int? TSBacklog { get; set; }
        public int? TSOverAllBacklog { get; set; }
        public int? TSSignInPending { get; set; }
        public int? RepairAndReturn { get; set; }
        public int? ReturnWithoutRepair { get; set; }
        public int? WorkInProgress { get; set; }
        public int? PendingForSpareParts { get; set; }
        public int? PendingForApproval { get; set; }
        public int? WarrentyDelivery { get; set; }
        public int? BillingDelivery { get; set; }
        public int? TotalDelivery { get; set; }
        public int? DeliveryPending { get; set; }
        public int? OverAllWarrentyUnDelivered { get; set; }
        public int? OverAllBillingUnDelivered { get; set; }
        public int? TotalUnDelivered { get; set; }
        public string BranchWiseDailyJob { get; set; }
        public int? TOTAL { get; set; }
        public double? AccessoriesSells { get; set; }
        public string BranchName { get; set; }
    }
}
