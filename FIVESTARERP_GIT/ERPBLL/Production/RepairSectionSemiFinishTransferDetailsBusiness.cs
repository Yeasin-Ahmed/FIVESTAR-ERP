using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
   public class RepairSectionSemiFinishTransferDetailsBusiness: IRepairSectionSemiFinishTransferDetailsBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairSectionSemiFinishTransferDetailsRepository _repairSectionSemiFinishTransferDetailsRepository;

        public RepairSectionSemiFinishTransferDetailsBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairSectionSemiFinishTransferDetailsRepository = new RepairSectionSemiFinishTransferDetailsRepository(this._productionDb);
        }

        public IEnumerable<RepairSectionSemiFinishTransferDetails> GetRepairSectionSemiFinishTransferDetails(long infoId,long orgId)
        {
            return _repairSectionSemiFinishTransferDetailsRepository.GetAll(d =>d.TransferInfoId==infoId && d.OrganizationId == orgId);
        }
    }
}
