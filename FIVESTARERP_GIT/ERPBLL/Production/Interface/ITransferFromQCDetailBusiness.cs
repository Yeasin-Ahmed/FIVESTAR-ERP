using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferFromQCDetailBusiness
    {
        IEnumerable<TransferFromQCDetail> GetTransferFromQCDetails(long orgId);
        IEnumerable<TransferFromQCDetail> GetTransferFromQCDetailByInfo(long infoId,long orgId);
        Task<IEnumerable<TransferFromQCDetail>> GetTransferFromQCDetailByInfoAsync(long infoId, long orgId);
        IEnumerable<TransferFromQCDetailDTO> GetTransferFromQCDetailDTO(long infoId, long orgId);
    }
}
