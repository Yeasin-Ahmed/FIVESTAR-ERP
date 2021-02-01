using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface IChequeBookBusiness
    {
        IEnumerable<ChequeBookDTO> GetChequeBookList(long orgId,string accName,string accNo,string chqType,string bankName,string chqNo, string fromDate, string toDate);
        bool SaveChequeBook(ChequeBookDTO dto,long userId, long orgId);
        ChequeBook GetCheckBookOneByOrgId(long chbId, long orgId);
    }
}
