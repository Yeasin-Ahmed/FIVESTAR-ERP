using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class ServicesWarehouseBusiness : IServicesWarehouseBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly ServicesWarehouseRepository servicesWarehouseRepository; // repo
        public ServicesWarehouseBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            servicesWarehouseRepository = new ServicesWarehouseRepository(this._configurationDb);
        }
        public bool DeleteServicesWarehouse(long id, long orgId)
        {
            servicesWarehouseRepository.DeleteOneByOrg(ware => ware.SWarehouseId == id && ware.OrganizationId == orgId);
            return servicesWarehouseRepository.Save();
        }

        public IEnumerable<ServiceWarehouse> GetAllServiceWarehouseByOrgId(long orgId)
        {
            return servicesWarehouseRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }

        public ServiceWarehouse GetServiceWarehouseOneByOrgId(long id, long orgId)
        {
            return servicesWarehouseRepository.GetOneByOrg(ware => ware.SWarehouseId == id && ware.OrganizationId == orgId);
        }

        public bool IsDuplicateServicesWarehouseName(string sWName, long id, long orgId)
        {
            return servicesWarehouseRepository.GetOneByOrg(ware => ware.ServicesWarehouseName == sWName && ware.SWarehouseId != id && ware.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveServiceWarehouse(ServicesWarehouseDTO servicesWarehouseDTO, long userId, long orgId)
        {
            ServiceWarehouse warehouse = new ServiceWarehouse();
            if (servicesWarehouseDTO.SWarehouseId == 0)
            {
                warehouse.ServicesWarehouseName = servicesWarehouseDTO.ServicesWarehouseName;
                warehouse.Remarks = servicesWarehouseDTO.Remarks;
                warehouse.OrganizationId = orgId;
                warehouse.EUserId = userId;
                warehouse.EntryDate = DateTime.Now;
                servicesWarehouseRepository.Insert(warehouse);
            }
            else
            {
                warehouse = GetServiceWarehouseOneByOrgId(servicesWarehouseDTO.SWarehouseId, orgId);
                warehouse.ServicesWarehouseName = servicesWarehouseDTO.ServicesWarehouseName;
                warehouse.Remarks = servicesWarehouseDTO.Remarks;
                warehouse.OrganizationId = orgId;
                warehouse.UpUserId = userId;
                warehouse.UpdateDate = DateTime.Now;
                servicesWarehouseRepository.Update(warehouse);
            }
           return servicesWarehouseRepository.Save();
        }
    }
}
