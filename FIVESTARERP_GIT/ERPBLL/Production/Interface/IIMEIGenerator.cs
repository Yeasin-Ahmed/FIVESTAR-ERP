using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IIMEIGenerator
    {
        List<string> IMEIGenerateByQRCode(string qrCode, int noOfSim,long userId,long OrgId);
    }
}
