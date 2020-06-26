using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQRCodeTraceBusiness
    {
        IEnumerable<QRCodeTrace> GetQRCodeTraceByOrg(long orgId);
        QRCodeTrace GetQRCodeTraceByCode(string code,long orgId);
        bool SaveQRCodeTrace(List<QRCodeTraceDTO> dtos, long userId, long orgId);
    }
}
