using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
   public class DescriptionBusiness: IDescriptionBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly DescriptionRepository descriptionRepository; // table
        private IInventoryUnitOfWork _inventoryDb;

        public DescriptionBusiness(IProductionUnitOfWork productionDb, IInventoryUnitOfWork inventoryDb)
        {
            this._productionDb = productionDb;
            descriptionRepository = new DescriptionRepository(this._productionDb);
            this._inventoryDb = inventoryDb;
        }
       

        public IEnumerable<Description> GetDescriptionByOrgId(long orgId)
        {
            return descriptionRepository.GetAll(des => des.OrganizationId == orgId).ToList();
        }

        public Description GetDescriptionOneByOrdId(long id, long orgId)
        {
            return descriptionRepository.GetOneByOrg(des => des.DescriptionId == id && des.OrganizationId == orgId);
        }
    }
}
