using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITempQRCodeTraceBusiness
    {
        IEnumerable<TempQRCodeTrace> GetTempQRCodeTraceByOrg(long orgId);
        TempQRCodeTrace GetTempQRCodeTraceByCode(string code, long orgId);
        Task<TempQRCodeTrace> GetTempQRCodeTraceByCodeAsync(string code, long orgId);
        bool SaveTempQRCodeTrace(List<TempQRCodeTraceDTO> dtos, long userId, long orgId);
        bool IsExistQRCodeWithStatus(string qrCode, string status, long orgId);
        bool UpdateQRCodeStatus(string qrCode, string status, long orgId);
        Task<bool> UpdateQRCodeStatusAsync(string qrCode, string status, long orgId);
        Task<IEnumerable<TempQRCodeTrace>> GetTempQRCodeTracesByQRCodesAsync(List<string> qrCodes, long orgId);
        Task<bool> UpdateQRCodeBatchAsync(List<TempQRCodeTrace> qrCodes, long orgId);
    }
}
