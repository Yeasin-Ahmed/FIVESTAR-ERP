using ERPBLL.FrontDesk.Interface;
using ERPBO.ControlPanel.DomainModels;
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
   public class InvoiceInfoBusiness: IInvoiceInfoBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly InvoiceInfoRepository _invoiceInfoRepository;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly JobOrderRepository _jobOrderRepository;

        public InvoiceInfoBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderBusiness jobOrderBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._invoiceInfoRepository = new InvoiceInfoRepository(this._frontDeskUnitOfWork);
            this._jobOrderBusiness = jobOrderBusiness;
            this._jobOrderRepository = new JobOrderRepository(this._frontDeskUnitOfWork);
        }

        public InvoiceInfo GetAllInvoice(long jobOrderId, long orgId, long branchId)
        {
            return _invoiceInfoRepository.GetOneByOrg(inv => inv.JobOrderId == jobOrderId && inv.OrganizationId == orgId && inv.BranchId == branchId);
        }

        public IEnumerable<InvoiceInfo> InvoiceInfoReport(long infoId,long orgId, long branchId)
        {
            List<InvoiceInfo> invoiceInfo = new List<InvoiceInfo>();
            var Info = _invoiceInfoRepository.GetAll(d =>d.InvoiceInfoId== infoId && d.OrganizationId == orgId && d.BranchId == branchId).ToList();
            foreach (var item in Info)
            {
                InvoiceInfo info = new InvoiceInfo();
                info.JobOrderCode = item.JobOrderCode;
                info.CustomerName = item.CustomerName;
                info.CustomerPhone = item.CustomerPhone;
                info.InvoiceCode = item.InvoiceCode;
                info.VAT = item.VAT;
                info.Tax = item.Tax;
                info.Discount = item.Discount;
                info.LabourCharge = item.LabourCharge;
                info.NetAmount = item.NetAmount;
                info.EntryDate = item.EntryDate;
                invoiceInfo.Add(info);
            }
            return invoiceInfo;
        }

        public bool SaveInvoiceForJobOrder(InvoiceInfoDTO infodto, List<InvoiceDetailDTO> detailsdto, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            double netamount = 0;
            netamount = ((infodto.TotalSPAmount + infodto.LabourCharge + infodto.VAT + infodto.Tax) - infodto.Discount);
            var jobOrder = _jobOrderBusiness.GetJobOrdersByIdWithBranch(infodto.JobOrderId, branchId,orgId);
            InvoiceInfo invoiceInfo = new InvoiceInfo();
            if (infodto.InvoiceInfoId == 0)
            {
                invoiceInfo.InvoiceCode = ("INV-" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                invoiceInfo.JobOrderId = infodto.JobOrderId;
                invoiceInfo.JobOrderCode = jobOrder.JobOrderCode;
                invoiceInfo.CustomerName = jobOrder.CustomerName;
                invoiceInfo.CustomerPhone = jobOrder.MobileNo;
                invoiceInfo.LabourCharge = infodto.LabourCharge;
                invoiceInfo.VAT = infodto.VAT;
                invoiceInfo.Tax = infodto.Tax;
                invoiceInfo.Discount = infodto.Discount;
                invoiceInfo.TotalSPAmount = infodto.TotalSPAmount;
                invoiceInfo.NetAmount = netamount;
                invoiceInfo.Remarks = infodto.Remarks;
                invoiceInfo.EntryDate = DateTime.Now;
                invoiceInfo.EUserId = userId;
                invoiceInfo.OrganizationId = orgId;
                invoiceInfo.BranchId = branchId;
                List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();

                foreach (var item in detailsdto)
                {
                    InvoiceDetail Detail = new InvoiceDetail();
                    Detail.PartsId = item.PartsId;
                    Detail.PartsName = item.PartsName;
                    Detail.Quantity = item.Quantity;
                    Detail.SellPrice = item.SellPrice;
                    Detail.Total = item.Total;
                    Detail.EUserId = userId;
                    Detail.EntryDate = DateTime.Now;
                    Detail.OrganizationId = orgId;
                    Detail.BranchId = branchId;
                    invoiceDetails.Add(Detail);
                }
                invoiceInfo.InvoiceDetails = invoiceDetails;
                _invoiceInfoRepository.Insert(invoiceInfo);
                IsSuccess = _invoiceInfoRepository.Save();
                if (IsSuccess == true)
                {
                    UpdateJobOrderInvoice(infodto.JobOrderId, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }
        public bool UpdateJobOrderInvoice(long jobOrderId,long userId, long orgId, long branchId)
        {
            var jobOrder = _jobOrderBusiness.GetJobOrderById(jobOrderId, orgId);
            var invoiceinfo = GetAllInvoice(jobOrderId, orgId, branchId);

            if (jobOrder != null)
            {
                jobOrder.InvoiceInfoId = invoiceinfo.InvoiceInfoId;
                jobOrder.InvoiceCode = invoiceinfo.InvoiceCode;
                jobOrder.UpUserId = userId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }
    }
}
