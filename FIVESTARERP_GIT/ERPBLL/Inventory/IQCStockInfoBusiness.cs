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
    public class IQCStockInfoBusiness : IIQCStockInfoBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private IQCStockInfoRepository IQCStockInfoRepository;
        public IQCStockInfoBusiness(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            this._inventoryUnitOfWork = inventoryUnitOfWork;
            IQCStockInfoRepository = new IQCStockInfoRepository(this._inventoryUnitOfWork);
        }
        public IEnumerable<IQCStockInfo> GetAllIQCStockInfoByOrgId(long orgId)
        {
            return IQCStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<IQCStockInfoDTO> GetAllIQCStockInformationList(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            IEnumerable<IQCStockInfoDTO> iQCStockInfoDTOs = new List<IQCStockInfoDTO>();
            iQCStockInfoDTOs = this._inventoryUnitOfWork.Db.Database.SqlQuery<IQCStockInfoDTO>(QueryForStockInfoList(warehouseId, modelId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
            return iQCStockInfoDTOs;
        }

        private string QueryForStockInfoList(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and si.OrganizationId = {0}", orgId);

            if (warehouseId != null && warehouseId>0)
            {
                param += string.Format(@" and wh.Id = {0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and des.DescriptionId={0}", modelId);
            }
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
                param += string.Format(@" and (si.StockInQty - si.StockOutQty)='{0}'", lessOrEq);
            }

            query = string.Format(@"Select si.StockInfoId, wh.WarehouseName, des.DescriptionName 'ModelName', it.ItemName 'ItemTypeName',i.ItemName,u.UnitSymbol, si.StockInQty, si.StockOutQty, si.Remarks
From tblIQCStockInfo si
Left Join tblWarehouses wh on si.WarehouseId = wh.Id
Left Join tblDescriptions des on si.DescriptionId =des.DescriptionId
Left Join tblItemTypes it on si.ItemTypeId = it.ItemId
Left Join tblItems i on si.ItemId  = i.ItemId
Left Join tblUnits u on si.UnitId= u.UnitId      
Left Join [ControlPanel].dbo.tblApplicationUsers au on si.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
