using ERPBLL.Accounts.Interface;
using ERPBO.Accounts.DTOModels;
using ERPDAL.AccountsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Accounts
{
   public class SupplierBusiness: ISupplierBusiness
    {
        private readonly IAccountsUnitOfWork _accountsDb; // database
        private readonly SupplierRepository _supplierRepository; // repo
        public SupplierBusiness(IAccountsUnitOfWork accountsDb)
        {
            this._accountsDb = accountsDb;
            _supplierRepository = new SupplierRepository(this._accountsDb);
        }

        public IEnumerable<SupplierDTO> GetAllSupplierList(long orgId)
        {
            return this._accountsDb.Db.Database.SqlQuery<SupplierDTO>(
                string.Format(@" Select * From tblSuppliers Where OrganizationId={0}", orgId)).ToList();
        }
    }
}
