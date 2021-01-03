using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IHandsetChangeTraceBusiness
    {

        bool UpdateAndChangeJobOrder(long jobId, string imei1, string imei2, long modelId, string color,long orgId,long branchId,long userId);
        HandsetChangeTrace GetOneJobByOrgId(long jobId,long orgId);
        bool StockOutHandset(string imei1,long orgId, long branchId, long userId);
        bool ExitJobOrderForIMEI(long jobId, long orgId, long branchId);
        bool IsDuplicateIMEI1(long jobId,string imei1, long orgId, long branchId);
        bool IsDuplicateIMEI2(long jobId,string imei2, long orgId, long branchId);
        IEnumerable<HandsetChangeInformationDTO> GetHandsetChangeList(long orgId, long branchId, string fromDate, string toDate);
    }
}
