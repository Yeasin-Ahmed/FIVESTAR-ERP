using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class FinishGoodsRowMaterialBusiness : IFinishGoodsRowMaterialBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly FinishGoodsRowMaterialRepository _finishGoodsRowMaterialRepository; //

        public FinishGoodsRowMaterialBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._finishGoodsRowMaterialRepository = new FinishGoodsRowMaterialRepository(this._productionDb);
        }
        public IEnumerable<FinishGoodsRowMaterial> GetGoodsRowMaterialsByOrg(long orgId)
        {
            return _finishGoodsRowMaterialRepository.GetAll(f => f.OrganizationId == orgId).ToList();
        }

        public IEnumerable<FinishGoodsRowMaterial> GetGoodsRowMaterialsByOrgAndFinishInfoId(long orgId, long finishGoodsInfoId)
        {
            return _finishGoodsRowMaterialRepository.GetAll(f => f.OrganizationId == orgId && f.FinishGoodsInfoId == finishGoodsInfoId).ToList();
        }
    }
}
