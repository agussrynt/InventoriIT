using InventoryIT.Areas.Master.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Master.Interface
{
    public interface IRoleService
    {
        ResponseJson SaveOrUpdate(RoleParam role);
        ResponseJson DeleteRole(string id);
    }
}
