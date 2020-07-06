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
   public class ServiceBusiness: IServiceBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly ServiceRepository serviceRepository; // repo
        public ServiceBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            serviceRepository = new ServiceRepository(this._configurationDb);
        }

        public bool DeleteService(long id, long orgId)
        {
            serviceRepository.DeleteOneByOrg(s => s.ServiceId == id && s.OrganizationId == orgId);
            return serviceRepository.Save();
        }

        public IEnumerable<Service> GetAllServiceByOrgId(long orgId)
        {
            return serviceRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public Service GetServiceOneByOrgId(long id, long orgId)
        {
            return serviceRepository.GetOneByOrg(s => s.ServiceId == id && s.OrganizationId == orgId);
        }

        public bool IsDuplicateServiceName(string name, long id, long orgId)
        {
            return serviceRepository.GetOneByOrg(s => s.ServiceName == name && s.ServiceId != id && s.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveService(ServiceDTO serviceDTO, long userId, long orgId)
        {
            Service service = new Service();
            if (serviceDTO.ServiceId == 0)
            {
                service.ServiceName = serviceDTO.ServiceName;
                service.ServiceCode = serviceDTO.ServiceCode;
                service.Remarks = serviceDTO.Remarks;
                service.OrganizationId = orgId;
                service.EUserId = userId;
                service.EntryDate = DateTime.Now;
                serviceRepository.Insert(service);
            }
            else
            {
                service = GetServiceOneByOrgId(serviceDTO.ServiceId, orgId);
                service.ServiceName = serviceDTO.ServiceName;
                service.ServiceCode = serviceDTO.ServiceCode;
                service.Remarks = serviceDTO.Remarks;
                service.OrganizationId = orgId;
                service.UpUserId = userId;
                service.UpdateDate = DateTime.Now;
                serviceRepository.Update(service);
            }
            return serviceRepository.Save();
        }
    }
}
