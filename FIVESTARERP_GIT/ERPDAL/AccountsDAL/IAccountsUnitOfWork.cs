using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.AccountsDAL
{
   public interface IAccountsUnitOfWork:IDisposable
    {
        DbContext Db { get; }
    }
}
