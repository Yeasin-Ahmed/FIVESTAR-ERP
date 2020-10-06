using ERPBLL.Common;
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

        public IEnumerable<InvoiceInfoDTO> GetSellsReport(long orgId, long branchId, string fromDate, string toDate)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<InvoiceInfoDTO>(QueryForSells( orgId, branchId, fromDate, toDate)).ToList();
        }
        private string QueryForSells(long orgId, long branchId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            
            if (orgId > 0)
            {
                param += string.Format(@"and OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and BranchId={0}", branchId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"select InvoiceInfoId,InvoiceCode,JobOrderCode,CustomerName,TotalSPAmount,InvoiceType,
LabourCharge,VAT,Tax,Discount,NetAmount,EntryDate,
OrganizationId,BranchId,(select top 1 sum(NetAmount)'Total' from tblInvoiceInfo)'Total' 
from tblInvoiceInfo
where 1=1{0} order by EntryDate desc", Utility.ParamChecker(param));
            return query;
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
                invoiceInfo.InvoiceType = "JobOrder";
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

        public InvoiceInfo GetAllInvoiceByOrgId(long invoiceId, long orgId, long branchId)
        {
            return _invoiceInfoRepository.GetOneByOrg(inv => inv.InvoiceInfoId == invoiceId && inv.OrganizationId == orgId && inv.BranchId == branchId);
        }

        public bool SaveInvoiceForAccessoriesSells(InvoiceInfoDTO infodto, List<InvoiceDetailDTO> detailsdto, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            double netamount = 0;
            double spamount = 0;
            spamount = detailsdto.Select(t => t.Total).Sum();
            netamount = ((spamount + infodto.VAT + infodto.Tax) - infodto.Discount);
           
            //var jobOrder = _jobOrderBusiness.GetJobOrdersByIdWithBranch(infodto.JobOrderId, branchId, orgId);
            InvoiceInfo invoiceInfo = new InvoiceInfo();
            if (infodto.InvoiceInfoId == 0)
            {
                invoiceInfo.InvoiceCode = ("INV-" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                invoiceInfo.JobOrderId = 0;
                invoiceInfo.JobOrderCode = null;
                invoiceInfo.InvoiceType = "Sells";
                invoiceInfo.CustomerName = infodto.CustomerName;
                invoiceInfo.CustomerPhone = infodto.CustomerPhone;
                invoiceInfo.Email = infodto.Email;
                invoiceInfo.Address = infodto.Address;
                invoiceInfo.WarrentyFor = infodto.WarrentyFor;
                invoiceInfo.LabourCharge = 0;
                invoiceInfo.VAT = infodto.VAT;
                invoiceInfo.Tax = infodto.Tax;
                invoiceInfo.Discount = infodto.Discount;//01925424687
                invoiceInfo.TotalSPAmount = spamount;
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
            }
            return IsSuccess;
        }

        public IEnumerable<InvoiceInfoDTO> GetSellsAccessories(long orgId, long branchId, string fromDate, string toDate, string invoice)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<InvoiceInfoDTO>(QueryForSellsAccessories(orgId, branchId, fromDate, toDate,invoice)).ToList();
        }
        private string QueryForSellsAccessories(long orgId, long branchId, string fromDate, string toDate, string invoice)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@"and inv.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and inv.BranchId={0}", branchId);
            }
            if (!string.IsNullOrEmpty(invoice))
            {
                param += string.Format(@"and inv.InvoiceCode Like '%{0}%'", invoice);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(inv.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(inv.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(inv.EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"select * from tblInvoiceInfo inv
left join [ControlPanel].dbo.tblApplicationUsers au on inv.EUserId=au.UserId
where 1=1{0} and InvoiceType='Sells'", Utility.ParamChecker(param));
            return query;
        }
    }
}
