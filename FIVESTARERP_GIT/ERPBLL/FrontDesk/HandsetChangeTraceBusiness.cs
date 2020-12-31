using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.FrontDesk.DomainModels;
using ERPDAL.ConfigurationDAL;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
    public class HandsetChangeTraceBusiness : IHandsetChangeTraceBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//database
        private readonly IConfigurationUnitOfWork _configurationUnitOfWork; // database

        private readonly HandsetChangeTraceRepository _handsetChangeTraceRepository;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly JobOrderRepository _jobOrderRepository;
        private readonly HandSetStockRepository _handSetStockRepository;
        private readonly IHandSetStockBusiness _handSetStockBusiness;


        public HandsetChangeTraceBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderBusiness jobOrderBusiness, IConfigurationUnitOfWork configurationUnitOfWork, IHandSetStockBusiness handSetStockBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._handsetChangeTraceRepository = new HandsetChangeTraceRepository(this._frontDeskUnitOfWork);
            this._jobOrderBusiness = jobOrderBusiness;
            this._jobOrderRepository = new JobOrderRepository(this._frontDeskUnitOfWork);
            this._configurationUnitOfWork = configurationUnitOfWork;
            this._handSetStockRepository = new HandSetStockRepository(this._configurationUnitOfWork);
            this._handSetStockBusiness = handSetStockBusiness;
        }

        public HandsetChangeTrace GetOneJobByOrgId(long jobId, long orgId)
        {
            return _handsetChangeTraceRepository.GetOneByOrg(c => c.JobOrderId == jobId && c.OrganizationId == orgId);
        }

        public bool ExitJobOrderForIMEI(long jobId, long orgId, long branchId)
        {
            return _handsetChangeTraceRepository.GetOneByOrg(f => f.JobOrderId == jobId && f.OrganizationId == orgId && f.BranchId == branchId) != null ? false : true;
        }

        public bool StockOutHandset(string imei1, long orgId, long branchId, long userId)
        {
            List<HandSetStock> handSetStocks = new List<HandSetStock>();

            var reqQty = 1;
            var imeiInStock = _handSetStockBusiness.GetAllHansetStockByOrgIdAndBranchId(orgId, branchId).Where(i => i.IMEI == imei1).OrderBy(i => i.HandSetStockId).ToList();

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
            return _handSetStockRepository.Save();
        }

        public bool UpdateAndChangeJobOrder(long jobId, string imei1, string imei2, long modelId, string color, long orgId, long branchId, long userId)
        {
            bool isSuccess = false;
            var JobOrderInDb = _jobOrderBusiness.GetJobOrderById(jobId, orgId);

            HandsetChangeTrace handset = new HandsetChangeTrace();
            handset.JobOrderId = JobOrderInDb.JodOrderId;
            handset.JobOrderCode = JobOrderInDb.JobOrderCode;
            handset.JobStatus = JobOrderInDb.StateStatus;
            handset.ModelId = JobOrderInDb.DescriptionId;
            handset.Type = JobOrderInDb.Type;
            handset.IMEI1 = JobOrderInDb.IMEI;
            handset.IMEI2 = JobOrderInDb.IMEI2;
            handset.Color = JobOrderInDb.ModelColor;
            handset.CustomerName = JobOrderInDb.CustomerName;
            handset.CustomerPhone = JobOrderInDb.MobileNo;
            handset.EUserId = userId;
            handset.EntryDate = DateTime.Now;
            handset.OrganizationId = orgId;
            handset.BranchId = branchId;
            _handsetChangeTraceRepository.Insert(handset);
            _handsetChangeTraceRepository.Save();

            if (JobOrderInDb != null)
            {
                JobOrderInDb.DescriptionId = modelId;
                JobOrderInDb.IMEI = imei1;
                JobOrderInDb.IMEI2 = imei2;
                JobOrderInDb.ModelColor = color;
                _jobOrderRepository.Update(JobOrderInDb);
            }
            if (_jobOrderRepository.Save() == true)
            {
                return StockOutHandset(imei1, orgId, branchId, userId);
            }
            return isSuccess;
        }

        public bool IsDuplicateIMEI1(long jobId, string imei1, long orgId, long branchId)
        {
            return _handsetChangeTraceRepository.GetOneByOrg(f => f.JobOrderId != jobId && f.IMEI2 == imei1 && f.OrganizationId == orgId) != null ? true : false;
        }

        public bool IsDuplicateIMEI2(long jobId, string imei2, long orgId, long branchId)
        {
            return _handsetChangeTraceRepository.GetOneByOrg(f => f.JobOrderId != jobId && f.IMEI2 == imei2 && f.OrganizationId == orgId) != null ? true : false;
        }
    }
}
