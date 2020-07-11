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
    public class ProductionToPackagingStockTransferDetailBusiness: IProductionToPackagingStockTransferDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly ProductionToPackagingStockTransferDetailRepository _productionToPackagingStockTransferDetailRepository;

        public ProductionToPackagingStockTransferDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._productionToPackagingStockTransferDetailRepository = new ProductionToPackagingStockTransferDetailRepository(this._productionDb);
        }

        public IEnumerable<ProductionToPackagingStockTransferDetail> GetProductionToPackagingStockTransferDetails(long orgId)
        {
            return _productionToPackagingStockTransferDetailRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<ProductionToPackagingStockTransferDetail> GetProductionToPackagingStockTransferDetailsByInfoId(long transferInfoId, long orgId)
        {
            return _productionToPackagingStockTransferDetailRepository.GetAll(s =>s.PTPSTInfoId == transferInfoId && s.OrganizationId == orgId);
        }

        public IEnumerable<ProductionToPackagingStockTransferDetailDTO> GetProductionToPackagingStockTransferDetailsByQuery(long transferInfoId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<ProductionToPackagingStockTransferDetailDTO>(QueryForProductionToPackagingStockTransferDetails(transferInfoId,orgId)).ToList();
        }

        private string QueryForProductionToPackagingStockTransferDetails(long transferInfoId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and trans.OrganizationId={0}",orgId);
            if(transferInfoId > 0)
            {
                param += string.Format(@" and trans.PTPSTInfoId={0}", transferInfoId);
            }
            
            query = string.Format(@"Select trans.PTPSTInfoId,W.WarehouseName,it.ItemName 'ItemTypeName', i.ItemName,trans.Quantity 
From tblProductionToPackagingStockTransferDetail trans
Inner Join [Inventory].dbo.tblWarehouses w on trans.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItems i on trans.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblItemTypes it on trans.ItemTypeId = it.ItemId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
