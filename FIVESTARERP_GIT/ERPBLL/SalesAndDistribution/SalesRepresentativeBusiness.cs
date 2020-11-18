using ERPBLL.Common;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.Common;
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
    public class SalesRepresentativeBusiness : ISalesRepresentativeBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly IAppUserBusiness _appUserBusiness;
        private readonly SalesRepresentativeRepository _salesRepresentativeRepository;

        public SalesRepresentativeBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution, IAppUserBusiness appUserBusiness)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._appUserBusiness = appUserBusiness;
            this._salesRepresentativeRepository = new SalesRepresentativeRepository(this._salesAndDistribution);

        }

        public IEnumerable<Dropdown> GetReportingSR(long orgId, long districtId, long zoneId, string srtype)
        {
            return _salesAndDistribution.Db.Database.SqlQuery<Dropdown>(string.Format("Exec spGetReportingSR {0},'{1}',{2},{3}", orgId, srtype, districtId, zoneId)).ToList();
        }

        public SalesRepresentative GetSalesRepresentativeById(long id, long orgId)
        {
            return _salesRepresentativeRepository.GetOneByOrg(s => s.SRID == id && s.OrganizationId == orgId);
        }

        public IEnumerable<SalesRepresentativeDTO> GetSalesRepresentatives(long orgId)
        {
            return _salesAndDistribution.Db.Database.SqlQuery<SalesRepresentativeDTO>(string.Format(@"Exec spSalesRepresentativeInformation {0}", orgId)).ToList();
        }

        public IEnumerable<Dropdown> GetSalesRepresentativesBySeniorId(long userId, long orgId)
        {
            string query = string.Format(@"Declare @srId bigint=0
Select @srId=ISNULL(SRID,0) From tblSalesRepresentatives Where UserId={0} and OrganizationId={1}
Select (Cast(SRID as Nvarchar(20))+'#'+Cast(ISNULL(UserId,0) as Nvarchar(20))) 'value',FullName 'text' From tblSalesRepresentatives Where ReportingSRId=@srId and OrganizationId={1}", userId,orgId);
            return _salesAndDistribution.Db.Database.SqlQuery<Dropdown>(query).ToList();
        }

        public IEnumerable<SalesRepresentative> GetSalesRepresentativesByType(string srType, long orgId)
        {
            return _salesRepresentativeRepository.GetAll(s => s.SRType == srType && s.OrganizationId == orgId);
        }

        public bool SaveSalesRepresentative(SalesRepresentativeDTO dto, SRUser sRUser, long userId, long branchId, long orgId)
        {
            SalesRepresentative sr = null;
            string srUserId = "0";
            if (dto.SRID == 0)
            {
                sr = new SalesRepresentative()
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
                    ZoneId = dto.ZoneId,
                    Remarks = dto.Remarks,
                    SRType = dto.SRType,
                    ReportingSRId = dto.ReportingSRId
                };
                if (dto.IsAllowToLogIn)
                {
                    SaveSRUser(dto, sRUser, userId, branchId, orgId, out srUserId);
                    sr.UserId = Convert.ToInt64(srUserId);
                }
                else
                {
                    sr.UserId = 0;
                }
                _salesRepresentativeRepository.Insert(sr);
            }
            else if (dto.SRID > 0)
            {
                sr = this.GetSalesRepresentativeById(dto.SRID, orgId);
                if (sr != null)
                {
                    sr.FullName = dto.FullName;
                    sr.Address = dto.Address;
                    sr.Email = dto.Email;
                    sr.MobileNo = dto.MobileNo;
                    sr.IsActive = dto.IsActive;
                    sr.IsAllowToLogIn = dto.IsAllowToLogIn;
                    sr.ZoneId = dto.ZoneId;
                    sr.DistrictId = dto.DistrictId;
                    sr.DivisionId = dto.DivisionId;
                    sr.UpUserId = userId;
                    sr.UpdateDate = DateTime.Now;
                    sr.Remarks = dto.Remarks;
                    sr.ReportingSRId = dto.ReportingSRId;
                    if (dto.IsAllowToLogIn && sr.UserId == 0)
                    {
                        SaveSRUser(dto, sRUser, userId, branchId, orgId, out srUserId);
                        sr.UserId = Convert.ToInt64(srUserId);
                    }
                    _salesRepresentativeRepository.Update(sr);
                }
            }
            return _salesRepresentativeRepository.Save();
        }

        private void SaveSRUser(SalesRepresentativeDTO dto, SRUser sRUser, long userId, long branchId, long orgId, out string srUserId)
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
            _appUserBusiness.SaveSRAppUser(appUser, userId, orgId, dto.SRType, out srUserId);
        }
    }
}
