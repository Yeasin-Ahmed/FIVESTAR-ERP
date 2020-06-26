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
   public class RequsitionDetailForJobOrderBusiness: IRequsitionDetailForJobOrderBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly RequsitionDetailForJobOrderRepository requsitionDetailForJobOrderRepository;

        public RequsitionDetailForJobOrderBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.requsitionDetailForJobOrderRepository = new RequsitionDetailForJobOrderRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<RequsitionDetailForJobOrder> GetAllRequsitionDetailForJobOrder(long orgId, long branchId)
        {
            return requsitionDetailForJobOrderRepository.GetAll(detail => detail.OrganizationId == orgId && detail.BranchId == branchId).ToList();
        }

        public IEnumerable<RequsitionDetailForJobOrder> GetAllRequsitionDetailForJobOrderId(long reqInfoId, long orgId, long branchId)
        {
            return requsitionDetailForJobOrderRepository.GetAll(detail =>detail.RequsitionInfoForJobOrderId == reqInfoId && detail.OrganizationId == orgId && detail.BranchId == branchId).ToList();
        }
    }
}
