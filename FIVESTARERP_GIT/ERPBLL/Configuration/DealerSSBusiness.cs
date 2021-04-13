using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class DealerSSBusiness: IDealerSSBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly DealerSSRepository dealerSSRepository; // repo
        public DealerSSBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            dealerSSRepository = new DealerSSRepository(this._configurationDb);
        }

        public IEnumerable<DealerSS> GetAllDealerByOrgId(long orgId)
        {
            return dealerSSRepository.GetAll(d => d.OrganizationId == orgId);
        }

        public IEnumerable<DealerSSDTO> GetAllDealerForD(long orgId)
        {
            var data = this._configurationDb.Db.Database.SqlQuery<DealerSSDTO>(
                    string.Format(@"Select DealerId,MobileNo,Cast(DealerName as Nvarchar(50))+'-'+Cast(ZoneName as Nvarchar(50))'Dealer' From tblDealerSS Where OrganizationId={0}", orgId)).ToList();
            return data;
        }

        public DealerSS GetDealerById(long dealerId, long orgId)
        {
            return dealerSSRepository.GetOneByOrg(d => d.DealerId == dealerId && d.OrganizationId == orgId);
        }

        public DealerSS GetDealerByMobile(string mobile, long orgId)
        {
            return dealerSSRepository.GetOneByOrg(d => d.MobileNo == mobile && d.OrganizationId == orgId);
        }

        public bool IsDuplicateDealer(string mobile, long id, long orgId)
        {
            return dealerSSRepository.GetOneByOrg(f => f.MobileNo == mobile && f.DealerId != id && f.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveDealer(DealerSSDTO dto, long orgId, long branchId, long userId)
        {
            DealerSS dealer = new DealerSS();
            if (dto.DealerId == 0)
            {
                dealer.DealerName = dto.DealerName;
                dealer.DealerCode = dto.DealerCode;
                dealer.Address = dto.Address;
                dealer.TelephoneNo = dto.TelephoneNo;
                dealer.MobileNo = dto.MobileNo;
                dealer.Email = dto.Email;
                dealer.DivisionName = dto.DivisionName;
                dealer.DistrictName = dto.DistrictName;
                dealer.ZoneName = dto.ZoneName;
                dealer.ContactPersonName = dto.ContactPersonName;
                dealer.ContactPersonMobile = dto.ContactPersonMobile;
                dealer.Remarks = dto.Remarks;
                dealer.Flag = dto.Flag;
                dealer.OrganizationId = orgId;
                dealer.BranchId = branchId;
                dealer.EUserId = userId;
                dealer.EntryDate = DateTime.Now;
                dealerSSRepository.Insert(dealer);
            }
            else
            {
                var dealerId = GetDealerById(dto.DealerId, orgId);
                dealerId.DealerName = dto.DealerName;
                dealerId.DealerCode = dto.DealerCode;
                dealerId.Address = dto.Address;
                dealerId.TelephoneNo = dto.TelephoneNo;
                dealerId.MobileNo = dto.MobileNo;
                dealerId.Email = dto.Email;
                dealerId.DivisionName = dto.DivisionName;
                dealerId.DistrictName = dto.DistrictName;
                dealerId.ZoneName = dto.ZoneName;
                dealerId.ContactPersonName = dto.ContactPersonName;
                dealerId.ContactPersonMobile = dto.ContactPersonMobile;
                dealerId.Remarks = dto.Remarks;
                dealerId.Flag = dto.Flag;
                dealerId.UpUserId = userId;
                dealerId.UpdateDate = DateTime.Now;
                dealerSSRepository.Update(dealerId);
            }
            return dealerSSRepository.Save();
        }
    }
}
