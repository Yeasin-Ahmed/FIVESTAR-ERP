using ERPBLL.Accounts.Interface;
using ERPBLL.Common;
using ERPBO.Accounts.DomainModels;
using ERPBO.Accounts.DTOModels;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts
{
   public class ChequeBookBusiness: IChequeBookBusiness
    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly ChequeBookRepository _chequeBookRepository; // repo
        public ChequeBookBusiness(IAccountsUnitOfWork accountsDb)
        {
            this._accountsDb = accountsDb;
            _chequeBookRepository = new ChequeBookRepository(this._accountsDb);
        }

        public ChequeBook GetCheckBookOneByOrgId(long chbId, long orgId)
        {
            return _chequeBookRepository.GetOneByOrg(c => c.ChequeBookId == chbId && c.OrganizationId == orgId);
        }

        public IEnumerable<ChequeBookDTO> GetChequeBookList(long orgId, string accName, string accNo, string chqType, string bankName, string chqNo, string fromDate, string toDate)
        {
            return this._accountsDb.Db.Database.SqlQuery<ChequeBookDTO>(QueryForChequeList(orgId, accName, accNo, chqType,bankName,chqNo, fromDate, toDate)).ToList();
        }
        private string QueryForChequeList(long orgId, string accName, string accNo, string chqType, string bankName, string chqNo, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@"and chb.OrganizationId={0}", orgId);
            }
            if (!string.IsNullOrEmpty(accName))
            {
                param += string.Format(@"and chb.AccName Like '%{0}%'", accName);
            }
            if (!string.IsNullOrEmpty(accNo))
            {
                param += string.Format(@"and chb.AccountNumber Like '%{0}%'", accNo);
            }
            if (!string.IsNullOrEmpty(chqType))
            {
                param += string.Format(@"and chb.CheckType Like '%{0}%'", chqType);
            }
            if (!string.IsNullOrEmpty(bankName))
            {
                param += string.Format(@"and chb.BankName Like '%{0}%'", bankName);
            }
            if (!string.IsNullOrEmpty(chqNo))
            {
                param += string.Format(@"and chb.CheckNo Like '%{0}%'", chqNo);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(chb.CheckDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(chb.CheckDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(chb.CheckDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select chb.CheckDate,chb.PayOrReceive,chb.AccName,chb.AccountNumber,chb.CheckType,
chb.CheckNo,chb.Amount,chb.BankName,chb.BranchName,chb.Remarks,apu.UserName from tblChequeBooks chb
Left join [ControlPanel].dbo.tblApplicationUsers apu
on chb.EUserId=apu.UserId
 Where 1=1{0}", Utility.ParamChecker(param));
            return query;
        
        }

        public bool SaveChequeBook(ChequeBookDTO dto,long userId, long orgId)
        {
            if (dto.ChequeBookId == 0)
            {
                ChequeBook cheque = new ChequeBook();
                cheque.CheckDate = dto.CheckDate;
                cheque.CheckType = dto.CheckType;
                cheque.AccName = dto.AccName;
                cheque.AccountNumber = dto.AccountNumber;
                cheque.CheckNo = dto.CheckNo;
                cheque.Amount = dto.Amount;
                cheque.BankName = dto.BankName;
                cheque.BranchName = dto.BranchName;
                cheque.PayOrReceive = dto.PayOrReceive;
                cheque.Remarks = dto.Remarks;
                cheque.OrganizationId = orgId;
                cheque.EUserId = userId;
                cheque.EntryDate = DateTime.Now;
                cheque.Flag = "";
                _chequeBookRepository.Insert(cheque);
            }
            else
            {
                var chbook = GetCheckBookOneByOrgId(dto.ChequeBookId, orgId);
                chbook.CheckDate = dto.CheckDate;
                chbook.CheckType = dto.CheckType;
                chbook.AccName = dto.AccName;
                chbook.AccountNumber = dto.AccountNumber;
                chbook.CheckNo = dto.CheckNo;
                chbook.Amount = dto.Amount;
                chbook.BankName = dto.BankName;
                chbook.BranchName = dto.BranchName;
                chbook.PayOrReceive = dto.PayOrReceive;
                chbook.Remarks = dto.Remarks;
                chbook.UpUserId = userId;
                chbook.UpdateDate = DateTime.Now;
                chbook.Flag = "";
                _chequeBookRepository.Update(chbook);

            }
            return _chequeBookRepository.Save();
        }
    }
}
