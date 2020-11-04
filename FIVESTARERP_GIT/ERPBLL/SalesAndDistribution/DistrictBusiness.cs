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
    public class DistrictBusiness : IDistrictBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly DistrictRepository _districtRepository;
        public DistrictBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._districtRepository = new DistrictRepository(this._salesAndDistribution);
        }
        public District GetDistrictById(long divisionId, long orgId)
        {
            return this._districtRepository.GetOneByOrg(s => s.DivisionId == divisionId && s.OrganizationId == orgId);
        }
        public IEnumerable<District> GetDistricts(long orgId)
        {
            return this._districtRepository.GetAll(s =>s.OrganizationId == orgId);
        }
        public bool SaveDistrict(DistrictDTO dto, long userId, long orgId)
        {
            if(dto.DivisionId == 0)
            {
                District district = new District()
                {
                    DistrictId = dto.DistrictId,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    DivisionId = dto.DivisionId,
                    DistrictName = dto.DistrictName
                };
                _districtRepository.Insert(district);
            }
            else if(dto.DistrictId > 0){
                var districtInDb = _districtRepository.GetOneByOrg(s => s.DistrictId == dto.DistrictId && s.OrganizationId == orgId);
                if(districtInDb != null)
                {
                    districtInDb.DistrictName = dto.DistrictName;
                    districtInDb.IsActive = dto.IsActive;
                    districtInDb.Remarks = dto.Remarks;
                    districtInDb.UpUserId = userId;
                    districtInDb.UpdateDate = DateTime.Now;
                    _districtRepository.Update(districtInDb);
                }
            }
            return _districtRepository.Save();
        }
    }
}
