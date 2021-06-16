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
   public class STransferDetailsMToMBusiness: ISTransferDetailsMToMBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly STransferDetailsMToMRepository sTransferDetailsMToMRepository; // repo
        public STransferDetailsMToMBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            sTransferDetailsMToMRepository = new STransferDetailsMToMRepository(this._inventoryDb);
        }

        public IEnumerable<StockTransferDetailsMToM> GetDetailsDataOneByOrg(long id, long orgId)
        {
            return sTransferDetailsMToMRepository.GetAll(d => d.STransferInfoId == id && d.OrganizationId == orgId).ToList();
        }
    }
}
