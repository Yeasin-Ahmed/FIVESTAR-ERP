﻿using ERPBO.FrontDesk.DomainModels;
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
}
