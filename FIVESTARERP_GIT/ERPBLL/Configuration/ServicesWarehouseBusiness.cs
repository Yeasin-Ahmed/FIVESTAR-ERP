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
        public bool DeleteServicesWarehouse(long id, long orgId,long branchId)
        {
            servicesWarehouseRepository.DeleteOneByOrg(ware => ware.SWarehouseId == id && ware.OrganizationId == orgId && ware.BranchId== branchId);
            return servicesWarehouseRepository.Save();
        }

        public IEnumerable<ServiceWarehouse> GetAllServiceWarehouseByOrgId(long orgId, long branchId)
        {
            return servicesWarehouseRepository.GetAll(ware => ware.OrganizationId == orgId && ware.BranchId== branchId).ToList();
        }

        public IEnumerable<ServicesWarehouseDTO> GetServiceWarehouseByOrgId(long orgId, long branchId)
        {
            return _configurationDb.Db.Database.SqlQuery<ServicesWarehouseDTO>(string.Format(@"select * from [Configuration].dbo.tblServiceWarehouses
where OrganizationId={0} and BranchId={1}", orgId, branchId)).ToList();
        }

        public ServiceWarehouse GetServiceWarehouseOneByOrgId(long id, long orgId, long branchId)
        {
            return servicesWarehouseRepository.GetOneByOrg(ware => ware.SWarehouseId == id && ware.OrganizationId == orgId && ware.BranchId == branchId);
        }

        public ServiceWarehouse GetWarehouseOneByOrgId(long orgId, long branchId)
        {
            return servicesWarehouseRepository.GetOneByOrg(ware => ware.OrganizationId == orgId && ware.BranchId == branchId);
        }

        public bool IsDuplicateServicesWarehouseName(string sWName, long id, long orgId, long branchId)
        {
            return servicesWarehouseRepository.GetOneByOrg(ware => ware.ServicesWarehouseName == sWName && ware.SWarehouseId != id && ware.OrganizationId == orgId && ware.BranchId == branchId) != null ? true : false;
        }

        public bool SaveServiceWarehouse(ServicesWarehouseDTO servicesWarehouseDTO, long userId, long orgId, long branchId)
        {
            ServiceWarehouse warehouse = new ServiceWarehouse();
            if (servicesWarehouseDTO.SWarehouseId == 0)
            {
                warehouse.ServicesWarehouseName = servicesWarehouseDTO.ServicesWarehouseName;
                warehouse.Remarks = servicesWarehouseDTO.Remarks;
                warehouse.OrganizationId = orgId;
                warehouse.BranchId = branchId;
                warehouse.EUserId = userId;
                warehouse.EntryDate = DateTime.Now;
                servicesWarehouseRepository.Insert(warehouse);
            }
            else
            {
                warehouse = GetServiceWarehouseOneByOrgId(servicesWarehouseDTO.SWarehouseId, orgId,branchId);
                warehouse.ServicesWarehouseName = servicesWarehouseDTO.ServicesWarehouseName;
                warehouse.Remarks = servicesWarehouseDTO.Remarks;
                warehouse.OrganizationId = orgId;
                warehouse.BranchId = branchId;
                warehouse.UpUserId = userId;
                warehouse.UpdateDate = DateTime.Now;
                servicesWarehouseRepository.Update(warehouse);
            }
           return servicesWarehouseRepository.Save();
        }
    }
}
