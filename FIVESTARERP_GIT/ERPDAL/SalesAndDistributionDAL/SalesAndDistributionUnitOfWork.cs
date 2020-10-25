using ERPDAL.Repository;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.SalesAndDistributionDAL
{
    public class SalesAndDistributionUnitOfWork : ISalesAndDistributionUnitOfWork
    {
        private readonly SalesAndDistributionDbContext _dbcontext;
        public SalesAndDistributionUnitOfWork() {
            _dbcontext = new SalesAndDistributionDbContext();
        }
        public DbContext Db { get { return _dbcontext; } }
        public void Dispose()
        {
            
        }
    }
}
