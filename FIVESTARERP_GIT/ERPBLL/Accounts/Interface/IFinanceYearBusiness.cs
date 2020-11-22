using ERPBO.Accounts.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface IFinanceYearBusiness
    {
        IEnumerable<FinanceYearDTO> GetAllFinanceList(long orgId);
        bool SaveYear(FinanceYearDTO yearDTO, long userId, long orgId);
    }
}
