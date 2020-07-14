﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.ControlPanel.DomainModels;
using ERPBO.FrontDesk.DomainModels;

namespace ERPDAL.FrontDeskDAL
{
    public class FrontDeskDbContext : DbContext
    {
        public FrontDeskDbContext():base("FrontDesk")
        {

        }
        public DbSet<JobOrder> tblJobOrders { get; set; }
        public DbSet<JobOrderAccessories> tblJobOrderAccessories { get; set; }
        public DbSet<JobOrderProblem> tblJobOrderProblems { get; set; }
        public DbSet<RequsitionInfoForJobOrder> tblRequsitionInfoForJobOrders { get; set; }
        public DbSet<RequsitionDetailForJobOrder> tblRequsitionDetailForJobOrders { get; set; }
        public DbSet<TechnicalServicesStock> tblTechnicalServicesStocks { get; set; }
        public DbSet<JobOrderFault> tblJobOrderFault { get; set; }
        public DbSet<JobOrderService> tblJobOrderServices { get; set; }
        public DbSet<JobOrderRepair> tblJobOrderRepair { get; set; }
        public DbSet<TsStockReturnInfo> tblTsStockReturnInfo { get; set; }
        public DbSet<TsStockReturnDetail> tblTsStockReturnDetails { get; set; }
    }
}
