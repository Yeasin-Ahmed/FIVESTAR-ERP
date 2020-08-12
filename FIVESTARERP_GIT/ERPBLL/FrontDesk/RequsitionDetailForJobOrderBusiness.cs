using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
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

        public IEnumerable<RequsitionDetailForJobOrderDTO> GetAvailableQtyByRequsition(long reqInfoId, long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionDetailForJobOrderDTO>(
                string.Format(@"select rd.RequsitionInfoForJobOrderId,rd.RequsitionDetailForJobOrderId,rd.PartsId,parts.MobilePartName 'PartsName',parts.MobilePartCode,rd.Quantity,ISNULL(Sum(std.StockInQty-std.StockOutQty),0) 'AvailableQty' 
        from tblRequsitionDetailForJobOrders rd
        inner join [Configuration].dbo.tblMobilePartStockInfo std
        on rd.PartsId=std.MobilePartId
        left join [Configuration].dbo.tblMobileParts parts
        on rd.PartsId=parts.MobilePartId
        where rd.RequsitionInfoForJobOrderId={0} and rd.OrganizationId={1} and rd.BranchId={2}
        group by rd.RequsitionDetailForJobOrderId,rd.PartsId,rd.Quantity,parts.MobilePartName,parts.MobilePartCode,rd.RequsitionInfoForJobOrderId", reqInfoId, orgId, branchId)).ToList();
        }
    }
}
