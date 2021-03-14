using ERPBLL.Configuration.Interface;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class ColorSSBusiness: IColorSSBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly ColorSSRepository colorSSRepository; // repo
        public ColorSSBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            colorSSRepository = new ColorSSRepository(this._configurationDb);
        }
    }
}
