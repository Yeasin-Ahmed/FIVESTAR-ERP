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
    public class TransferFromQCDetailBusiness : ITransferFromQCDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferFromQCDetailRepository _transferFromQCDetailRepository;

        public TransferFromQCDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferFromQCDetailRepository = new TransferFromQCDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferFromQCDetail> GetTransferFromQCDetailByInfo(long infoId, long orgId)
        {
            return _transferFromQCDetailRepository.GetAll(t => t.TSQInfoId == infoId && t.OrganizationId == orgId).ToList();
        }

        public async Task<IEnumerable<TransferFromQCDetail>> GetTransferFromQCDetailByInfoAsync(long infoId, long orgId)
        {
            return await _transferFromQCDetailRepository.GetAllAsync(t => t.TSQInfoId == infoId && t.OrganizationId == orgId);
        }

        public IEnumerable<TransferFromQCDetail> GetTransferFromQCDetails(long orgId)
        {
            return _transferFromQCDetailRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
    }
}
