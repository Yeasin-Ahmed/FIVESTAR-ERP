﻿using ERPBLL.Common;
using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERPBLL.Production
{
   public class RequsitionInfoBusiness: IRequsitionInfoBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for        - Repository
        /// </summary>
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly RequsitionInfoRepository requsitionInfoRepository; // table
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly IItemBusiness _itemBusiness; // interface
        //private readonly IRequsitionDetailBusiness _requsitionDetailBusiness; //

        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        public RequsitionInfoBusiness(IProductionUnitOfWork productionDb, IInventoryUnitOfWork inventoryDb, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IItemBusiness itemBusiness)
        {
            this._productionDb = productionDb;
            requsitionInfoRepository = new RequsitionInfoRepository(this._productionDb);
            this._inventoryDb = inventoryDb;
            this._warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            this._itemBusiness = itemBusiness;
            //this._requsitionDetailBusiness = requsitionDetailBusiness;
            //, IRequsitionDetailBusiness requsitionDetailBusiness
        }

        public IEnumerable<DashboardRequisitionSummeryDTO> DashboardRequisitionSummery(long orgId)
        {
           return this._productionDb.Db.Database.SqlQuery<DashboardRequisitionSummeryDTO>(
               string.Format(@"select StateStatus, count(*) as TotalCount
                from dbo.tblRequsitionInfo
                Where OrganizationId={0}
                group by StateStatus",orgId)).ToList();
        }

        public IEnumerable<RequsitionInfo> GetAllReqInfoByOrgId(long orgId)
        {
            return requsitionInfoRepository.GetAll(unit => unit.OrganizationId == orgId).ToList();
        }
        public RequsitionInfo GetRequisitionById(long reqId, long orgId)
        {
            return requsitionInfoRepository.GetOneByOrg(req=>req.ReqInfoId ==reqId && req.OrganizationId == orgId);
        }
        public bool SaveRequisition(ReqInfoDTO reqInfoDTO, long userId, long orgId)
        {
            bool IsSuccess = false;
            RequsitionInfo requsitionInfo = new RequsitionInfo();
            var items = _itemBusiness.GetAllItemByOrgId(orgId);
            if (reqInfoDTO.ReqInfoId == 0)
            {
                requsitionInfo.WarehouseId = reqInfoDTO.WarehouseId.Value;
                requsitionInfo.LineId = reqInfoDTO.LineId.Value;
                requsitionInfo.OrganizationId = orgId;
                requsitionInfo.StateStatus = RequisitionStatus.Pending;
                requsitionInfo.ReqInfoCode = ("REQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                requsitionInfo.DescriptionId = reqInfoDTO.DescriptionId;
                requsitionInfo.EntryDate = DateTime.Now;
                requsitionInfo.EUserId = userId;
                requsitionInfo.RequisitionType = reqInfoDTO.RequisitionType;
                requsitionInfo.RequisitionFor = reqInfoDTO.RequisitionFor;
                requsitionInfo.IsBundle = reqInfoDTO.IsBundle;
                requsitionInfo.ItemTypeId = reqInfoDTO.ItemTypeId;
                requsitionInfo.ItemId = reqInfoDTO.ItemId;
                requsitionInfo.ForQty = reqInfoDTO.ForQty;
                if(reqInfoDTO.ItemId != null && reqInfoDTO.ItemId > 0)
                {
                    requsitionInfo.UnitId = items.FirstOrDefault(i=> i.ItemId == reqInfoDTO.ItemId.Value).UnitId;
                }

                List<RequsitionDetail> requsitionDetails = new List<RequsitionDetail>();

                foreach (var item in reqInfoDTO.ReqDetails)
                {
                    RequsitionDetail requsitionDetail = new RequsitionDetail();
                    requsitionDetail.ItemTypeId = item.ItemTypeId;
                    requsitionDetail.ItemId = item.ItemId;
                    requsitionDetail.Quantity = item.Quantity;
                    requsitionDetail.UnitId = items.FirstOrDefault(i => i.ItemId == item.ItemId.Value).UnitId;
                    requsitionDetail.Remarks = item.Remarks;
                    requsitionDetail.EUserId = userId;
                    requsitionDetail.EntryDate = DateTime.Now;
                    requsitionDetail.OrganizationId = orgId;
                    requsitionDetails.Add(requsitionDetail);
                }
                requsitionInfo.RequsitionDetails = requsitionDetails;
                requsitionInfoRepository.Insert(requsitionInfo);
                IsSuccess = requsitionInfoRepository.Save();
            }
            else
            {
            }
            return IsSuccess;
        }
        public bool SaveRequisitionStatus(long reqId, string status, long orgId, long userId)
        {
           var reqInfo = requsitionInfoRepository.GetOneByOrg(req => req.ReqInfoId == reqId && req.OrganizationId == orgId);
            if(reqInfo != null)
            {
                reqInfo.StateStatus = status;
                reqInfo.UpUserId = userId;
                requsitionInfoRepository.Update(reqInfo);
            }
            return requsitionInfoRepository.Save();
        }
    }
}
