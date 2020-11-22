using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.Common;
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

        public IEnumerable<Dropdown> GetRepresentativesByZone(long zoneId, long orgId)
        {
            throw new NotImplementedException();
        }

        public Zone GetZoneById(long zoneId, long orgId)
        {
            return this._zoneRepository.GetOneByOrg(s => s.ZoneId == zoneId && s.OrganizationId == orgId);
        }

        public IEnumerable<Zone> GetZones(long orgId)
        {
            return this._zoneRepository.GetAll(s =>s.OrganizationId == orgId);
        }

        public IEnumerable<Dropdown> GetZonesByDistrict(long districtId, long orgId)
        {
            return _zoneRepository.GetAll(s => s.DistrictId == districtId && s.OrganizationId == orgId).OrderBy(s=> s.ZoneName).Select(s => new Dropdown() {text=s.ZoneName,value=s.ZoneId.ToString() }).ToList();
        }

        public IEnumerable<Dropdown> GetZonesWithDistrict(long? districtId, long orgId)
        {
            List<Dropdown> data = new List<Dropdown>();
            data = _salesAndDistribution.Db.Database.SqlQuery<Dropdown>(string.Format(@"Declare @districtId bigint={1}
Select (Cast(zn.ZoneId as Nvarchar(20))+'#'+Cast(ISNULL(dis.DistrictId,0) as Nvarchar(20))) 'value',(zn.ZoneName+' ['+dis.DistrictName+']') 'text' From tblZone zn
Left Join tblDistrict dis on zn.DistrictId =dis.DistrictId and zn.OrganizationId = dis.OrganizationId
Where zn.OrganizationId = {0} and ((@districtId = 0) OR (zn.DistrictId=@districtId))", orgId, districtId??0)).ToList();
            return data;
        }

        public IEnumerable<Dropdown> GetZoneWithDistrictAndDivision(long orgId)
        {
            List<Dropdown> data = new List<Dropdown>();
            data = _salesAndDistribution.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select (Cast(z.ZoneId as Nvarchar(20))+'#'+Cast(dis.DistrictId as Nvarchar(20))+'#'+Cast(div.DivisionId as Nvarchar(20))) 'value',(z.ZoneName+' ['+dis.DistrictName+'-'+div.DivisionName+']') 'text' From tblZone z
Inner Join tblDistrict dis on z.DistrictId = dis.DistrictId
Inner Join tblDivision div on dis.DivisionId = div.DivisionId
Where z.OrganizationId={0}",orgId)).ToList();
            return data;
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
                    ZoneName = dto.ZoneName
                };
                _zoneRepository.Insert(zone);
            }
            else if(dto.ZoneId > 0){
                var zoneInDb = _zoneRepository.GetOneByOrg(s => s.ZoneId == dto.ZoneId && s.OrganizationId == orgId);
                if(zoneInDb != null)
                {
                    zoneInDb.ZoneName = dto.ZoneName;
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
