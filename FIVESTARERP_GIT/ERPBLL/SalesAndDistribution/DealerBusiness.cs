using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.Common;
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

        public DealerBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._dealerRepository = new DealerRepository(this._salesAndDistributionDb);
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

        public bool SaveDealer(DealerDTO dealerDto, long userId, long orgId)
        {
            
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
                    _dealerRepository.Update(dealerInDb);
                }
            }
            return _dealerRepository.Save();
        }
    }
}
