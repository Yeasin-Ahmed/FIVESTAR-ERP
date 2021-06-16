using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface ILotInLogBusiness
    {
        IEnumerable<LotInLog> GetAllLotInByToDay(long userId, long orgId, DateTime date);
        IEnumerable<LotInLogDTO> GetAllDataByDateWise(string fromDate, string toDate, string qrCode);
    }
}
