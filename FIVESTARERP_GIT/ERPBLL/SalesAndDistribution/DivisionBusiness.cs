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
    public class DivisionBusiness : IDivisionBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly DivisionRepository _divisionRepository;
        public DivisionBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._divisionRepository = new DivisionRepository(this._salesAndDistribution);
        }
        public Division GetDivisionById(long divisionId, long orgId)
        {
            return this._divisionRepository.GetOneByOrg(s => s.DivisionId == divisionId && s.OrganizationId == orgId);
        }
        public IEnumerable<Division> GetDivisions(long orgId)
        {
            return this._divisionRepository.GetAll(s =>s.OrganizationId == orgId);
        }
        public bool SaveDivision(DivisionDTO dto, long userId, long orgId)
        {
            if(dto.DivisionId == 0)
            {
                Division division = new Division()
                {
                    DivisionId = dto.DivisionId,
                    DivisionName = dto.DivisionName,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId
                };
                _divisionRepository.Insert(division);
            }
            else if(dto.DivisionId > 0){
                var divisionInDb = _divisionRepository.GetOneByOrg(s => s.DivisionId == dto.DivisionId && s.OrganizationId == orgId);
                if(divisionInDb != null)
                {
                    divisionInDb.DivisionName = dto.DivisionName;
                    divisionInDb.IsActive = dto.IsActive;
                    divisionInDb.Remarks = dto.Remarks;
                    divisionInDb.UpUserId = userId;
                    divisionInDb.UpdateDate = DateTime.Now;
                    _divisionRepository.Update(divisionInDb);
                }
            }
            return _divisionRepository.Save();
        }
    }
}
