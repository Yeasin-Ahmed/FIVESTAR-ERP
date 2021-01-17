using ERPBO.Accounts.DTOModels;
using ERPBO.Common;
using ERPBO.FrontDesk.ReportModels;
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
        ExecutionStateWithText SaveDebitVoucharAndPrint(List<JournalDTO> journalDTO, long userId, long orgId);
        IEnumerable<JournalDTO> GetDebitVoucherReport(string voucherNo, long orgId);
        ServicesReportHead GetBranchInformation(long orgId, long branchId);
        IEnumerable<JournalDTO> GetCreditVoucherReport(string voucherNo, long orgId);
        ExecutionStateWithText SaveCreditVoucharAndPrint(List<JournalDTO> journalDTO, long userId, long orgId);
        ExecutionStateWithText SaveJournalVoucharAndPrint(List<JournalDTO> journalDTO, long userId, long orgId);
        IEnumerable<JournalDTO> GetJournalVoucherReport(string voucherNo, long orgId);
    }
}
