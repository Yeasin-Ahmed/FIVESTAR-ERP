﻿using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ConfigurationDAL
{
   public class ConfigurationDbContext:DbContext
    {
        public ConfigurationDbContext() : base("Configuration")
        {

        }
        public DbSet<Accessories> tblAccessories { get; set; }
        public DbSet<ClientProblem> tblClientProblems { get; set; }
        public DbSet<MobilePart> tblMobileParts { get; set; }
        public DbSet<Customer> tblCustomers { get; set; }
        public DbSet<TechnicalServiceEng> tblTechnicalServiceEngs { get; set; }
        public DbSet<CustomerService> tblCustomerServices { get; set; }
        public DbSet<ServiceWarehouse> tblServicesWarehouses { get; set; }
        public DbSet<MobilePartStockInfo> tblMobilePartStockInfo { get; set; }
        public DbSet<MobilePartStockDetail> tblMobilePartStockDetails { get; set; }
    }
}
