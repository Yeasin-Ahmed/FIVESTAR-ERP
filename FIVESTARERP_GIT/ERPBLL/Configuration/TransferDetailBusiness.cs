using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class TransferDetailBusiness: ITransferDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferDetailRepository transferDetailRepository; // repo
        public TransferDetailBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            transferDetailRepository = new TransferDetailRepository(this._configurationDb);
        }

        public IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId, long orgId, long branchId)
        {
            return transferDetailRepository.GetAll(ts =>ts.TransferInfoId == transferId && ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }

        public IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId, long orgId)
        {
            return transferDetailRepository.GetAll(ts => ts.OrganizationId == orgId && ts.TransferInfoId == transferId).ToList();
        }

        public IEnumerable<TransferDetail> GetAllTransferDetailByOrgId(long orgId, long branchId)
        {
            return transferDetailRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }
    }
}
