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
    public class StockItemReturnDetailBusiness : IStockItemReturnDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly StockItemReturnDetailRepository _stockItemReturnDetailRepository;
        public StockItemReturnDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._stockItemReturnDetailRepository = new StockItemReturnDetailRepository(this._productionDb);
        }
        public IEnumerable<StockItemReturnDetailDTO> GetStockItemReturnDetails(long infoId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<StockItemReturnDetailDTO>(string.Format(@"Select srd.SIRDetailId,srd.WarehouseId,w.WarehouseName,srd.ItemTypeId,it.ItemName 'ItemTypeName',srd.ItemId, i.ItemName,srd.Quantity,srd.UnitId, u.UnitSymbol 'UnitName' 
From tblStockItemReturnDetail srd
Inner Join [Inventory].dbo.tblWarehouses w on srd.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on srd.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on srd.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblUnits u on srd.UnitId = u.UnitId
Where 1=1 and srd.OrganizationId ={0} and srd.SIRInfoId={1}", orgId, infoId)).ToList();
        }

        public IEnumerable<StockItemReturnDetail> GetStockItemReturnDetailsByInfo(long infoId, long orgId)
        {
            return this._stockItemReturnDetailRepository.GetAll(s => s.SIRInfoId == infoId && s.OrganizationId == orgId);
        }
    }
}
