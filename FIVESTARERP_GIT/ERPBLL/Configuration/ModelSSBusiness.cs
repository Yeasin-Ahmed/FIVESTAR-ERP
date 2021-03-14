using ERPBLL.Configuration.Interface;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class ModelSSBusiness: IModelSSBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly ModelSSRepository modelSSRepository; // repo
        public ModelSSBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            modelSSRepository = new ModelSSRepository(this._configurationDb);
        }
    }
}
