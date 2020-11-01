using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DTOModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class ModelColorBusiness : IModelColorBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly ModelColorsRepository _modelColorsRepository;

        public ModelColorBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            this._modelColorsRepository = new ModelColorsRepository(this._inventoryDb);
        }
        public ModelColors GetModelColors(long modelId, long colorId, long orgId)
        {
            return _modelColorsRepository.GetOneByOrg(s => s.DescriptionId == modelId && s.ColorId == colorId && s.OrganizationId == orgId);
        }
        public IEnumerable<ModelColors> GetModelColors(long orgId)
        {
            return _modelColorsRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public IEnumerable<ModelColorDTO> GetModelColorsByModel(long modelId, long orgId)
        {
            var query = string.Format(@"Select c.ColorId,c.ColorName From [Inventory].dbo.tblColors c
Inner Join [Inventory].dbo.tblModelColors mc on c.ColorId = mc.ColorId and mc.DescriptionId={0}
Where c.OrgnarizationId = {1}", modelId, orgId);
            return _inventoryDb.Db.Database.SqlQuery<ModelColorDTO>(query).ToList();
        }
        public IEnumerable<ModelColors> GetModelColorsByOrg(long orgId)
        {
            return _modelColorsRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public bool SaveModelColors(long modelId, List<long> colors, long userId, long orgId, out List<long> insertedColor)
        {
            bool IsSuccess = false;
            insertedColor = new List<long>();
            List<ModelColors> modelColorslist = new List<ModelColors>();
            foreach (var color in colors)
            {
                var modelColorInDb = this.GetModelColors(modelId, color, orgId);
                if (modelColorInDb == null)
                {
                    insertedColor.Add(color);
                    ModelColors modelColors = new ModelColors()
                    {
                        DescriptionId = modelId,
                        ColorId = color,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        OrganizationId = orgId
                    };
                    modelColorslist.Add(modelColors);
                }
            }
            if(modelColorslist.Count > 0)
            {
                _modelColorsRepository.InsertAll(modelColorslist);
                IsSuccess = _modelColorsRepository.Save();
            }
            else
            {
                IsSuccess = true;
            }
            return IsSuccess;
        }
    }
}
