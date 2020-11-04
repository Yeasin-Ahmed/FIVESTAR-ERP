using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution
{
    public class RSMBusiness : IRSMBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        private readonly RSMRepository _rSMRepository;

        public RSMBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._rSMRepository = new RSMRepository(this._salesAndDistributionDb);
        }

        public RSM GetRSMById(long id, long orgId)
        {
            return _rSMRepository.GetOneByOrg(s => s.RSMID == id && s.OrganizationId == orgId);
        }

        public IEnumerable<RSMDTO> GetRSMInformations(long orgId)
        {
            return _salesAndDistributionDb.Db.Database.SqlQuery<RSMDTO>(string.Format(@"Select rsm.RSMID,rsm.FullName,rsm.[Address],rsm.MobileNo,rsm.Email,rsm.Remarks,rsm.IsActive,rsm.IsAllowToLogIn,
rsm.EntryDate,rsm.EUserId,app.UserName 'EntryUser',rsm.DivisionId,dv.DivisionId,rsm.DistrictId,dis.DistrictName
From tblRSM rsm
Inner Join [ControlPanel].dbo.tblApplicationUsers app on app.UserId = rsm.EUserId
Inner Join tblDivision dv on rsm.DivisionId =dv.DivisionId
Inner Join tblDistrict dis on rsm.DistrictId =dis.DistrictId
Where 1=1 and rsm.OrganizationId={0}", orgId)).ToList();
        }

        public bool SaveRSM(RSMDTO dto, long userId, long orgId)
        {
            bool IsSuccess = false;
            if (dto.RSMID == 0)
            {
                RSM rsm = new RSM()
                {
                    FullName =dto.FullName,
                    Address = dto.Address,
                    Email =dto.Email,
                    MobileNo =dto.MobileNo,
                    IsActive =dto.IsActive,
                    IsAllowToLogIn =dto.IsAllowToLogIn,
                    DistrictId =dto.DistrictId,
                    DivisionId =dto.DivisionId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                };
            }
            else if (dto.RSMID > 0)
            {

            }
            return IsSuccess;
        }
    }
}
