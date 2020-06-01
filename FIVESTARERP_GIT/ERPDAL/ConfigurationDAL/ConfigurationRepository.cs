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
}
