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
    public class IQCItemReqInfoListBusiness : IIQCItemReqInfoList
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IQCItemReqInfoListRepository iQCItemReqInfoListRepository;

        public IQCItemReqInfoListBusiness(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            this._inventoryUnitOfWork = inventoryUnitOfWork;
            iQCItemReqInfoListRepository = new IQCItemReqInfoListRepository(this._inventoryUnitOfWork);
        }

        public IQCItemReqInfoList GetIQCItemReqById(long? reqId, long orgId)
        {
            return iQCItemReqInfoListRepository.GetOneByOrg(s => s.IQCItemReqInfoId == reqId && s.OrganizationId == orgId);
        }

        public IQCItemReqInfoListDTO GetIQCItemReqDataById(long reqId, long orgId)
        {
            return this._inventoryUnitOfWork.Db.Database.SqlQuery<IQCItemReqInfoListDTO>(string.Format(@"Select iri.IQCItemReqInfoId, iri.WarehouseId, iri.DescriptionId, iri.IQCId, iri.SupplierId, iri.IQCReqCode, iri.StateStatus, wh.WarehouseName 'Warehouse', des.DescriptionName 'ModelName', sup.SupplierName 'Supplier', iq.IQCName, au.UserName 'EntryUser',iri.EntryDate, iri.Remarks
From tblIQCItemReqInfoList iri
Left Join tblWarehouses wh on iri.WarehouseId = wh.Id
Left Join tblDescriptions des on iri.DescriptionId =des.DescriptionId
Left Join tblIQCList iq on iri.IQCId = iq.Id
Left Join tblSupplier sup on iri.SupplierId = sup.SupplierId      
Left Join [ControlPanel].dbo.tblApplicationUsers au on iri.EUserId = au.UserId
Where 1=1 and iri.OrganizationId={0} and iri.IQCItemReqInfoId={1}", orgId, reqId)).Single();
        }

        public IEnumerable<IQCItemReqInfoList> GetIQCItemReqInfoListByOrgId(long orgId)
        {
            return iQCItemReqInfoListRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public bool SaveIQCReqInfoStatus(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = iQCItemReqInfoListRepository.GetOneByOrg(req => req.IQCItemReqInfoId == reqId && req.OrganizationId == orgId);
            if (reqInfo != null)
            {
                reqInfo.StateStatus = status;
                reqInfo.UpUserId = userId;
                reqInfo.UpdateDate = DateTime.Now;
                iQCItemReqInfoListRepository.Update(reqInfo);
            }
            return iQCItemReqInfoListRepository.Save();
        }

        public IEnumerable<IQCItemReqInfoListDTO> GetIQCItemReqInfoLists(long? warehouseId, long? modelId, string status, long orgId)
        {
            IEnumerable<IQCItemReqInfoListDTO> iQCItemReqInfoListDTOs = new List<IQCItemReqInfoListDTO>();
            iQCItemReqInfoListDTOs = this._inventoryUnitOfWork.Db.Database.SqlQuery<IQCItemReqInfoListDTO>(QueryForIQCItemReqInfoList(warehouseId, modelId, status, orgId)).ToList();
            return iQCItemReqInfoListDTOs;
        }

        private string QueryForIQCItemReqInfoList(long? warehouseId, long? modelId, string status, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and iri.OrganizationId = {0}", orgId);
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId={0}", modelId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and iri.StateStatus In({0})", status);
            }

            query = string.Format(@"Select iri.IQCItemReqInfoId, wh.WarehouseName 'Warehouse', de.DescriptionName 'ModelName', iq.IQCName, iri.StateStatus,iri.Remarks, sup.SupplierName 'Supplier',au.UserName 'ReturnUser',au.UserName 'ReturnReaciveUser',iri.ReturnUserDate,iri.ReturnReaciveUserDate
From tblIQCItemReqInfoList iri
Left Join tblWarehouses wh on iri.WarehouseId = wh.Id
Left Join tblDescriptions de on iri.DescriptionId =de.DescriptionId
Left Join tblIQCList iq on iri.IQCId = iq.Id
Left Join tblSupplier sup on iri.SupplierId = sup.SupplierId      
Left Join [ControlPanel].dbo.tblApplicationUsers au on iri.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
