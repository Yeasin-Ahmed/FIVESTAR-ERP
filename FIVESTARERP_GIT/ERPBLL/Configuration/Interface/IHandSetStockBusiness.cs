using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IHandSetStockBusiness
    {
        HandSetStock GetHandsetOneInfoById(long id, long orgId);
        bool SaveHandSetStock(HandSetStockDTO dto, long userId, long branchId, long orgId);
        IEnumerable<HandSetStockDTO> GetHandsetStockInformationsByQuery(long? modelId, long? colorId, string stockType, long orgId);
        bool IsDuplicateHandsetStockIMEI(string imei, long id, long orgId);
    }
}
