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
        bool SaveAssemblyLineStockOut(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId,string flag);
        Task<bool> SaveAssemblyLineStockInAsync(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId);
        Task<bool> SaveAssemblyLineStockOutAsync(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId, string flag);
        bool SaveAssemblyLineStockReturn(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId, string flag);
    }
}
