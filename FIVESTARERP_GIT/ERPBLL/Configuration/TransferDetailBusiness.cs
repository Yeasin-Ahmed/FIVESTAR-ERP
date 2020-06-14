using ERPBLL.Configuration.Interface;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class TransferDetailBusiness: ITransferDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferDetailRepository transferDetailRepository; // repo
        public TransferDetailBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            transferDetailRepository = new TransferDetailRepository(this._configurationDb);
        }
    }
}
