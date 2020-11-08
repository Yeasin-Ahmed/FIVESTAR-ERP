using ERPBLL.Common;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.ControlPanel.DTOModels;
using ERPBO.SalesAndDistribution.CommonModels;
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
        private readonly IAppUserBusiness _appUserBusiness;
        public RSMBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb, IAppUserBusiness appUserBusiness)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._rSMRepository = new RSMRepository(this._salesAndDistributionDb);
            this._appUserBusiness = appUserBusiness;
        }
        public RSM GetRSMById(long id, long orgId)
        {
            return _rSMRepository.GetOneByOrg(s => s.RSMID == id && s.OrganizationId == orgId);
        }
        public IEnumerable<RSM> GetRSMByOrg(long orgId)
        {
            return _rSMRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public IEnumerable<RSMDTO> GetRSMInformations(long orgId)
        {
            return _salesAndDistributionDb.Db.Database.SqlQuery<RSMDTO>(string.Format(@"Select rsm.RSMID,rsm.FullName,rsm.[Address],rsm.MobileNo,rsm.Email,rsm.Remarks,rsm.IsActive,rsm.IsAllowToLogIn,
rsm.EntryDate,rsm.EUserId,app.UserName 'EntryUser',rsm.DivisionId,dv.DivisionName,rsm.DistrictId,dis.DistrictName,ISNULL(rsm.UserId,0) 'UserId'
From tblRSM rsm
Inner Join [ControlPanel].dbo.tblApplicationUsers app on app.UserId = rsm.EUserId
Inner Join tblDivision dv on rsm.DivisionId =dv.DivisionId
Inner Join tblDistrict dis on rsm.DistrictId =dis.DistrictId
Where 1=1 and rsm.OrganizationId={0}", orgId)).ToList();
        }
        public bool SaveRSM(RSMDTO dto, SRUser sRUser, long userId, long branchId, long orgId)
        {
            RSM rsm = null;
            string rsmUserId = "0";
            if (dto.RSMID == 0)
            {
                rsm = new RSM()
                {
                    FullName = dto.FullName,
                    Address = dto.Address,
                    Email = dto.Email,
                    MobileNo = dto.MobileNo,
                    IsActive = dto.IsActive,
                    IsAllowToLogIn = dto.IsAllowToLogIn,
                    DistrictId = dto.DistrictId,
                    DivisionId = dto.DivisionId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    BranchId = branchId
                };
                if (dto.IsAllowToLogIn)
                {
                    SaveRSMUser(dto, sRUser, userId, branchId, orgId, out rsmUserId);
                    rsm.UserId = Convert.ToInt64(rsmUserId);
                }
                else
                {
                    rsm.UserId = 0;
                }
                _rSMRepository.Insert(rsm);
            }
            else if (dto.RSMID > 0)
            {
                rsm = this.GetRSMById(dto.RSMID, orgId);
                if (rsm != null)
                {
                    rsm.FullName = dto.FullName;
                    rsm.Address = dto.Address;
                    rsm.Email = dto.Email;
                    rsm.MobileNo = dto.MobileNo;
                    rsm.IsActive = dto.IsActive;
                    rsm.IsAllowToLogIn = dto.IsAllowToLogIn;
                    rsm.DistrictId = dto.DistrictId;
                    rsm.DivisionId = dto.DivisionId;
                    rsm.UpUserId = userId;
                    rsm.UpdateDate = DateTime.Now;

                    if (dto.IsAllowToLogIn && rsm.UserId == 0)
                    {
                        SaveRSMUser(dto, sRUser, userId, branchId, orgId, out rsmUserId);
                        rsm.UserId = Convert.ToInt64(rsmUserId);
                    }

                    _rSMRepository.Update(rsm);
                }
            }
            return _rSMRepository.Save();
        }
        private void SaveRSMUser(RSMDTO dto, SRUser sRUser, long userId, long branchId, long orgId, out string rsmUserId)
        {
            AppUserDTO appUser = new AppUserDTO()
            {
                FullName = dto.FullName,
                EmployeeId = (DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")).ToString(),
                Address = dto.Address,
                BranchId = branchId,
                OrganizationId = orgId,
                Email = dto.Email,
                MobileNo = dto.MobileNo,
                IsActive = dto.IsActive,
                EUserId = userId,
                EntryDate = DateTime.Now,
                UserName = sRUser.UserName,
                Password = Utility.Encrypt(sRUser.Password),
                ConfirmPassword = Utility.Encrypt(sRUser.Password),
                IsRoleActive = true
            };
            _appUserBusiness.SaveSRAppUser(appUser, userId, orgId,"RSM", out rsmUserId);
        }
    }
}
