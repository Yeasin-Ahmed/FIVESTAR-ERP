using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
    public class HandSetIMEI
    {
        public List<string> SetIMEI { get; set; }
    }

    public class IMEIListWithSerial
    {
        public List<HandSetIMEI> HandSetIMEIs { get; set; }
        public long Serial { get; set; }
    }
}
