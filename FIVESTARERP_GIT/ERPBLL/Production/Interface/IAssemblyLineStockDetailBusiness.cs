using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IAssemblyLineStockDetailBusiness
    {
        IEnumerable<AssemblyLineStockDetail> GetAssemblyLineStockDetails(long orgId);
        bool SaveAssemblyLineStockIn(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId);
        bool SaveAssemblyStockInByProductionLine(long transferId, string status, long orgId, long userId);

        bool SaveAssemblyLineStockOut(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId,string flag);
    }
}
