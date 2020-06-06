using ERPDAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.FrontDeskDAL
{
    public class FrontDeskUnitOfWork : IFrontDeskUnitOfWork
    {
        private readonly FrontDeskDbContext _dbcontext;
        public FrontDeskUnitOfWork() {
            _dbcontext = new FrontDeskDbContext();
        }
        public DbContext Db { get { return _dbcontext; } }

        public void Dispose()
        {
            
        }
    }
}
