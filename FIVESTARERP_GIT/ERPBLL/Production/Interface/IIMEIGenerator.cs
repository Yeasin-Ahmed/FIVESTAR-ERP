using ERPBO.Inventory.DTOModel;
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
        List<string> IMEIGenerate(long modelId, long tac, long serial, int noOfSim, int noOfHandset, long userId, long orgId);

        IMEIListWithSerial IMEIGeneratedList(long modelId, long tac, long serial, int noOfSim, int noOfHandset, long userId, long orgId);
    }
}
