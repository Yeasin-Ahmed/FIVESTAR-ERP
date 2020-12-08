using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class StockTransferDetailModelToModelBusiness : IStockTransferDetailModelToModelBusiness
    {
        private readonly IConfigurationUnitOfWork _configDb;
        private readonly StockTransferDetailModelToModelRepository _stockTransferDetailModelToModelRepository;

        public StockTransferDetailModelToModelBusiness(IConfigurationUnitOfWork configDb)
        {
            this._configDb = configDb;
            _stockTransferDetailModelToModelRepository = new StockTransferDetailModelToModelRepository(this._configDb = configDb);
        }
        public IEnumerable<StockTransferDetailModelToModel> GetAllTransferDetailMMByInfoId(long transferId, long orgId)
        {
            return _stockTransferDetailModelToModelRepository.GetAll(ts => ts.OrganizationId == orgId && ts.TransferInfoModelToModelId == transferId).ToList();
        }
    }
}
