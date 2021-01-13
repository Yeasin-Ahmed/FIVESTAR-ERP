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
        IEnumerable<HandSetStock> GetAllHansetStockByOrgId(long orgId);
        IEnumerable<HandSetStockDTO> GetAllHansetModelAndColor(long orgId);
        IEnumerable<HandSetStock> GetAllHansetStockByOrgIdAndBranchId(long orgId, long branchId);
        bool IsExistsHandsetStockIMEI(string imei, long orgId, string status);
        HandSetStock GetIMEI2ByIMEI1(string imei, long branchId, long orgId);
    }
}
