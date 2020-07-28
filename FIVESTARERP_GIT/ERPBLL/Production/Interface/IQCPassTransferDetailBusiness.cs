using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCPassTransferDetailBusiness
    {
        IEnumerable<QCPassTransferDetail> GetQCPassTransferDetails(long orgId);
        IEnumerable<QCPassTransferDetailDTO> GetQCPassTransferDetailsByQuery(long? floorId, long? assemblyId, long? qcLineId, string qrCode, string transferCode, string status, string date,long? qcPassId, long? userId, long orgId);
    }
}
