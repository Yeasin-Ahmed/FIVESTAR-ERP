using ERPBO.ControlPanel.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IInvoiceInfoBusiness
    {
        bool SaveInvoiceForJobOrder(InvoiceInfoDTO infodto, List<InvoiceDetailDTO> detailsdto, long userId, long orgId, long branchId);
        InvoiceInfo GetAllInvoice(long jobOrderId, long orgId, long branchId);
        bool UpdateJobOrderInvoice(long jobOrderId, long userId, long orgId, long branchId);
        IEnumerable<InvoiceInfo> InvoiceInfoReport(long infoId,long orgId, long branchId);
        IEnumerable<InvoiceInfoDTO> GetSellsReport(long orgId, long branchId, string fromDate, string toDate);
        InvoiceInfo GetAllInvoiceByOrgId(long invoiceId, long orgId, long branchId);
    }
}
