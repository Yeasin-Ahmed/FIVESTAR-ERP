﻿using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Inventory.DomainModels;
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
        private readonly DescriptionRepository descriptionRepository; // table
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;

        public DescriptionBusiness(IInventoryUnitOfWork inventoryDb, IProductionStockInfoBusiness productionStockInfoBusiness)
        {
            this._inventoryDb = inventoryDb;
            descriptionRepository = new DescriptionRepository(this._inventoryDb);
            this._productionStockInfoBusiness = productionStockInfoBusiness;
        }

        public IEnumerable<Dropdown> GetAllDescriptionsInProductionStock(long orgId)
        {
            var modelInPDN = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(orgId).Select(s => s.DescriptionId).Distinct().ToList();
          return  GetDescriptionByOrgId(orgId).Where(d => modelInPDN.Contains(d.DescriptionId)).OrderBy(d => d.DescriptionName).Select(s => new Dropdown { text = s.DescriptionName, value = s.DescriptionId.ToString() }).ToList();
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