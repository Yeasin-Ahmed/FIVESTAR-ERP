using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;

namespace ERPBLL.SalesAndDistribution
{
    public class ColorBusiness : IColorBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly ColorRepository _colorRepository;
        public ColorBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._colorRepository = new ColorRepository(this._salesAndDistribution);
        }
        public Color GetColorById(long colorId, long orgId)
        {
            return this._colorRepository.GetOneByOrg(s => s.ColorId == colorId && s.OrganizationId == orgId);
        }
        public IEnumerable<Color> GetColors(long orgId)
        {
            return this._colorRepository.GetAll(s =>s.OrganizationId == orgId);
        }
        public bool SaveColor(ColorDTO dto, long userId, long orgId)
        {
            if(dto.ColorId == 0)
            {
                Color color = new Color()
                {
                    ColorName = dto.ColorName,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId
                };
                _colorRepository.Insert(color);
            }
            else if(dto.ColorId > 0){
                var colorInDb = _colorRepository.GetOneByOrg(s => s.ColorId == dto.ColorId && s.OrganizationId == orgId);
                if(colorInDb != null)
                {
                    colorInDb.ColorName = dto.ColorName;
                    colorInDb.IsActive = dto.IsActive;
                    colorInDb.Remarks = dto.Remarks;
                    colorInDb.UpUserId = userId;
                    colorInDb.UpdateDate = DateTime.Now;
                    _colorRepository.Update(colorInDb);
                }
            }
            return _colorRepository.Save();
        }
    }
}
