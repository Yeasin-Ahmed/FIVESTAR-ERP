using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Common
{
    public class RequisitionStatus
    {
        // Never change this fields values //
        public static readonly string Pending = "Pending";
        public static readonly string Rejected = "Rejected";
        public static readonly string Recheck = "Recheck";
        public static readonly string Approved = "Approved";
        public static readonly string Accepted = "Accepted";
        public static readonly string Canceled = "Canceled";
    }
    public class StockStatus
    {
        public static readonly string StockIn = "Stock-In";
        public static readonly string StockOut = "Stock-Out";
        public static readonly string StockReturn = "Stock-Return";
    }
    public class ReturnType
    {
        public static readonly string RepairFaultyReturn = "Repair Faulty Return";
        public static readonly string RepairGoodsReturn = "Repair Goods Return";
        public static readonly string ProductionGoodsReturn = "Production Goods Return";
        public static readonly string ProductionFaultyReturn = "Production Faulty Return";
    }
    public class FaultyCase
    {
        public static readonly string ManMade = "Man Made";
        public static readonly string ChinaMade = "China Made";
    }
    public class ProductionTypes
    {
        public static readonly string CKD = "CKD";
        public static readonly string SKD = "SKD";
        public static readonly string HandSet = "HandSet";
    }
    public class StockOutReason
    {
        public static readonly string StockOutByProductionForProduceGoods = "StockOutByProductionForProduceGoods";
    }
    public class FinishGoodsSendStatus
    {
        public static readonly string Send = "Send";
        public static readonly string Received = "Received";
    }
    public class UserType
    {
        public static readonly string SystemAdmin = "System Admin";
        public static readonly string Admin = "Admin";
    }
    public class RequisitionType
    {
        public static readonly string CKD = "CKD";
        public static readonly string SKD = "SKD";
    }
    public class Flag
    {
        public static readonly string View = "View";
        public static readonly string Search = "Search";
        public static readonly string Info = "Info";
        public static readonly string Detail = "Detail";
        public static readonly string Report = "Report";
        public static readonly string Delete = "Delete";
    }

    public class RequisitionExecuationType
    {
        public static readonly string Single = "Single";
        public static readonly string Bundle = "Bundle";
    }

}
