using ERPBLL.Common;
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
    public class RequsitionInfoBusiness : IRequsitionInfoBusiness
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
                group by StateStatus", orgId)).ToList();
        }
        public IEnumerable<RequsitionInfo> GetAllReqInfoByOrgId(long orgId)
        {
            return requsitionInfoRepository.GetAll(unit => unit.OrganizationId == orgId).ToList();
        }
        public RequsitionInfo GetRequisitionById(long reqId, long orgId)
        {
            return requsitionInfoRepository.GetOneByOrg(req => req.ReqInfoId == reqId && req.OrganizationId == orgId);
        }

        public IEnumerable<RequsitionInfoDTO> GetRequsitionInfosByQuery(long? floorId, long? assemblyId, long? packagingId, long? repairLineId, long? warehouseId, long? modelId, string reqCode, string reqType, string reqFor, string fromDate, string toDate, string status, string reqFlag, long? reqInfoId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RequsitionInfoDTO>(QueryForRequsitionInfos(floorId, assemblyId, packagingId, repairLineId, warehouseId, modelId, reqCode, reqType, reqFor, fromDate, toDate, status, reqFlag, reqInfoId, orgId)).ToList();
        }

        private string QueryForRequsitionInfos(long? floorId, long? assemblyId, long? packagingId, long? repairLineId, long? warehouseId, long? modelId, string reqCode, string reqType, string reqFor, string fromDate, string toDate, string status, string reqFlag, long? reqInfoId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and ri.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and ri.LineId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and ri.AssemblyLineId={0}", assemblyId);
            }
            if (packagingId != null && packagingId > 0)
            {
                param += string.Format(@" and ri.PackagingLineId={0}", packagingId);
            }
            if (repairLineId != null && repairLineId > 0)
            {
                param += string.Format(@" and ri.RepairLineId={0}", repairLineId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and ri.WarehouseId={0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and ri.DescriptionId={0}", modelId);
            }
            if (!string.IsNullOrEmpty(reqCode) && reqCode.Trim() != "")
            {
                param += string.Format(@" and ri.ReqInfoCode LIKE'%{0}%'", reqCode);
            }
            if (!string.IsNullOrEmpty(reqType) && reqType.Trim() != "")
            {
                param += string.Format(@" and ri.RequisitionType='{0}'", reqType);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and ri.StateStatus IN({0})", status);
            }
            if (!string.IsNullOrEmpty(reqFlag) && reqFlag.Trim() != "")
            {
                param += string.Format(@" and ri.Flag='{0}'", reqFlag);
            }
            if (!string.IsNullOrEmpty(reqFor) && reqFor.Trim() != "")
            {
                param += string.Format(@" and ri.RequisitionFor='{0}'", reqFor);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date)='{0}'", tDate);
            }
            if (reqInfoId != null && reqInfoId > 0)
            {
                param += string.Format(@" and ri.ReqInfoId={0}", reqInfoId);
            }
            query = string.Format(@"Select ri.ReqInfoId,ri.ReqInfoCode,ri.RequisitionType,ri.RequisitionFor,
ri.LineId,pl.LineNumber,ri.AssemblyLineId,al.AssemblyLineName,ri.PackagingLineId,pac.PackagingLineName,
ri.WarehouseId,wi.WarehouseName,ri.DescriptionId,de.DescriptionName 'ModelName',ri.StateStatus,
ri.IsBundle,ri.Flag,ri.Remarks,(Select Count(*) From tblRequsitionDetails Where ReqInfoId = ri.ReqInfoId) 'TotalReqCount',
ri.EntryDate,app.UserName 'EntryUser',ri.UpdateDate, (Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId= ri.UpUserId ) 'UpdateUser'
From [Production].dbo.tblRequsitionInfo ri
Inner Join [ControlPanel].dbo.tblApplicationUsers app on ri.EUserId = app.UserId
Inner Join [Production].dbo.tblProductionLines pl on ri.LineId = pl.LineId
Left Join [Production].dbo.tblAssemblyLines al on ri.AssemblyLineId = al.AssemblyLineId
Left Join [Production].dbo.tblPackagingLine pac on ri.PackagingLineId = pac.PackagingLineId
Inner Join [Inventory].dbo.tblDescriptions de on ri.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses wi on ri.WarehouseId = wi.Id
Where 1=1 {0} Order By ri.ReqInfoId desc", Utility.ParamChecker(param));
            return query;
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
                requsitionInfo.Flag = reqInfoDTO.Flag;
                requsitionInfo.AssemblyLineId = reqInfoDTO.AssemblyLineId;
                requsitionInfo.PackagingLineId = reqInfoDTO.PackagingLineId;

                if (requsitionInfo.IsBundle == true)
                {
                    //reqInfoDTO.ItemId != null && reqInfoDTO.ItemId > 0
                    requsitionInfo.UnitId = items.FirstOrDefault(i => i.ItemId == reqInfoDTO.ItemId.Value).UnitId;
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

            return IsSuccess;
        }
        public bool SaveRequisitionStatus(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = requsitionInfoRepository.GetOneByOrg(req => req.ReqInfoId == reqId && req.OrganizationId == orgId);
            if (reqInfo != null)
            {
                reqInfo.StateStatus = status;
                reqInfo.UpUserId = userId;
                requsitionInfoRepository.Update(reqInfo);
            }
            return requsitionInfoRepository.Save();
        }
        public bool SaveRequisitionWithItemInfoAndDetail(RequsitionInfoDTO infoDTO, long userId, long orgId)
        {
            RequsitionInfo requsitionInfo = new RequsitionInfo()
            {
                ReqInfoCode = ("REQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                RequisitionFor = infoDTO.RequisitionFor,
                RequisitionType = infoDTO.RequisitionType,
                LineId = infoDTO.LineId,
                PackagingLineId = infoDTO.PackagingLineId,
                AssemblyLineId = infoDTO.AssemblyLineId,
                DescriptionId = infoDTO.DescriptionId,
                WarehouseId = infoDTO.WarehouseId,
                ItemTypeId = infoDTO.ItemTypeId,
                ItemId = infoDTO.ItemId,
                ForQty = infoDTO.ForQty,
                UnitId = infoDTO.UnitId,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                StateStatus = RequisitionStatus.Pending,
                Remarks = "Requisition By Production Floor For " + infoDTO.RequisitionFor,
                IsBundle = true,
                Flag = Flag.Direct
            };

            List<RequsitionDetail> requsitionDetails = new List<RequsitionDetail>();
            List<RequisitionItemInfo> requisitionItemInfos = new List<RequisitionItemInfo>();

            var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);
            foreach (var item in infoDTO.RequisitionDetails)
            {
                RequsitionDetail requsitionDetail = new RequsitionDetail()
                {
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    Quantity = item.Quantity,
                    IssueQty = 0,
                    Remarks = "Requisition By Production Floor For " + infoDTO.RequisitionFor,
                    UnitId = allItemsInDb.FirstOrDefault(s => s.ItemId == item.ItemId).UnitId
                };
                requsitionDetails.Add(requsitionDetail);
            }

            requsitionInfo.RequsitionDetails = requsitionDetails;

            foreach (var item in infoDTO.RequisitionItemInfos)
            {
                RequisitionItemInfo requisitionItemInfo = new RequisitionItemInfo()
                {
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    AssemblyLineId = item.AssemblyLineId,
                    PackagingLineId = item.PackagingLineId,
                    FloorId = item.FloorId,
                    DescriptionId = item.DescriptionId,
                    Remarks = "",
                    UnitId = allItemsInDb.FirstOrDefault(s => s.ItemId == item.ItemId).UnitId
                };
                List<RequisitionItemDetail> requisitionItemDetails = new List<RequisitionItemDetail>();
                foreach (var i in item.RequisitionItemDetails)
                {
                    RequisitionItemDetail requisitionItemDetail = new RequisitionItemDetail()
                    {
                        WarehouseId = i.WarehouseId,
                        ItemTypeId = i.ItemTypeId,
                        ItemId = i.ItemId,
                        ConsumptionQty = i.ConsumptionQty,
                        TotalQuantity = i.TotalQuantity,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = "Requisition By Production Floor For " + infoDTO.RequisitionFor
                    };
                    requisitionItemDetails.Add(requisitionItemDetail);
                }
                requisitionItemInfo.RequisitionItemDetails = requisitionItemDetails;
                requisitionItemInfos.Add(requisitionItemInfo);
            }

            requsitionInfo.RequisitionItemInfos = requisitionItemInfos;
            requsitionInfoRepository.Insert(requsitionInfo);
            return requsitionInfoRepository.Save();
        }
        public IEnumerable<DashBoardAssemblyProgressDTO> GetDashBoardAssemblyProgresses(long? floorId, long? assemblyId, long orgId)
        {
            var q = string.Format(@"Select pl.LineId 'ProductionFloorId',pl.LineNumber 'ProductionFloorName',al.AssemblyLineId,al.AssemblyLineName,
(Select ISNULL(SUM(Quantity),0) From tblRequisitionItemInfo req 
Inner Join tblRequsitionInfo r on r.ReqInfoId = req.ReqInfoId And r.StateStatus='Accepted'
Where r.OrganizationId={0} and r.AssemblyLineId=al.AssemblyLineId and Cast(req.EntryDate as date)= Cast(GETDATE() as date)) 'TargetQuantity',

--(Select COUNT(*) From tblTempQRCodeTrace 
--Where AssemblyId=al.AssemblyLineId and OrganizationId={0} and StateStatus='MiniStock' and Cast(EntryDate as date)= Cast(GETDATE() as date)) 'CompleteQuantity',
ISNULL((Select SUM(Quantity) From tblQCPassTransferInformation Where AssemblyLineId=al.AssemblyLineId and OrganizationId={0} and Cast(EntryDate as date)= Cast(GETDATE() as date)),0) 'CompleteQuantity',
ISNULL((Select SUM(Quantity) From tblQCPassTransferInformation Where StateStatus='Received By Production' and  AssemblyLineId=al.AssemblyLineId and OrganizationId={0} and Cast(EntryDate as date)= Cast(GETDATE() as date)),0) 'MiniStockReceivedQty',
ISNULL((Select SUM(Quantity) From tblQCPassTransferInformation Where StateStatus='Send By QC' and  AssemblyLineId=al.AssemblyLineId and OrganizationId={0} and Cast(EntryDate as date)= Cast(GETDATE() as date)),0) 'MiniStockNotReceivedQty',
--and StateStatus IN('Send','Received')
(Select COUNT(*) From tblQRCodeTransferToRepairInfo Where OrganizationId={0}  and Cast(EntryDate as date)= Cast(GETDATE() as date) and AssemblyLineId=al.AssemblyLineId) 'RepairIn',

(Select COUNT(*) From tblQRCodeTransferToRepairInfo Where OrganizationId={0} and StateStatus ='Repair-Done' and Cast(EntryDate as date)= Cast(GETDATE() as date) and AssemblyLineId=al.AssemblyLineId) 'RepairOut'

From tblRequsitionInfo ri
Inner Join [Production].dbo.tblAssemblyLines al on ri.AssemblyLineId =al.AssemblyLineId and ri.OrganizationId=al.OrganizationId and ISNULL(ri.AssemblyLineId,0) > 0
Inner Join [Production].dbo.tblProductionLines pl on al.ProductionLineId = pl.LineId
Where al.OrganizationId={0} and Cast(ri.EntryDate as date)= Cast(GETDATE() as date) and ri.StateStatus='Accepted'
Group By pl.LineId,pl.LineNumber,al.AssemblyLineId,al.AssemblyLineName", orgId);
            var data = this._productionDb.Db.Database.SqlQuery<DashBoardAssemblyProgressDTO>(q).ToList();

            foreach (var item in data)
            {
                string assemblyFaulty = string.Format(@"Select fsd.ItemId,i.ItemName,ISNULL(COUNT(fsd.Quantity),0) 'Quantity' From tblFaultyItemStockDetail fsd
Inner Join [Inventory].dbo.tblItems i on fsd.ItemId = i.ItemId
Inner Join [Production].dbo.tblAssemblyLines al on fsd.AsseemblyLineId = al.AssemblyLineId
Inner Join [Production].dbo.tblProductionLines pl on al.ProductionLineId = pl.LineId
Where 1=1 and fsd.OrganizationId={0} and Cast(fsd.EntryDate as Date) = Cast(GetDate() as date) and fsd.StockStatus='Stock-In' and fsd.AsseemblyLineId={1}
Group By al.AssemblyLineId,fsd.ItemId,i.ItemName", orgId, item.AssemblyLineId);

                item.AssemblyFaultys = this._productionDb.Db.Database.SqlQuery<DashBoardAssemblyFaultyDTO>(assemblyFaulty).ToList();

                string AssemblyProblems = string.Format(@"Select al.AssemblyLineId,al.AssemblyLineName,f.CaseId 'ProblemId',f.ProblemDescription,COUNT(*) 'Count' From tblQRCodeProblem qp
Inner Join tblFaultyCase f on f.CaseId = qp.ProblemId
Inner Join [Production].dbo.tblAssemblyLines al on qp.AssemblyLineId = al.AssemblyLineId
Where 1= 1 and qp.OrganizationId={0} and Cast(qp.EntryDate as date)= Cast(GETDATE() as date) and qp.AssemblyLineId={1}
Group By al.AssemblyLineId,al.AssemblyLineName,f.CaseId,f.ProblemDescription", orgId, item.AssemblyLineId);

                item.AssemblyProblems = this._productionDb.Db.Database.SqlQuery<DashBoardAssemblyProblemDTO>(AssemblyProblems).ToList();

            }
            return data;
        }
    }
}
