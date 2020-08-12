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
    public class MiniStockTransferDetailBusiness : IMiniStockTransferDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly MiniStockTransferDetailRepository _miniStockTransferDetailRepository;
        public MiniStockTransferDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._miniStockTransferDetailRepository = new MiniStockTransferDetailRepository(this._productionDb);
        }
        public IEnumerable<MiniStockTransferDetail> GetMiniStockTransfersByInfo(long infoId, long orgId)
        {
            return this._miniStockTransferDetailRepository.GetAll(s => s.MSTInfoId == infoId && s.OrganizationId == orgId);
        }

        public IEnumerable<MiniStockTransferDetail> GetMiniStockTransfersByOrg(long orgId)
        {
            return this._miniStockTransferDetailRepository.GetAll(s =>s.OrganizationId == orgId);
        }

        public IEnumerable<MiniStockTransferDetailDTO> GetMiniStockTransfersDetailByQuery(long? modelId, long? warehouseId, long? itemTypeId,long? itemid, long? infoId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<MiniStockTransferDetailDTO>(QueryForMiniStockTransfersDetail(modelId, warehouseId, itemTypeId, itemid, infoId, orgId)).ToList();
        }

        private string QueryForMiniStockTransfersDetail(long? modelId, long? warehouseId, long? itemTypeId, long? itemid, long? infoId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and msd.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and msd.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and msd.ItemTypeId={0}", itemTypeId);
            }
            if (itemid != null && itemid > 0)
            {
                param += string.Format(@" and msd.ItemId={0}", itemid);
            }
            if (infoId != null && infoId > 0)
            {
                param += string.Format(@" and msd.MSTInfoId={0}", infoId);
            }

            query = string.Format(@"Select msd.MSTDetailId,msd.DescriptionId,de.DescriptionName 'ModelName',msd.WarehouseId,w.WarehouseName,msd.ItemTypeId,it.ItemName 'ItemTypeName',msd.ItemId,i.ItemName,msd.Quantity,msd.UnitId,u.UnitSymbol 'UnitName' 
From tblMiniStockTransferDetail msd
Inner Join tblMiniStockTransferInfo msi on msd.MSTInfoId = msi.MSTInfoId
Left Join [Inventory].dbo.tblDescriptions de on msd.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on msd.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on msd.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on msd.ItemId= i.ItemId
Left Join [Inventory].dbo.tblUnits u on msd.UnitId= u.UnitId
Where 1=1 and msd.OrganizationId={0} {1}", orgId,Utility.ParamChecker(param));
            return query;
        }
    }
}
