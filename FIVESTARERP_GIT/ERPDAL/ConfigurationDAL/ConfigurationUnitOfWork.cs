using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ConfigurationDAL
{
    public class ConfigurationUnitOfWork : IConfigurationUnitOfWork
    {
        private readonly ConfigurationDbContext _dbcontext;
        public ConfigurationUnitOfWork()
        {
            _dbcontext = new ConfigurationDbContext();
        }
        public DbContext Db { get { return _dbcontext; } }

        public void Dispose()
        {

        }
    }
}
