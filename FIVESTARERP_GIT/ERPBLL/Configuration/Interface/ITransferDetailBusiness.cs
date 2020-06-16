using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface ITransferDetailBusiness
    {
        IEnumerable<TransferDetail> GetAllTransferDetailByOrgId(long orgId, long branchId);
        IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId,long orgId, long branchId);
        IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId, long orgId);
    }
}
