using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class ColorSSBusiness: IColorSSBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly ColorSSRepository colorSSRepository; // repo
        public ColorSSBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            colorSSRepository = new ColorSSRepository(this._configurationDb);
        }

        public IEnumerable<ColorSS> GetAllColorsByOrgId(long orgId)
        {
            return colorSSRepository.GetAll(c => c.OrganizationId == orgId).ToList();
        }

        public ColorSS GetOneColorsById(long colorId, long orgId)
        {
            return colorSSRepository.GetOneByOrg(c => c.ColorId == colorId && c.OrganizationId == orgId);
        }

        public bool IsDuplicateColor(string color, long orgId)
        {
            throw new NotImplementedException();
        }

        public bool SaveColorSS(ColorSSDTO dto, long orgId, long branchId, long userId)
        {
            ColorSS color = new ColorSS();
            if (dto.ColorId == 0)
            {
                color.ColorName = dto.ColorName;
                color.Remarks = dto.Remarks;
                color.Flag = dto.Flag;
                color.EUserId = userId;
                color.OrganizationId = orgId;
                color.EntryDate = DateTime.Now;
                color.BranchId = branchId;
                colorSSRepository.Insert(color);
            }
            else
            {
                color = GetOneColorsById(dto.ColorId, orgId);
                color.ColorName = dto.ColorName;
                color.Remarks = dto.Remarks;
                color.Flag = dto.Flag;
                color.UpUserId = dto.UpUserId;
                color.UpdateDate = DateTime.Now;
                colorSSRepository.Update(color);
            }
            return colorSSRepository.Save();
        }
    }
}
