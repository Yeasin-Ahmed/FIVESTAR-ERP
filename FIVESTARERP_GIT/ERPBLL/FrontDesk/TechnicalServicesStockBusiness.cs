using ERPBLL.Common;
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
    public class TechnicalServicesStockBusiness : ITechnicalServicesStockBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly TechnicalServicesStockRepository technicalServicesStockRepository;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IRequsitionDetailForJobOrderBusiness _requsitionDetailForJobOrderBusiness;
        private readonly ITsStockReturnInfoBusiness _tsStockReturnInfoBusiness;

        public TechnicalServicesStockBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.technicalServicesStockRepository = new TechnicalServicesStockRepository(this._frontDeskUnitOfWork);
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._requsitionDetailForJobOrderBusiness = requsitionDetailForJobOrderBusiness;
            this._tsStockReturnInfoBusiness = tsStockReturnInfoBusiness;
        }

        public IEnumerable<TechnicalServicesStock> GetAllTechnicalServicesStock(long id, long orgId, long branchId)
        {
            return technicalServicesStockRepository.GetAll(stock => stock.JobOrderId == id && stock.OrganizationId == orgId && stock.BranchId == branchId).ToList();
        }

        public IEnumerable<TechnicalServicesStock> GetRequsitionStockByStockId(long id, long orgId, long branchId)
        {
            return technicalServicesStockRepository.GetAll(stock => stock.TsStockId == id && stock.OrganizationId == orgId && stock.BranchId == branchId).ToList();
        }

        public IEnumerable<TSStockByRequsitionDTO> GetStockByJobOrder(long jobOrderId, long tsId, long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<TSStockByRequsitionDTO>(
                string.Format(@"Select jo.JodOrderId,rq.RequsitionInfoForJobOrderId,jo.JobOrderCode,rq.RequsitionCode,parts.MobilePartName,rq.EUserId,rq.EntryDate,jo.DescriptionId,
stock.PartsId,stock.Quantity,stock.StateStatus 
from [FrontDesk].dbo.tblTechnicalServicesStock stock 
inner join [FrontDesk].dbo.tblJobOrders jo on stock.JobOrderId=jo.JodOrderId
inner join [FrontDesk].dbo.tblRequsitionInfoForJobOrders rq on stock.RequsitionInfoForJobOrderId=rq.RequsitionInfoForJobOrderId
inner join [Configuration].dbo.tblMobileParts parts on stock.PartsId=parts.MobilePartId
where jo.JodOrderId={0} and rq.StateStatus='Approved' and stock.EUserId={1} and jo.OrganizationId={2}
and jo.BranchId={3} and stock.StateStatus='Stock-Open'", jobOrderId, tsId, orgId, branchId)).ToList();
        }

        public bool SaveTechnicalServicesStockIn(List<TechnicalServicesStockDTO> servicesStockDTOs, long userId, long orgId, long branchId)
        {
            List<TechnicalServicesStock> servicesStocks = new List<TechnicalServicesStock>();
            foreach (var item in servicesStockDTOs)
            {
                var servicesInfo = GetAllTechnicalServicesStock(item.JobOrderId.Value, orgId, branchId).Where(o => o.SWarehouseId == item.SWarehouseId && o.PartsId == item.PartsId && o.JobOrderId == item.JobOrderId && o.RequsitionInfoForJobOrderId == item.RequsitionInfoForJobOrderId).FirstOrDefault();
                if (servicesInfo != null)
                {
                    servicesInfo.Quantity += item.Quantity;
                    technicalServicesStockRepository.Update(servicesInfo);
                }
                else
                {
                    TechnicalServicesStock stockServices = new TechnicalServicesStock();
                    stockServices.JobOrderId = item.JobOrderId;
                    stockServices.SWarehouseId = item.SWarehouseId;
                    stockServices.RequsitionInfoForJobOrderId = item.RequsitionInfoForJobOrderId;
                    stockServices.PartsId = item.PartsId;
                    stockServices.StateStatus = "Stock-Open";
                    stockServices.CostPrice = item.CostPrice;
                    stockServices.SellPrice = item.SellPrice;
                    stockServices.Quantity = item.Quantity;
                    stockServices.UsedQty = 0;
                    stockServices.ReturnQty = 0;
                    stockServices.OrganizationId = orgId;
                    stockServices.BranchId = branchId;
                    stockServices.EUserId = userId;
                    stockServices.Remarks = item.Remarks;
                    stockServices.EntryDate = DateTime.Now;
                    technicalServicesStockRepository.Insert(stockServices);
                }
            }
            technicalServicesStockRepository.InsertAll(servicesStocks);
            return technicalServicesStockRepository.Save();
        }

        public bool SaveTechnicalServicesStockOut(List<TechnicalServicesStockDTO> servicesStockDTOs, long userId, long orgId, long branchId)
        {
            //bool IsSuccess = false;
            List<TechnicalServicesStock> servicesStocks = new List<TechnicalServicesStock>();
            List<TsStockReturnDetailDTO> returnStocks = new List<TsStockReturnDetailDTO>();
            foreach (var item in servicesStockDTOs)
            {
                var servicesInfo = GetAllTechnicalServicesStock(item.JobOrderId.Value, orgId, branchId).Where(o => o.PartsId == item.PartsId && o.JobOrderId == item.JobOrderId && o.RequsitionInfoForJobOrderId == item.RequsitionInfoForJobOrderId).FirstOrDefault();
                servicesInfo.UsedQty = item.UsedQty;
                servicesInfo.ReturnQty = item.Quantity - item.UsedQty;
                servicesInfo.StateStatus = "Stock-Closed";

                technicalServicesStockRepository.Update(servicesInfo);
                if(servicesInfo.ReturnQty > 0)
                {
                    TsStockReturnDetailDTO returnStock = new TsStockReturnDetailDTO()
                    {
                        ReqInfoId = item.RequsitionInfoForJobOrderId,
                        RequsitionCode = item.RequsitionCode,
                        PartsId = item.PartsId.Value,
                        PartsName = item.PartsName,
                        Quantity = servicesInfo.ReturnQty,
                        JobOrderId = item.JobOrderId.Value,
                        BranchId = branchId,
                        OrganizationId = orgId,
                        EUserId = userId,
                    };
                    returnStocks.Add(returnStock); // 
                }
                
            }

            var distinctReturnInfo = returnStocks.Select(s => new { RequsitionCode = s.RequsitionCode, JobOrderId = s.JobOrderId,ReqInfoId=s.ReqInfoId }).Distinct().ToList();

            List<TsStockReturnInfoDTO> returnInfoList = new List<TsStockReturnInfoDTO>();
            foreach (var info in distinctReturnInfo)
            {
                TsStockReturnInfoDTO tsReturnInfo = new TsStockReturnInfoDTO()
                {
                    RequsitionCode = info.RequsitionCode,
                    JobOrderId = info.JobOrderId,
                    ReqInfoId=info.ReqInfoId,
                    
                };
                var detailsList =returnStocks.Where(s => s.RequsitionCode == info.RequsitionCode && s.JobOrderId == info.JobOrderId && s.ReqInfoId==info.ReqInfoId).ToList();
                tsReturnInfo.TsStockReturnDetails = detailsList;
                returnInfoList.Add(tsReturnInfo);
            }

            technicalServicesStockRepository.InsertAll(servicesStocks);
            if (technicalServicesStockRepository.Save()==true)
            {
                return _tsStockReturnInfoBusiness.SaveTsReturnStock(returnInfoList, userId, orgId, branchId);
            }
            return false ;
        }
        public bool SaveTechnicalStockInRequistion(long id, string status, long orgId, long userId, long branchId)
        {
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(id, orgId);
            var reqDetail = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(id, orgId, branchId).ToList();
            if (reqDetail != null && reqInfo.StateStatus == RequisitionStatus.Approved && reqDetail.Count > 0)
            {
                List<TechnicalServicesStockDTO> technicalServicesStockDTOs = new List<TechnicalServicesStockDTO>();
                foreach (var item in reqDetail)
                {
                    TechnicalServicesStockDTO technicalServicesStockDTO = new TechnicalServicesStockDTO()
                    {
                        JobOrderId = reqInfo.JobOrderId,
                        SWarehouseId = reqInfo.SWarehouseId,
                        PartsId = item.PartsId,
                        CostPrice = item.CostPrice,
                        SellPrice = item.SellPrice,
                        Quantity = item.Quantity,
                        UsedQty = 0,
                        ReturnQty = 0,
                        Remarks = item.Remarks,
                        OrganizationId = orgId,
                        EUserId = userId,
                        BranchId = branchId,
                    };
                    technicalServicesStockDTOs.Add(technicalServicesStockDTO);
                }
                if (SaveTechnicalServicesStockIn(technicalServicesStockDTOs, userId, orgId, branchId) == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(id, status, userId, orgId, branchId);
                }
            }
            return false;
        }
    }
}
