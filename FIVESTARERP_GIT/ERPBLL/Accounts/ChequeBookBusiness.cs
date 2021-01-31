using ERPBLL.Accounts.Interface;
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

        public IEnumerable<ChequeBookDTO> GetChequeBookList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<ChequeBookDTO>(
                string.Format(@"Select * from tblChequeBooks chb
Left join [ControlPanel].dbo.tblApplicationUsers apu
on chb.EUserId=apu.UserId
 Where chb.OrganizationId={0}", orgId)).ToList();
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
            return _chequeBookRepository.Save();
        }
    }
}
