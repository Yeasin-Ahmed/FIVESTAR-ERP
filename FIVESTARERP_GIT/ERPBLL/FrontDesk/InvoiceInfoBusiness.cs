using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.ControlPanel.DomainModels;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.ConfigurationDAL;
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
        private readonly InvoiceInfoRepository _invoiceInfoRepository;//repo
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly JobOrderRepository _jobOrderRepository;
        //
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly MobilePartStockDetailRepository _mobilePartStockDetailRepository; // repo
        private readonly MobilePartStockInfoRepository _mobilePartStockInfoRepository; //repo
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly InvoiceDetailBusiness _invoiceDetailBusiness;
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;
        private readonly IHandSetStockBusiness _handSetStockBusiness;
        private readonly FaultyStockInfoRepository _faultyStockInfoRepository;
        private readonly FaultyStockDetailRepository _faultyStockDetailRepository;
        private readonly HandSetStockRepository _handSetStockRepository;

        public InvoiceInfoBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IConfigurationUnitOfWork ConfigurationUnitOfWork, IJobOrderBusiness jobOrderBusiness,  IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, InvoiceDetailBusiness invoiceDetailBusiness, IHandSetStockBusiness handSetStockBusiness, IFaultyStockInfoBusiness faultyStockInfoBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._configurationDb = ConfigurationUnitOfWork;
            this._invoiceInfoRepository = new InvoiceInfoRepository(this._frontDeskUnitOfWork);
            this._jobOrderRepository = new JobOrderRepository(this._frontDeskUnitOfWork);
            this._mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configurationDb);
            this._mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
            this._jobOrderBusiness = jobOrderBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._invoiceDetailBusiness = invoiceDetailBusiness;
            this._handSetStockBusiness = handSetStockBusiness;
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            _faultyStockInfoRepository = new FaultyStockInfoRepository(this._configurationDb);
            _handSetStockRepository = new HandSetStockRepository(this._configurationDb);
            _faultyStockDetailRepository = new FaultyStockDetailRepository(this._configurationDb);
        }

        public InvoiceInfo GetAllInvoice(long jobOrderId, long orgId, long branchId)
        {
            return _invoiceInfoRepository.GetOneByOrg(inv => inv.JobOrderId == jobOrderId && inv.OrganizationId == orgId && inv.BranchId == branchId);
        }

        public IEnumerable<InvoiceInfoDTO> GetSellsReport(long orgId, long branchId, string fromDate, string toDate,string status,string invoice)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<InvoiceInfoDTO>(QueryForSells( orgId, branchId, fromDate, toDate,status,invoice)).ToList();
        }
        private string QueryForSells(long orgId, long branchId, string fromDate, string toDate,string status,string invoice)
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
            if (!string.IsNullOrEmpty(status))
            {
                param += string.Format(@"and InvoiceType ='{0}'", status);
            }
            if (!string.IsNullOrEmpty(invoice))
            {
                param += string.Format(@"and InvoiceCode Like '%{0}%'", invoice);
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
                invoiceInfo.InvoiceType = InvoiceTypeStatus.JobOrder;
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
           var modelId = detailsdto.FirstOrDefault().ModelId;
            //var modelName= detailsdto.FirstOrDefault().ModelName;

            //var jobOrder = _jobOrderBusiness.GetJobOrdersByIdWithBranch(infodto.JobOrderId, branchId, orgId);
            InvoiceInfo invoiceInfo = new InvoiceInfo();
            if (infodto.InvoiceInfoId == 0)
            {
                invoiceInfo.InvoiceCode = ("INV-" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                invoiceInfo.JobOrderId = 0;
                invoiceInfo.JobOrderCode = null;
                invoiceInfo.InvoiceType = InvoiceTypeStatus.Sells;
                invoiceInfo.ModelId = modelId;
                invoiceInfo.ModelName = infodto.ModelName;
                invoiceInfo.CustomerName = infodto.CustomerName;
                invoiceInfo.CustomerPhone = infodto.CustomerPhone;
                invoiceInfo.Email = infodto.Email;
                invoiceInfo.Address = infodto.Address;
                invoiceInfo.WarrentyFor = infodto.WarrentyFor;
                invoiceInfo.LabourCharge = 0;
                invoiceInfo.VAT = infodto.VAT;
                invoiceInfo.Tax = infodto.Tax;
                invoiceInfo.Discount = infodto.Discount;
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
                    Detail.SalesType = item.SalesType;
                    Detail.IMEI = item.IMEI;
                    Detail.ModelId = item.ModelId;
                    Detail.ModelName = item.ModelName;
                    invoiceDetails.Add(Detail);
                }
                invoiceInfo.InvoiceDetails = invoiceDetails;
                _invoiceInfoRepository.Insert(invoiceInfo);
                if (_invoiceInfoRepository.Save())
                {
                    IsSuccess = StockOutAccessoriesSells(invoiceInfo.InvoiceInfoId, orgId, branchId,userId);
                }
            }
            return IsSuccess;
        }
        public bool StockOutAccessoriesSells(long invoiceId, long orgId, long branchId, long userId)
        {
            bool isSuccess = false;
            var invInfo = GetAllInvoiceByOrgId(invoiceId, orgId, branchId);
            var invDetail = _invoiceDetailBusiness.GetAllDetailByInfoId(invoiceId, orgId, branchId);
            List<FaultyStockDetails> faultyStockDetails = new List<FaultyStockDetails>();
            List<HandSetStock> handSetStocks = new List<HandSetStock>();
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
            foreach (var item in invDetail)
            {
                if (item.SalesType == "Good")
                {
                    var reqQty = item.Quantity;
                    var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(i => i.MobilePartId == item.PartsId && i.DescriptionId== item.ModelId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                    if (partsInStock.Count() > 0)
                    {
                        int remainQty = reqQty;
                        foreach (var stock in partsInStock)
                        {

                            var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                            var stockOutQty = 0;
                            if (totalStockqty <= remainQty)
                            {
                                stock.StockOutQty += totalStockqty;
                                stockOutQty = totalStockqty.Value;
                                remainQty -= totalStockqty.Value;
                            }
                            else
                            {
                                stockOutQty = remainQty;
                                stock.StockOutQty += remainQty;
                                remainQty = 0;
                            }

                            MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                            {
                                SWarehouseId = stock.SWarehouseId,
                                MobilePartId = item.PartsId,
                                CostPrice = stock.CostPrice,
                                SellPrice = item.SellPrice,
                                Quantity = stockOutQty,
                                Remarks = item.Remarks,
                                OrganizationId = orgId,
                                BranchId = branchId,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                StockStatus = StockStatus.StockOut,
                                ReferrenceNumber = invInfo.InvoiceCode,
                                DescriptionId = stock.DescriptionId,
                                
                            };
                            stockDetails.Add(stockDetail);
                            _mobilePartStockInfoRepository.Update(stock);
                            if (remainQty == 0)
                            {
                                break;
                            }
                        }
                    }
                }
                if (item.SalesType == "Faulty")
                {
                    var reqQty = item.Quantity;
                    var partsInStock = _faultyStockInfoBusiness.GetAllFaultyStockInfoByOrgId(orgId, branchId).Where(i => i.PartsId == item.PartsId && i.DescriptionId == item.ModelId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.FaultyStockInfoId).ToList();

                    if (partsInStock.Count() > 0)
                    {
                        int remainQty = reqQty;
                        foreach (var stock in partsInStock)
                        {

                            var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                            var stockOutQty = 0;
                            if (totalStockqty <= remainQty)
                            {
                                stock.StockOutQty += totalStockqty;
                                stockOutQty = totalStockqty;
                                remainQty -= totalStockqty;
                            }
                            else
                            {
                                stockOutQty = remainQty;
                                stock.StockOutQty += remainQty;
                                remainQty = 0;
                            }

                            FaultyStockDetails stockDetail = new FaultyStockDetails()
                            {
                                SWarehouseId = stock.SWarehouseId,
                                PartsId = item.PartsId,
                                CostPrice = stock.CostPrice,
                                SellPrice = item.SellPrice,
                                Quantity = stockOutQty,
                                Remarks = item.Remarks,
                                OrganizationId = orgId,
                                BranchId = branchId,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                StateStatus = StockStatus.StockOut,
                                DescriptionId = stock.DescriptionId,
                                JobOrderId = stock.JobOrderId,
                                FaultyStockInfoId = stock.FaultyStockInfoId, 
                            };
                            faultyStockDetails.Add(stockDetail);
                            _faultyStockInfoRepository.Update(stock);
                            if (remainQty == 0)
                            {
                                break;
                            }
                        }
                    }
                }
                if (item.SalesType == "Handset")
                {
                    var reqQty = item.Quantity;
                    var imeiInStock = _handSetStockBusiness.GetAllHansetStockByOrgIdAndBranchId(orgId, branchId).Where(i => i.IMEI1 == item.IMEI).OrderBy(i => i.HandSetStockId).ToList();

                    if (imeiInStock.Count() == 1)
                    {
                        int remainQty = reqQty;
                        foreach (var stock in imeiInStock)
                        {
                            stock.StateStatus = StockStatus.StockOut;
                            stock.UpdateDate = DateTime.Now;
                            stock.UpUserId = userId;

                            _handSetStockRepository.Update(stock);
                            handSetStocks.Add(stock);
                        }
                    }
                }
            }
            if (stockDetails.Count > 0)
            {
                _mobilePartStockDetailRepository.InsertAll(stockDetails);
                isSuccess = _mobilePartStockDetailRepository.Save();
                //return true;
            }
            if (faultyStockDetails.Count > 0)
            {
                _faultyStockDetailRepository.InsertAll(faultyStockDetails);
                isSuccess = _faultyStockDetailRepository.Save();
            }
            if (handSetStocks.Count > 0)
            {
                isSuccess = _handSetStockRepository.Save();
            }
            return isSuccess;
        }

        public IEnumerable<InvoiceInfoDTO> GetSellsAccessories(long orgId, long branchId, string fromDate, string toDate, string invoice,string mobileNo)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<InvoiceInfoDTO>(QueryForSellsAccessories(orgId, branchId, fromDate, toDate,invoice, mobileNo)).ToList();
        }
        private string QueryForSellsAccessories(long orgId, long branchId, string fromDate, string toDate, string invoice,string mobileNo)
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
            if (!string.IsNullOrEmpty(mobileNo))
            {
                param += string.Format(@"and inv.CustomerPhone Like '%{0}%'", mobileNo);
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
where 1=1{0} and InvoiceType='Sells' order by inv.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }
    }
}
