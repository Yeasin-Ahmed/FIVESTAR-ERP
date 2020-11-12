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
    public class ModelBusiness : IModelBusiness
    {
        // Db
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        // Business
        private readonly IModelColorBusiness _modelColorBusiness;
        //repo
        private readonly ModelRepository _modelRepository;

        public ModelBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution, IModelColorBusiness modelColorBusiness)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._modelRepository = new ModelRepository(_salesAndDistribution);
            this._modelColorBusiness = modelColorBusiness;
        }

        public Description GetModelById(long id, long orgId)
        {
            return this._modelRepository.GetOneByOrg(s => s.DescriptionId == id && s.OrganizationId == orgId);
        }

        public IEnumerable<Description> GetModels(long orgId)
        {
            return this._modelRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public List<ModelColor> GetModelColors(long modelId, long orgId)
        {
            List<ModelColor> colors = _salesAndDistribution.Db.Database.SqlQuery<ModelColor>(string.Format(@"Select C.ColorId,C.ColorName From tblModelColors mc 
Inner Join tblColors c on mc.ColorId = c.ColorId
Where mc.DescriptionId = {0} and mc.OrganizationId={1}", modelId, orgId)).ToList();
            return colors;
        }

        public bool SaveModel(DescriptionDTO dto, long userId, long orgId)
        {
            bool IsSuccess = false;
            Description model = null;
            if (dto.DescriptionId == 0)
            {
                model = new Description()
                {
                    DescriptionName = dto.DescriptionName,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    SalePrice = dto.SalePrice,
                    CostPrice = dto.CostPrice,
                    Flag = "External"
                };
                _modelRepository.Insert(model);
            }
            else if (dto.DescriptionId > 0)
            {
                model = this.GetModelById(dto.DescriptionId, orgId);
                if (model != null)
                {
                    model.DescriptionName = dto.DescriptionName;
                    model.IsActive = dto.IsActive;
                    model.Remarks = dto.Remarks;
                    model.UpdateDate = DateTime.Now;
                    model.UpUserId = dto.UpUserId;
                    model.CategoryId = dto.CategoryId;
                    model.BrandId = dto.BrandId;
                    model.Remarks = dto.Remarks;
                    model.SalePrice = dto.SalePrice;
                    model.CostPrice = dto.CostPrice;
                }
                _modelRepository.Update(model);
            }
            if (_modelRepository.Save())
            {
                if (dto.Colors.Count > 0)
                {
                    IsSuccess = _modelColorBusiness.SaveModelColors(model.DescriptionId, dto.Colors, userId, orgId);
                }
                else
                {
                    IsSuccess = true;
                }
            }
            return IsSuccess;
        }
    }
}
