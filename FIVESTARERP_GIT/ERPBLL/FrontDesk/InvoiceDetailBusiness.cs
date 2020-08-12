using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
   public class InvoiceDetailBusiness: IInvoiceDetailBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly InvoiceDetailRepository _invoiceDetailRepository;

        public InvoiceDetailBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._invoiceDetailRepository = new InvoiceDetailRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<InvoiceUsedPartsDTO> GetUsedPartsDetails(long jobOrderId, long orgId, long branchId)
        {
            var data= this._frontDeskUnitOfWork.Db.Database.SqlQuery<InvoiceUsedPartsDTO>(
                string.Format(@"Select *,UsedQty*Price 'Total' From(select tstock.PartsId,parts.MobilePartName,tstock.UsedQty,
(select Top 1  info.SellPrice from [Configuration].dbo.tblMobilePartStockInfo info
where info.MobilePartId=tstock.PartsId) 'Price'
from [FrontDesk].dbo.tblTechnicalServicesStock tstock
inner join [Configuration].dbo.tblMobileParts parts
on tstock.PartsId=parts.MobilePartId
where tstock.UsedQty>0 and tstock.JobOrderId={0} ) tbl ", jobOrderId, orgId, branchId)).ToList();
            return data;
        }

        public IEnumerable<InvoiceDetailDTO> InvoiceDetailsReport(long infoId,long orgId, long branchId)
        {
            var data = this._frontDeskUnitOfWork.Db.Database.SqlQuery<InvoiceDetailDTO>(
                string.Format(@"select InvoiceInfoId,PartsId,PartsName,Quantity,SellPrice,Total from InvoiceDetails
where InvoiceInfoId={0} and OrganizationId={1} and BranchId={2}", infoId, orgId, branchId)).ToList();
            return data;
        }
    }
}
