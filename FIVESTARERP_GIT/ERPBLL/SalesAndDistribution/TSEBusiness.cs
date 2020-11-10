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
    public class TSEBusiness : ITSEBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        private readonly TSERepository _tseRepository;
        private readonly IAppUserBusiness _appUserBusiness;

        public TSEBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb, IAppUserBusiness appUserBusiness)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._tseRepository = new TSERepository(this._salesAndDistributionDb);
            this._appUserBusiness = appUserBusiness;
        }

        public TSE GetTSEById(long id, long orgId)
        {
            return _tseRepository.GetOneByOrg(s => s.TSEID ==id && s.OrganizationId == orgId);
        }

        public IEnumerable<TSEDTO> GetTSEInformations(long orgId, long userId)
        {
            return _salesAndDistributionDb.Db.Database.SqlQuery<TSEDTO>(string.Format(@"Select tse.TSEID,tse.FullName,tse.[Address],tse.MobileNo,tse.Email,tse.Remarks,tse.IsActive,tse.IsAllowToLogIn,
tse.EntryDate,tse.EUserId,app.UserName 'EntryUser',z.ZoneName,tse.ZoneId,tse.DivisionId,dv.DivisionName,tse.DistrictId,dis.DistrictName,
ISNULL(tse.UserId,0) 'UserId',tse.ASMUserId,asm.FullName 'ASMName'
From tblTSE tse
Inner Join tblASM asm on tse.ASMUserId = asm.UserId
Left Join [ControlPanel].dbo.tblApplicationUsers app on app.UserId = tse.EUserId
Left Join tblZone z on tse.ZoneId =z.ZoneId
Left Join tblDivision dv on tse.DivisionId =dv.DivisionId
Left Join tblDistrict dis on tse.DistrictId =dis.DistrictId
Where 1=1 and tse.OrganizationId={0} and  tse.EUserId={1}", orgId, userId)).ToList();
        }

        public bool SaveTSE(TSEDTO dto, SRUser sRUser, long userId, long branchId, long orgId)
        {
            TSE tse = null;
            string tseUserId = "0";
            if (dto.TSEID == 0)
            {
                tse = new TSE()
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
                    BranchId = branchId,
                    ASMUserId = userId,
                    ZoneId = dto.ZoneId,
                    Remarks = dto.Remarks
                };
                if (dto.IsAllowToLogIn)
                {
                    SaveTSEUser(dto, sRUser, userId, branchId, orgId, out tseUserId);
                    tse.UserId = Convert.ToInt64(tseUserId);
                }
                else
                {
                    tse.UserId = 0;
                }
                _tseRepository.Insert(tse);
            }
            else if (dto.TSEID > 0)
            {
                tse = this.GetTSEById(dto.TSEID, orgId);
                if (tse != null)
                {
                    tse.FullName = dto.FullName;
                    tse.Address = dto.Address;
                    tse.Email = dto.Email;
                    tse.MobileNo = dto.MobileNo;
                    tse.IsActive = dto.IsActive;
                    tse.IsAllowToLogIn = dto.IsAllowToLogIn;
                    tse.ZoneId = dto.ZoneId;
                    tse.DistrictId = dto.DistrictId;
                    tse.DivisionId = dto.DivisionId;
                    tse.UpUserId = userId;
                    tse.UpdateDate = DateTime.Now;
                    tse.Remarks = dto.Remarks;
                    if (dto.IsAllowToLogIn && tse.UserId == 0)
                    {
                        SaveTSEUser(dto, sRUser, userId, branchId, orgId, out tseUserId);
                        tse.UserId = Convert.ToInt64(tseUserId);
                    }
                    _tseRepository.Update(tse);
                }
            }
            return _tseRepository.Save();
        }

        private void SaveTSEUser(TSEDTO dto, SRUser sRUser, long userId, long branchId, long orgId, out string asmUserId)
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
            _appUserBusiness.SaveSRAppUser(appUser, userId, orgId, "ASM", out asmUserId);
        }
    }
}
