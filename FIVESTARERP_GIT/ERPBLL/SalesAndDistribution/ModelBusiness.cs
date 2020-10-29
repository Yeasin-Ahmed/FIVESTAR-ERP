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

        public Model GetModelById(long id, long orgId)
        {
            return this._modelRepository.GetOneByOrg(s => s.ModelId == id && s.OrganizationId == orgId);
        }

        public IEnumerable<Model> GetModels(long orgId)
        {
            return this._modelRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public bool SaveModel(ModelDTO dto, long[] colors, long userId, long orgId)
        {
            bool IsSuccess = false;
            Model model = null;
            if(dto.ModelId == 0)
            {
                model = new Model()
                {
                    ModelName =dto.ModelName,
                    IsActive =dto.IsActive,
                    Remarks=dto.Remarks,
                    OrganizationId = dto.OrganizationId,
                    EUserId = userId,
                    EntryDate = DateTime.Now
                };
                _modelRepository.Insert(model);
            }
            else if(dto.ModelId > 0)
            {
                model = this.GetModelById(dto.ModelId, orgId);
                if(model != null)
                {
                    model.ModelName = dto.ModelName;
                    model.IsActive = dto.IsActive;
                    model.Remarks = dto.Remarks;
                    model.UpdateDate = DateTime.Now;
                    model.UpUserId = dto.UpUserId;
                }
                _modelRepository.Update(model);
            }
            if (_modelRepository.Save())
            {
                if(colors != null && colors.Length > 0)
                {
                    IsSuccess=_modelColorBusiness.SaveModelColors(model.ModelId, colors, userId, orgId);
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
