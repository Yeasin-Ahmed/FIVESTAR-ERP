using ERPBO.ControlPanel.DomainModels;
using ERPBO.FrontDesk.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.FrontDeskDAL
{                                       
    public class JobOrderRepository: FrontDeskBaseRepository<JobOrder>
    {
        public JobOrderRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork ) { }
    }
    public class JobOrderAccessoriesRepository: FrontDeskBaseRepository<JobOrderAccessories>
    {
        public JobOrderAccessoriesRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class JobOrderProblemRepository: FrontDeskBaseRepository<JobOrderProblem>
    {
        public JobOrderProblemRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class RequsitionInfoForJobOrderRepository : FrontDeskBaseRepository<RequsitionInfoForJobOrder>
    {
        public RequsitionInfoForJobOrderRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class RequsitionDetailForJobOrderRepository : FrontDeskBaseRepository<RequsitionDetailForJobOrder>
    {
        public RequsitionDetailForJobOrderRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class TechnicalServicesStockRepository : FrontDeskBaseRepository<TechnicalServicesStock>
    {
        public TechnicalServicesStockRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class JobOrderTSRepository: FrontDeskBaseRepository<JobOrderTS>
    {
        public JobOrderTSRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class JobOrderFaultRepository : FrontDeskBaseRepository<JobOrderFault>
    {
        public JobOrderFaultRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class JobOrderServiceRepository : FrontDeskBaseRepository<JobOrderService>
    {
        public JobOrderServiceRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class JobOrderRepairRepository : FrontDeskBaseRepository<JobOrderRepair>
    {
        public JobOrderRepairRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class TsStockReturnInfoRepository : FrontDeskBaseRepository<TsStockReturnInfo>
    {
        public TsStockReturnInfoRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class TsStockReturnDetailRepository : FrontDeskBaseRepository<TsStockReturnDetail>
    {
        public TsStockReturnDetailRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class InvoiceInfoRepository : FrontDeskBaseRepository<InvoiceInfo>
    {
        public InvoiceInfoRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class InvoiceDetailRepository : FrontDeskBaseRepository<InvoiceDetail>
    {
        public InvoiceDetailRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class JobOrderTransferDetailRepository : FrontDeskBaseRepository<JobOrderTransferDetail>
    {
        public JobOrderTransferDetailRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class JobOrderReturnDetailRepository : FrontDeskBaseRepository<JobOrderReturnDetail>
    {
        public JobOrderReturnDetailRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base(frontDeskUnitOfWork) { }
    }
    public class FaultyStockAssignTSRepository : FrontDeskBaseRepository<FaultyStockAssignTS>
    {
        public FaultyStockAssignTSRepository(IFrontDeskUnitOfWork frontDeskUnitOfWork) : base (frontDeskUnitOfWork)
        {

        }
    }
}
