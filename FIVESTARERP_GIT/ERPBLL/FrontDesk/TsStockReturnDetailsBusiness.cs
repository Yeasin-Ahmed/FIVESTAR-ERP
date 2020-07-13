using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
   public class TsStockReturnDetailsBusiness: ITsStockReturnDetailsBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly TsStockReturnDetailRepository _tsStockReturnDetailRepository;

        public TsStockReturnDetailsBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._tsStockReturnDetailRepository = new TsStockReturnDetailRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<TsStockReturnDetail> GetAllTsStockReturn(long orgId, long BranchId)
        {
            return _tsStockReturnDetailRepository.GetAll(detail => detail.OrganizationId == orgId && detail.BranchId == BranchId);
        }
    }
}
