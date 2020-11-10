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
    public class CategoryRepository : SalesAndDistributionBaseRepository<Category>
    {
        public CategoryRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class BrandRepository : SalesAndDistributionBaseRepository<Brand>
    {
        public BrandRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class BrandCategoriesRepository : SalesAndDistributionBaseRepository<BrandCategories>
    {
        public BrandCategoriesRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class ModelRepository : SalesAndDistributionBaseRepository<Description>
    {
        public ModelRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class ColorRepository : SalesAndDistributionBaseRepository<Color>
    {
        public ColorRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class ModelColorsRepository : SalesAndDistributionBaseRepository<ModelColors>
    {
        public ModelColorsRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class RSMRepository : SalesAndDistributionBaseRepository<RSM>
    {
        public RSMRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class DivisionRepository : SalesAndDistributionBaseRepository<Division>
    {
        public DivisionRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class DistrictRepository : SalesAndDistributionBaseRepository<District>
    {
        public DistrictRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class ZoneRepository : SalesAndDistributionBaseRepository<Zone>
    {
        public ZoneRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class ASMRepository : SalesAndDistributionBaseRepository<ASM>
    {
        public ASMRepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
    public class TSERepository : SalesAndDistributionBaseRepository<TSE>
    {
        public TSERepository(ISalesAndDistributionUnitOfWork salesAndDistributionUnitOfWork) :
            base(salesAndDistributionUnitOfWork)
        {

        }
    }
}
