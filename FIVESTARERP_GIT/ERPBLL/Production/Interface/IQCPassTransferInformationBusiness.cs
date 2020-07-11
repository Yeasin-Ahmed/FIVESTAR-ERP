using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCPassTransferInformationBusiness
    {
        IEnumerable<QCPassTransferInformation> GetQCPassTransferInformation(long orgId);
        QCPassTransferInformation GetQCPassTransferInformationById(long qcPassId,long orgId);
        bool SaveQCPassTransferInformation(QCPassTransferInformationDTO qcPassInfo, long userId, long orgId);
    }
}
