using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class IMEIWriteViewModel
    {
        public int codeId { get; set; }
        [StringLength(100),Required]
        public string qrCode { get; set; }
        [StringLength(100), Required]
        public string barCode { get; set; }
        [Range(1,long.MaxValue)]
        public long floorId { get; set; }
        [Range(1, long.MaxValue)]
        public long packagingLineId { get; set; }
        [StringLength(100), Required]
        public string packagingLineName { get; set; }
    }
}
