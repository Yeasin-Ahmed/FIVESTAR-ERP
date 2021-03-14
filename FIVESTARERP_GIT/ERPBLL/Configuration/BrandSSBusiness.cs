using ERPBLL.Configuration.Interface;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class BrandSSBusiness: IBrandSSBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly BrandSSRepository brandSSRepository; // repo
        public BrandSSBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            brandSSRepository = new BrandSSRepository(this._configurationDb);
        }
    }
}
