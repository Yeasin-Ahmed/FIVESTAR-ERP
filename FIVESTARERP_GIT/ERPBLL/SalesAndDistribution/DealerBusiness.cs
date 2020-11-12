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
    public class DealerBusiness : IDealerBusiness
    {
        // Db
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        // Repo
        private readonly DealerRepository _dealerRepository;
        private readonly IAppUserBusiness _appUserBusiness;

        public DealerBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb, IAppUserBusiness appUserBusiness)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._dealerRepository = new DealerRepository(this._salesAndDistributionDb);
            this._appUserBusiness = appUserBusiness;
        }

        public Dealer GetDealerById(long id, long orgId)
        {
            return _dealerRepository.GetOneByOrg(s => s.DealerId == id && s.OrganizationId == orgId);
        }

        public IEnumerable<DealerDTO> GetDealerInformations(long orgId)
        {
            return this._salesAndDistributionDb.Db.Database.SqlQuery<DealerDTO>(string.Format(@"Exec spDealerInformation {0}",orgId)).ToList();
        }

        public IEnumerable<Dropdown> GetDealerRepresentatives(long orgId)
        {
            return _salesAndDistributionDb.Db.Database.SqlQuery<Dropdown>("Exec spDealerRepresentatives {0}",orgId).ToList();
        }

        public IEnumerable<Dealer> GetDealers(long orgId)
        {
            return _dealerRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public bool SaveDealer(DealerDTO dealerDto, SRUser user, long userId, long branchId, long orgId)
        {
            string srUserId = "0";
            if (dealerDto.DealerId == 0)
            {
                Dealer dealer = new Dealer
                {
                    DealerName = dealerDto.DealerName,
                    Address = dealerDto.Address,
                    TelephoneNo = dealerDto.TelephoneNo,
                    MobileNo = dealerDto.MobileNo,
                    Email = dealerDto.Email,
                    ContactPersonName = dealerDto.ContactPersonName,
                    ContactPersonMobile = dealerDto.ContactPersonMobile,
                    Remarks = dealerDto.Remarks,
                    IsActive = dealerDto.IsActive,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    RepresentativeId= dealerDto.RepresentativeId,
                    RepresentativeUserId= dealerDto.RepresentativeUserId,
                    RepresentativeFlag=dealerDto.RepresentativeFlag,
                    ZoneId= dealerDto.ZoneId,
                    DistrictId= dealerDto.DistrictId,
                    DivisionId = dealerDto.DivisionId
                };
                if(dealerDto.IsAllowToLogIn)
                {
                    SaveSRUser(dealerDto, user, userId, branchId, orgId, out srUserId);
                    dealerDto.UserId = Convert.ToInt64(srUserId);
                }
                else
                {
                    dealerDto.UserId = 0;
                }
                _dealerRepository.Insert(dealer);
            }
            else
            {
                var dealerInDb = this.GetDealerById(dealerDto.DealerId, orgId);
                if(dealerInDb != null)
                {
                    dealerInDb.DealerName = dealerDto.DealerName;
                    dealerInDb.Address = dealerDto.Address;
                    dealerInDb.TelephoneNo = dealerDto.TelephoneNo;
                    dealerInDb.MobileNo = dealerDto.MobileNo;
                    dealerInDb.Email = dealerDto.Email;
                    dealerInDb.ContactPersonName = dealerDto.ContactPersonName;
                    dealerInDb.ContactPersonMobile = dealerDto.ContactPersonMobile;
                    dealerInDb.Remarks = dealerDto.Remarks;
                    dealerInDb.IsActive = dealerDto.IsActive;
                    dealerInDb.UpUserId= userId;
                    dealerInDb.UpdateDate= DateTime.Now;
                    dealerInDb.RepresentativeId = dealerDto.RepresentativeId;
                    dealerInDb.RepresentativeUserId = dealerDto.RepresentativeUserId;
                    dealerInDb.RepresentativeFlag = dealerDto.RepresentativeFlag;
                    dealerInDb.ZoneId = dealerDto.ZoneId;
                    dealerInDb.DistrictId = dealerDto.DistrictId;
                    dealerInDb.DivisionId = dealerDto.DivisionId;
                    dealerInDb.IsAllowToLogIn = dealerDto.IsAllowToLogIn;
                    if (dealerDto.IsAllowToLogIn && dealerInDb.UserId == 0)
                    {
                        SaveSRUser(dealerDto, user, userId, branchId, orgId, out srUserId);
                        dealerInDb.UserId = Convert.ToInt64(srUserId);
                    }
                    _dealerRepository.Update(dealerInDb);
                }
            }
            return _dealerRepository.Save();
        }

        private void SaveSRUser(DealerDTO dto, SRUser sRUser, long userId, long branchId, long orgId, out string srUserId)
        {
            AppUserDTO appUser = new AppUserDTO()
            {
                FullName = dto.DealerName,
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
            _appUserBusiness.SaveSRAppUser(appUser, userId, orgId, "Dealer", out srUserId);
        }
    }
}
