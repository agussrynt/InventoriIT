using PlanCorp.Areas.Master.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Master.Interface
{
    public interface IRoleService
    {
        ResponseJson SaveOrUpdate(RoleParam role);
        ResponseJson DeleteRole(string id);
    }
}
