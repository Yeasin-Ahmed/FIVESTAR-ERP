using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface ISupplierBusiness
    {
        IEnumerable<Supplier> GetSuppliers(long orgId);
        Supplier GetSupplierById(long supplierId,long orgId);
        bool SaveSupplier(SupplierDTO dto, long userId, long orgId);
    }
}
