using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ConfigurationDAL
{
    public class AccessoriesRepository : ConfigurationBaseRepository<Accessories>
    {
        public AccessoriesRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class ClientProblemRepository : ConfigurationBaseRepository<ClientProblem>
    {
        public ClientProblemRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class MobilePartRepository : ConfigurationBaseRepository<MobilePart>
    {
        public MobilePartRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class CustomerRepository : ConfigurationBaseRepository<Customer>
    {
        public CustomerRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class TechnicalServiceRepository : ConfigurationBaseRepository<TechnicalServiceEng>
    {
        public TechnicalServiceRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class CustomerServiceRepository : ConfigurationBaseRepository<CustomerService>
    {
        public CustomerServiceRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class ServicesWarehouseRepository : ConfigurationBaseRepository<ServiceWarehouse>
    {
        public ServicesWarehouseRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class MobilePartStockInfoRepository : ConfigurationBaseRepository<MobilePartStockInfo>
    {
        public MobilePartStockInfoRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class MobilePartStockDetailRepository : ConfigurationBaseRepository<MobilePartStockDetail>
    {
        public MobilePartStockDetailRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class BranchRepository : ConfigurationBaseRepository<Branch>
    {
        public BranchRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class TransferInfoRepository : ConfigurationBaseRepository<TransferInfo>
    {
        public TransferInfoRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class TransferDetailRepository : ConfigurationBaseRepository<TransferDetail>
    {
        public TransferDetailRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class FaultRepository : ConfigurationBaseRepository<Fault>
    {
        public FaultRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class ServiceRepository : ConfigurationBaseRepository<Service>
    {
        public ServiceRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class RepairRepository : ConfigurationBaseRepository<Repair>
    {
        public RepairRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class WorkShopRepository : ConfigurationBaseRepository<WorkShop>
    {
        public WorkShopRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork) { }
    }
    public class FaultyStockDetailRepository : ConfigurationBaseRepository<FaultyStockDetails>
    {
        public FaultyStockDetailRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork)
        {

        }
    }
    public class FaultyStockInfoRepository : ConfigurationBaseRepository<FaultyStockInfo>
    {
        public FaultyStockInfoRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork)
        {

        }
    }
    public class HandSetStockRepository : ConfigurationBaseRepository<HandSetStock>
    {
        public HandSetStockRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork)
        {

        }
    }
    public class MissingStockRepository : ConfigurationBaseRepository<MissingStock>
    {
        public MissingStockRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork)
        {

        }
    }
    public class StockTransferInfoModelToModelRepository : ConfigurationBaseRepository<StockTransferInfoModelToModel>
    {
        public StockTransferInfoModelToModelRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork)
        {

        }
    }
    public class StockTransferDetailModelToModelRepository : ConfigurationBaseRepository<StockTransferDetailModelToModel>
    {
        public StockTransferDetailModelToModelRepository(IConfigurationUnitOfWork configurationUnitOfWork) : base(configurationUnitOfWork)
        {

        }
    }
}
