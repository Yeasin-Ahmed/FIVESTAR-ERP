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
   public class TechnicalServicesStockBusiness: ITechnicalServicesStockBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly TechnicalServicesStockRepository technicalServicesStockRepository;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IRequsitionDetailForJobOrderBusiness _requsitionDetailForJobOrderBusiness;

        public TechnicalServicesStockBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.technicalServicesStockRepository = new TechnicalServicesStockRepository(this._frontDeskUnitOfWork);
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._requsitionDetailForJobOrderBusiness = requsitionDetailForJobOrderBusiness;
        }

        public IEnumerable<TechnicalServicesStock> GetAllTechnicalServicesStock(long id,long orgId, long branchId)
        {
            return technicalServicesStockRepository.GetAll(stock =>stock.JobOrderId==id && stock.OrganizationId == orgId && stock.BranchId == branchId).ToList();
        }

        public IEnumerable<TechnicalServicesStock> GetRequsitionStockByStockId(long id, long orgId, long branchId)
        {
            return technicalServicesStockRepository.GetAll(stock =>stock.TsStockId==id && stock.OrganizationId == orgId && stock.BranchId == branchId).ToList();
        }

        public bool SaveTechnicalServicesStockIn(List<TechnicalServicesStockDTO> servicesStockDTOs, long userId, long orgId, long branchId)
        {
            List<TechnicalServicesStock> servicesStocks = new List<TechnicalServicesStock>();
            foreach (var item in servicesStockDTOs)
            {
                var servicesInfo = GetAllTechnicalServicesStock(item.JobOrderId.Value,orgId,branchId).Where(o =>o.SWarehouseId==item.SWarehouseId && o.PartsId == item.PartsId && o.JobOrderId==item.JobOrderId).FirstOrDefault();
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
                    stockServices.PartsId = item.PartsId;
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

        public bool SaveTechnicalStockInRequistion(long id, string status, long orgId, long userId, long branchId)
        {
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(id, orgId);
            var reqDetail = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(id, orgId,branchId).ToList();
            if (reqDetail != null && reqInfo.StateStatus == RequisitionStatus.Approved && reqDetail.Count > 0)
            {
                List<TechnicalServicesStockDTO> technicalServicesStockDTOs = new List<TechnicalServicesStockDTO>();
                foreach (var item in reqDetail)
                {
                    TechnicalServicesStockDTO technicalServicesStockDTO = new TechnicalServicesStockDTO
                    {
                        JobOrderId=reqInfo.JobOrderId,
                        SWarehouseId = reqInfo.SWarehouseId,
                        PartsId = item.PartsId,
                        Quantity = item.Quantity,
                        UsedQty= 0,
                        ReturnQty = 0,
                        Remarks = item.Remarks,
                        OrganizationId = orgId,
                        EUserId = userId,
                        BranchId = branchId,
                    };
                    technicalServicesStockDTOs.Add(technicalServicesStockDTO);
                }
                if (SaveTechnicalServicesStockIn(technicalServicesStockDTOs, userId, orgId,branchId) == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(id, status, userId, orgId,branchId );
                }
            }
            return false;
        }
    }
}
