using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;

namespace ERPBLL.SalesAndDistribution
{
    public class ZoneBusiness : IZoneBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly ZoneRepository _zoneRepository;
        public ZoneBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._zoneRepository = new ZoneRepository(this._salesAndDistribution);
        }

        public Zone GetZoneById(long zoneId, long orgId)
        {
            return this._zoneRepository.GetOneByOrg(s => s.ZoneId == zoneId && s.OrganizationId == orgId);
        }

        public IEnumerable<Zone> GetZones(long orgId)
        {
            return this._zoneRepository.GetAll(s =>s.OrganizationId == orgId);
        }

        public bool SaveZone(ZoneDTO dto, long userId, long orgId)
        {
            if(dto.ZoneId == 0)
            {
                Zone zone = new Zone()
                {
                    DistrictId = dto.DistrictId,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    DivisionId = dto.DivisionId,
                    ZoneId = dto.ZoneId,
                    ZoneName = dto.DistrictName
                };
                _zoneRepository.Insert(zone);
            }
            else if(dto.ZoneId > 0){
                var zoneInDb = _zoneRepository.GetOneByOrg(s => s.ZoneId == dto.ZoneId && s.OrganizationId == orgId);
                if(zoneInDb != null)
                {
                    zoneInDb.ZoneName = dto.DistrictName;
                    zoneInDb.IsActive = dto.IsActive;
                    zoneInDb.Remarks = dto.Remarks;
                    zoneInDb.UpUserId = userId;
                    zoneInDb.UpdateDate = DateTime.Now;
                    _zoneRepository.Update(zoneInDb);
                }
            }
            return _zoneRepository.Save();
        }
    }
}
