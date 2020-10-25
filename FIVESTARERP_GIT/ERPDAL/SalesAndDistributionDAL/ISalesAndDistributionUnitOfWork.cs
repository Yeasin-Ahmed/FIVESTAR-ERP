using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.SalesAndDistributionDAL
{
    public interface ISalesAndDistributionUnitOfWork : IDisposable
    {
        DbContext Db { get; }
    }
}
