using ERPBO.Accounts.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts.Interface
{
   public interface IJournalBusiness
    {
        bool SaveDebitVouchar(List<JournalDTO> journalDTO, long userId, long orgId);
        bool SaveCreditVouchar(List<JournalDTO> journalDTO, long userId, long orgId);
        bool SaveJournalVouchar(List<JournalDTO> journalDTO, long userId, long orgId);
        IEnumerable<JournalDTO> GetJournalList(long orgId, string fromDate, string toDate);
        IEnumerable<JournalDTO> CashVoucherList(long orgId, string fromDate, string toDate);
        IEnumerable<JournalDTO> LedgerList(long? accountId,long orgId, string fromDate, string toDate);
    }
}
