using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ProductionDAL
{
    public class RequsitionInfoRepository : ProductionBaseRepository<RequsitionInfo>
    {
        public RequsitionInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class RequsitionDetailRepository : ProductionBaseRepository<RequsitionDetail>
    {
        public RequsitionDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class ProductionLineRepository : ProductionBaseRepository<ProductionLine>
    {
        public ProductionLineRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class ProductionStockInfoRepository : ProductionBaseRepository<ProductionStockInfo>
    {
        public ProductionStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class ProductionStockDetailRepository : ProductionBaseRepository<ProductionStockDetail>
    {
        public ProductionStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class ItemReturnInfoRepository : ProductionBaseRepository<ItemReturnInfo>
    {
        public ItemReturnInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class ItemReturnDetailRepository : ProductionBaseRepository<ItemReturnDetail>
    {
        public ItemReturnDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class DescriptionRepository : ProductionBaseRepository<Description>
    {
        public DescriptionRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class FinishGoodsInfoRepository : ProductionBaseRepository<FinishGoodsInfo>
    {
        public FinishGoodsInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class FinishGoodsRowMaterialRepository : ProductionBaseRepository<FinishGoodsRowMaterial>
    {
        public FinishGoodsRowMaterialRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class FinishGoodsStockInfoRepository : ProductionBaseRepository<FinishGoodsStockInfo>
    {
        public FinishGoodsStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class FinishGoodsStockDetailRepository : ProductionBaseRepository<FinishGoodsStockDetail>
    {
        public FinishGoodsStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

}
