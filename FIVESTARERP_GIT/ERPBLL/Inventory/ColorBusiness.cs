using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class ColorBusiness : IColorBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly ColorRepository colorRepository;

        public ColorBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            colorRepository = new ColorRepository(this._inventoryDb);
        }
        public IEnumerable<Color> GetAllColorByOrgId(long orgId)
        {
            return colorRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public bool IsDuplicateColorName(string colorName, long id, long orgId)
        {
            return colorRepository.GetOneByOrg(s => s.ColorName == colorName && s.ColorId != id && s.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveColor(ColorDTO dto, long userId, long orgId)
        {
            Color color = new Color();
            if (dto.ColorId == 0)
            {
                color.ColorName = dto.ColorName;
                color.EntryDate = DateTime.Now;
                color.EUserId = userId;
                color.IsActive = dto.IsActive;
                color.OrganizationId = orgId;
                color.Remarks = dto.Remarks;
                colorRepository.Insert(color);
            }
            else
            {
                color = GetColorOneByOrgId(dto.ColorId, orgId);
                //color.ColorName = dto.ColorName;
                color.IsActive = dto.IsActive;
                color.OrganizationId = orgId;
                color.Remarks = dto.Remarks;
                color.UpdateDate = DateTime.Now;
                color.UpUserId = userId;
                colorRepository.Update(color);
            }
            return colorRepository.Save();
        }

        public Color GetColorOneByOrgId(long id, long orgId)
        {
            return colorRepository.GetOneByOrg(s => s.ColorId == id && s.OrganizationId == orgId);
        }
    }
}
