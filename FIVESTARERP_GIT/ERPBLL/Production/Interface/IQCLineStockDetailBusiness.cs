using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCLineStockDetailBusiness
    {
        IEnumerable<QualityControlLineStockDetail> GetQCLineStockDetails(long orgId);
        bool SaveQCLineStockIn(List<QualityControlLineStockDetailDTO> qualityControlStockDetailDto, long userId, long orgId);
        bool SaveQCStockInByAssemblyLine(long transferId, string status, long orgId, long userId);
        bool SaveQCLineStockOut(List<QualityControlLineStockDetailDTO> qualityControlStockDetailDto, long userId, long orgId,string flag);
    }
}
