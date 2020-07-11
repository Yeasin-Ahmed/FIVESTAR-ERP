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
    public class RepairSectionFaultyItemTransferDetailBusiness : IRepairSectionFaultyItemTransferDetailBusiness
    {
        private readonly IProductionUnitOfWork _production;
        private readonly RepairSectionFaultyItemTransferDetailRepository _repairSectionFaultyItemTransferDetailRepository;

        public RepairSectionFaultyItemTransferDetailBusiness(IProductionUnitOfWork production)
        {
            this._production = production;
            this._repairSectionFaultyItemTransferDetailRepository = new RepairSectionFaultyItemTransferDetailRepository(this._production);
        }

        public IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetailByInfo(long transferInfoId, long orgId)
        {
            return _repairSectionFaultyItemTransferDetailRepository.GetAll(s => s.RSFIRInfoId == transferInfoId && s.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetails(long orgId)
        {
            throw new NotImplementedException();
        }
    }
}
