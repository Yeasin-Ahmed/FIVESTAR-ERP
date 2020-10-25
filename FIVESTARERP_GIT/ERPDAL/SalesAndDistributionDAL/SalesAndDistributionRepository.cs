using ERPBO.Production.DomainModels;
using ERPBO.SalesAndDistribution.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.SalesAndDistributionDAL
{
    public class DealerRepository : SalesAndDistributionBaseRepository<Dealer>
    {
        public DealerRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) : 
            base(salesAndDistributionUnitOfWork)
        {

        }
    }

    public class BTRCApprovedIMEIRepository : SalesAndDistributionBaseRepository<BTRCApprovedIMEI>
    {
        public BTRCApprovedIMEIRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class ItemStockRepository: SalesAndDistributionBaseRepository<ItemStock>
    {
        public ItemStockRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }

}
