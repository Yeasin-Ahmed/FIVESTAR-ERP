using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class SetIMEIDataForHTML
    {
        public string ModelName { get; set; }
        public List<IMEIImage> IMEIImages { get; set; }
    }

    public class IMEIImage
    {
        public string Text { get; set; }
        public string Image { get; set; }
    }

}
