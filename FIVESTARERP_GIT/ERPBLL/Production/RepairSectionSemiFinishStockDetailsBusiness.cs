using ERPBLL.Production.Interface;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
   public class RepairSectionSemiFinishStockDetailsBusiness: IRepairSectionSemiFinishStockDetailsBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairSectionSemiFinishStockDetailsRepository _repairSectionSemiFinishStockDetailsRepository;

        public RepairSectionSemiFinishStockDetailsBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairSectionSemiFinishStockDetailsRepository = new RepairSectionSemiFinishStockDetailsRepository(this._productionDb);
        }
    }
}
