using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class IQCItemReqDetailListBusiness : IIQCItemReqDetailList
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;//db
        private readonly IIQCItemReqInfoList _iQCItemReqInfoList;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        //private readonly IIQCItemReqDetailList _iQCItemReqDetailList;
        private readonly IQCItemReqDetailListRepository iQCItemReqDetailListRepository;
        private readonly IQCItemReqInfoListRepository iQCItemReqInfoListRepository;
        public IQCItemReqDetailListBusiness(IInventoryUnitOfWork inventoryUnitOfWork, IIQCItemReqInfoList iQCItemReqInfoList, IWarehouseStockDetailBusiness warehouseStockDetailBusiness )
        {
            this._inventoryUnitOfWork = inventoryUnitOfWork;
            this._iQCItemReqInfoList = iQCItemReqInfoList;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            //this._iQCItemReqDetailList = iQCItemReqDetailList;
            iQCItemReqDetailListRepository = new IQCItemReqDetailListRepository(this._inventoryUnitOfWork);
            iQCItemReqInfoListRepository = new IQCItemReqInfoListRepository(this._inventoryUnitOfWork);
        }

        public IEnumerable<IQCItemReqDetailList> GetIQCItemReqInfoListByInfo(long infoId, long orgId)
        {
            return iQCItemReqDetailListRepository.GetAll(s => s.IQCItemReqInfoId == infoId && s.OrganizationId == orgId).ToList();
        }

        public IEnumerable<IQCItemReqDetailList> GetIQCItemReqInfoListByOrgId(long orgId)
        {
            return iQCItemReqDetailListRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public IQCItemReqInfoList GetIQCItemReqInfoById(long reqInfoId, long orgId)
        {
            return iQCItemReqInfoListRepository.GetOneByOrg(s => s.IQCItemReqInfoId == reqInfoId && s.OrganizationId == orgId);
        }

        public IQCItemReqDetailList GetIQCItemReqDetailById(long reqDetailId, long reqInfo, long orgId)
        {
            return iQCItemReqDetailListRepository.GetOneByOrg(r => r.IQCItemReqInfoId == reqInfo && r.IQCItemReqDetailId == reqDetailId && r.OrganizationId == orgId);
        }
        public bool SaveIQCItemReq(List<IQCItemReqDetailListDTO> iQCItemReqDetailListDTO, long userId, long orgId)
        {

            List<IQCItemReqDetailList> iQCItemReqDetailLists = new List<IQCItemReqDetailList>();
            IQCItemReqInfoList reqInfo = new IQCItemReqInfoList();
                reqInfo.IQCReqCode = ("IQCR-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));

                reqInfo.DescriptionId = iQCItemReqDetailListDTO.FirstOrDefault().DescriptionId;
                reqInfo.SupplierId = iQCItemReqDetailListDTO.FirstOrDefault().SupplierId;
                reqInfo.EntryDate = DateTime.Now;
                reqInfo.EUserId = userId;
                reqInfo.IQCId = iQCItemReqDetailListDTO.FirstOrDefault().IQCId;
                reqInfo.OrganizationId = orgId;
                reqInfo.Remarks = iQCItemReqDetailListDTO.FirstOrDefault().Remarks;
                reqInfo.WarehouseId = iQCItemReqDetailListDTO.FirstOrDefault().WarehouseId;
                reqInfo.StateStatus = RequisitionStatus.Pending;

                foreach (var item in iQCItemReqDetailListDTO)
                {
                    IQCItemReqDetailList reqDetails = new IQCItemReqDetailList();
                    reqDetails.ItemId = item.ItemId;
                    reqDetails.ItemTypeId = item.ItemTypeId;
                    reqDetails.UnitId = item.UnitId;
                    reqDetails.Quantity = item.Quantity;
                    reqDetails.EntryDate = DateTime.Now;
                    reqDetails.EUserId = userId;
                    reqDetails.OrganizationId = orgId;
                    iQCItemReqDetailLists.Add(reqDetails);
                }
                reqInfo.IQCItemReqDetailLists = iQCItemReqDetailLists;
                iQCItemReqInfoListRepository.Insert(reqInfo);
            //}

            return iQCItemReqDetailListRepository.Save();
        }

        public bool SaveIQCItemRequestIssueByWarehouse(IQCItemReqInfoListDTO model, long orgId, long userId)
        {
            if (model.StateStatus == RequisitionStatus.Approved)
            {
                var reqInfo = GetIQCItemReqInfoById(model.IQCItemReqInfoId, orgId);
                List<WarehouseStockDetailDTO> warehouseStocks = new List<WarehouseStockDetailDTO>();
                foreach (var item in model.IQCItemReqDetails)
                {
                    WarehouseStockDetailDTO warehouse = new WarehouseStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        DescriptionId = reqInfo.DescriptionId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = Convert.ToInt32(item.IssueQty),
                        OrganizationId = orgId,
                        UnitId = item.UnitId,
                        StockStatus = StockStatus.StockOut,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        RefferenceNumber = reqInfo.IQCReqCode,
                        Remarks = "IQC Request has been issued"
                    };
                    warehouseStocks.Add(warehouse);

                    var reqDetail = GetIQCItemReqDetailById(item.IQCItemReqDetailId, reqInfo.IQCItemReqInfoId, orgId);
                    if (reqDetail != null)
                    {
                        reqDetail.IssueQty = item.IssueQty;
                        reqDetail.UpdateDate = DateTime.Now;
                        reqDetail.UpUserId = userId;
                        iQCItemReqDetailListRepository.Update(reqDetail);
                    }
                }
                if (SaveIQCItemRequestStatus(model.IQCItemReqInfoId, model.StateStatus, orgId, userId))
                {
                    //if (iQCItemReqDetailListRepository.Save())
                    //{

                    //}
                    return _warehouseStockDetailBusiness.SaveWarehouseStockOut(warehouseStocks, userId, orgId, string.Empty);
                }
            }
            return false;
        }

        public bool SaveIQCItemRequestStatus(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = GetIQCItemReqInfoById(reqId, orgId);
            //if(status == RequisitionStatus.Approved)
            //{
            //    reqInfo.UpdateDate = DateTime.Now;
            //    reqInfo.UpUserId = userId;
            //    reqInfo.StateStatus = status;
            //    iQCItemReqInfoListRepository.Update(reqInfo);
            //}

            if (status == RequisitionStatus.Rechecked || status == RequisitionStatus.Rejected || status == RequisitionStatus.Approved || status == RequisitionStatus.Accepted)
            {
                reqInfo.UpdateDate = DateTime.Now;
                reqInfo.UpUserId = userId;
                reqInfo.StateStatus = status;
                iQCItemReqInfoListRepository.Update(reqInfo);

                //var reqDetail = GetIQCItemReqDetailById(detId, reqId, orgId);
                //if (reqDetail != null)
                //{
                //    reqDetail.IssueQty = qty;
                //    reqDetail.UpdateDate = DateTime.Now;
                //    reqDetail.UpUserId = userId;
                //    iQCItemReqDetailListRepository.Update(reqDetail);
                //}
            }
           
            return iQCItemReqInfoListRepository.Save();
        }

        public IEnumerable<IQCItemReqDetailListDTO> GetIQCItemReqDetails(long reqId, long orgId)
        {
            return this._inventoryUnitOfWork.Db.Database.SqlQuery<IQCItemReqDetailListDTO>(string.Format(@"Select ird.IQCItemReqDetailId,ird.ItemTypeId,ird.ItemId,ird.UnitId, iri.DescriptionId,iri.WarehouseId,it.ItemName 'ItemType',i.ItemName 'Item',u.UnitName 'Unit',ird.Quantity, ird.IssueQty,au.UserName 'EntryUser',Convert(nvarchar(20),ird.EntryDate,106) 'EntryDate',(ISNULL(ws.StockInQty,0)-ISNULL(ws.StockOutQty,0)) 'AvailableQty'
From tblIQCItemReqDetailList ird
Left Join tblItemTypes it on ird.ItemTypeId = it.ItemId
Left Join tblItems i on ird.ItemId = i.ItemId
Left Join tblUnits u on ird.UnitId = u.UnitId
Inner Join tblIQCItemReqInfoList iri on ird.IQCItemReqInfoId = iri.IQCItemReqInfoId
Left Join tblWarehouses wh on iri.WarehouseId = wh.Id
Left Join tblDescriptions des on iri.DescriptionId =des.DescriptionId
Left Join[Inventory].dbo.tblWarehouseStockInfo ws on iri.WarehouseId = ws.WarehouseId and ird.ItemTypeId = ws.ItemTypeId and ird.ItemId = ws.ItemId and iri.DescriptionId = ws.DescriptionId
Left Join[ControlPanel].dbo.tblApplicationUsers au on ird.EUserId = au.UserId
Where ird.IQCItemReqInfoId ={0}and ird.OrganizationId ={1}", reqId, orgId)).ToList();
//            Inner Join tblIQCItemReqInfoList iri on ird.IQCItemReqInfoId = iri.IQCItemReqInfoId
//Left Join[Inventory].dbo.tblWarehouseStockInfo ws on iri.WarehouseId = ws.WarehouseId and ird.ItemTypeId = ws.ItemTypeId and ird.ItemId = ws.ItemId and iri.DescriptionId = ws.DescriptionId Where iri.IQCItemReqInfoId ={ 0}
//            and iri.OrganizationId ={ 1}
//            ", reqId, orgId)).ToList();
        }

        public IEnumerable<IQCItemReqDetailList> GetIQCReqDetailDetailByInfoId(long id, long orgId)
        {
            return iQCItemReqDetailListRepository.GetAll(s => s.IQCItemReqInfoId == id && s.OrganizationId == orgId).ToList();
        }

        public IEnumerable<IQCItemReqDetailListDTO> GetIQCItemReqDetailList(long? itemTypeId, long? itemId, long? unitId, string lessOrEq, string fDate, string tDate, long orgId)
        {
            IEnumerable<IQCItemReqDetailListDTO> iQCItemReqDetailListDTOs = new List<IQCItemReqDetailListDTO>();
            iQCItemReqDetailListDTOs = this._inventoryUnitOfWork.Db.Database.SqlQuery<IQCItemReqDetailListDTO>(QueryForIQCItemReqDetail(itemTypeId, itemId, unitId, lessOrEq, fDate, tDate, orgId)).ToList();
            return iQCItemReqDetailListDTOs;
        }

        private string QueryForIQCItemReqDetail(long? itemTypeId, long? itemId, long? unitId,string lessOrEq,string fDate,string tDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and ird.OrganizationId={0}", orgId);
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                param += string.Format(@" and ird.Quantity={0}", lessOrEq);
            }
            if (!string.IsNullOrEmpty(fDate) && fDate.Trim() != "" && !string.IsNullOrEmpty(tDate) && tDate.Trim() != "")
            {
                string fromDate = Convert.ToDateTime(fDate).ToString("yyyy-MM-dd");
                string toDate = Convert.ToDateTime(tDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ird.EntryDate as date) between '{0}' and '{1}'", fromDate, toDate);
            }
            else if (!string.IsNullOrEmpty(fDate) && fDate.Trim() != "")
            {
                string fromDate = Convert.ToDateTime(fDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ird.EntryDate as date)='{0}'", fromDate);
            }
            else if (!string.IsNullOrEmpty(tDate) && tDate.Trim() != "")
            {
                string toDate = Convert.ToDateTime(tDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ird.EntryDate as date)='{0}'", toDate);
            }

            query = string.Format(@"Select ird.IQCItemReqDetailId,it.ItemName 'ItemType',i.ItemName 'Item',u.UnitName 'Unit',ird.Quantity,au.UserName 'EntryUser',Convert(nvarchar(20),ird.EntryDate,106) 'EntryDate'
From tblIQCItemReqDetailList ird
Left Join tblItemTypes it on ird.ItemTypeId = it.ItemId
Left Join tblItems i on ird.ItemId  = i.ItemId
Left Join tblUnits u on ird.UnitId= u.UnitId
Left Join [ControlPanel].dbo.tblApplicationUsers au on ird.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
