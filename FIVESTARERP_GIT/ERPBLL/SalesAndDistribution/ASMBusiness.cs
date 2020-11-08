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
    public class ASMBusiness : IASMBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        private readonly ASMRepository _aSMRepository;
        private readonly IAppUserBusiness _appUserBusiness;
        public ASMBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb, IAppUserBusiness appUserBusiness)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._aSMRepository = new ASMRepository(this._salesAndDistributionDb);
            this._appUserBusiness = appUserBusiness;
        }
        public ASM GetASMById(long id, long orgId)
        {
            return _aSMRepository.GetOneByOrg(s => s.ASMID == id && s.OrganizationId == orgId);
        }
        public IEnumerable<ASMDTO> GetASMInformations(long orgId)
        {
            return _salesAndDistributionDb.Db.Database.SqlQuery<ASMDTO>(string.Format(@"Select asm.ASMID,asm.FullName,asm.[Address],asm.MobileNo,asm.Email,asm.Remarks,asm.IsActive,asm.IsAllowToLogIn,
asm.EntryDate,asm.EUserId,app.UserName 'EntryUser',asm.DivisionId,dv.DivisionName,asm.DistrictId,dis.DistrictName,ISNULL(asm.UserId,0) 'UserId'
From tblASM asm
Inner Join [ControlPanel].dbo.tblApplicationUsers app on app.UserId = asm.EUserId
Inner Join tblDivision dv on asm.DivisionId =dv.DivisionId
Inner Join tblDistrict dis on asm.DistrictId =dis.DistrictId
Where 1=1 and asm.OrganizationId={0}", orgId)).ToList();
        }
        public bool SaveASM(ASMDTO dto, SRUser sRUser, long userId, long branchId, long orgId)
        {
            ASM asm = null;
            string asmUserId = "0";
            if (dto.ASMID == 0)
            {
                asm = new ASM()
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
                    SaveASMUser(dto, sRUser, userId, branchId, orgId, out asmUserId);
                    asm.UserId = Convert.ToInt64(asmUserId);
                }
                else
                {
                    asm.UserId = 0;
                }
                _aSMRepository.Insert(asm);
            }
            else if (dto.ASMID > 0)
            {
                asm = this.GetASMById(dto.ASMID, orgId);
                if (asm != null)
                {
                    asm.FullName = dto.FullName;
                    asm.Address = dto.Address;
                    asm.Email = dto.Email;
                    asm.MobileNo = dto.MobileNo;
                    asm.IsActive = dto.IsActive;
                    asm.IsAllowToLogIn = dto.IsAllowToLogIn;
                    asm.DistrictId = dto.DistrictId;
                    asm.DivisionId = dto.DivisionId;
                    asm.UpUserId = userId;
                    asm.UpdateDate = DateTime.Now;
                    if (dto.IsAllowToLogIn && asm.UserId == 0)
                    {
                        SaveASMUser(dto, sRUser, userId, branchId, orgId, out asmUserId);
                        asm.UserId = Convert.ToInt64(asmUserId);
                    }
                    _aSMRepository.Update(asm);
                }
            }
            return _aSMRepository.Save();
        }
        private void SaveASMUser(ASMDTO dto, SRUser sRUser, long userId, long branchId, long orgId, out string asmUserId)
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
            _appUserBusiness.SaveSRAppUser(appUser, userId, orgId,"ASM", out asmUserId);
        }
    }
}
