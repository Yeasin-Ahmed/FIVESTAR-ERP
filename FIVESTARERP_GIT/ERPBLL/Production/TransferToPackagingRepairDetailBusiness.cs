using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class TransferToPackagingRepairDetailBusiness : ITransferToPackagingRepairDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferToPackagingRepairDetailRepository _transferToPackagingRepairDetailRepository;

        public TransferToPackagingRepairDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferToPackagingRepairDetailRepository = new TransferToPackagingRepairDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferToPackagingRepairDetailDTO> GetTransferToPackagingRepairDetailsByQuery(long? transferId, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string transferCode, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferToPackagingRepairDetailDTO>(QueryForTransferToPackagingRepairDetails(transferId, floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, transferCode, orgId)).ToList();
        }

        public IEnumerable<TransferToPackagingRepairDetail> GetTransferToPackagingRepairDetailsByTransferId(long transferId, long orgId)
        {
            return this._transferToPackagingRepairDetailRepository.GetAll(s => s.TPRInfoId == transferId && s.OrganizationId == orgId);
        }

        private string QueryForTransferToPackagingRepairDetails(long? transferId, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string transferCode, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (transferId != null && transferId > 0)
            {
                param += string.Format(@" and detail.TPRInfoId={0}", transferId);
            }
            if (floorId !=null && floorId > 0)
            {
                param += string.Format(@" and detail.ProductionFloorId={0}",floorId);
            }
            if (packagingLineId != null && packagingLineId > 0)
            {
                param += string.Format(@" and detail.PackagingLineId={0}", packagingLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and detail.DescriptionId={0}", modelId);
            }
            if(warehouseId !=null && warehouseId > 0)
            {
                param += string.Format(@" and detail.WarehouseId={0}", warehouseId);
            }
            if(itemTypeId !=null && itemTypeId > 0)
            {
                param += string.Format(@" and detail.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and detail.ItemId={0}", itemId);
            }
            if(!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and detail.TransferCode LIKE'%{0}%'", transferCode);
            }

            query = string.Format(@"Select detail.TPRInfoId,detail.ProductionFloorId,pl.LineNumber 'ProductionFloorName',detail.PackagingLineId,pac.PackagingLineName,detail.DescriptionId,de.DescriptionName 'ModelName',
detail.WarehouseId,wa.WarehouseName,detail.ItemTypeId,it.ItemName 'ItemTypeName',detail.ItemId,i.ItemName,detail.UnitId,u.UnitSymbol 
'UnitName',detail.Quantity
From [Production].dbo.tblTransferToPackagingRepairDetail detail
Inner Join [Production].dbo.tblProductionLines pl on detail.ProductionFloorId = pl.LineId
Left Join [Production].dbo.tblPackagingLine  pac on detail.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblDescriptions de on detail.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses wa on detail.WarehouseId = wa.Id
Left Join [Inventory].dbo.tblItemTypes it on detail.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on detail.ItemId = i.ItemId
Left Join [Inventory].dbo.tblUnits u on detail.UnitId = u.UnitId Where 1=1 and detail.OrganizationId={0} {1}",orgId,Utility.ParamChecker(param));
            return query;
        }
    }
}
