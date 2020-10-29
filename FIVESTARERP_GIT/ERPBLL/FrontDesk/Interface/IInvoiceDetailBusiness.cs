using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IInvoiceDetailBusiness
    {
        IEnumerable<InvoiceUsedPartsDTO> GetUsedPartsDetails(long jobOrderId,long orgId);
        IEnumerable<InvoiceDetailDTO> InvoiceDetailsReport(long infoId, long orgId, long branchId);
        IEnumerable<InvoiceDetail> GetAllDetailByInfoId(long infoId, long orgId, long branchId);
       
    }
}
