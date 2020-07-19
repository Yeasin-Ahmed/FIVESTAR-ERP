using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IJobOrderProblemBusiness
    {
        JobOrderProblem GetJobOrderProblemById(long id, long orgId);
        IEnumerable<JobOrderProblem> GetJobOrderProblemByJobOrderId(long jobOrderId, long orgId);
        JobOrderProblem GetJobOrderProblemByJobId(long id, long orgId);

        bool SaveJobOrderProblem(List<JobOrderProblemDTO> jobOrderProblems, long userId, long orgId);
        bool IsDuplicateSymptomName(long id, long prblmId, long orgId);

    }
}
