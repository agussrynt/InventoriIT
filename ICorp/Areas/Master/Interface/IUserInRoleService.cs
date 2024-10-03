using PlanCorp.Areas.Master.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Master.Interface
{
    public interface IUserInRoleService
    {
        Task<List<UserInRole>> GetAllUserInRoles();
        int GetUserFPPByEmpNumber(string empNumber);
        UserInRole GetById(string id);
        void Save(UserInRole role);
        void Update(UserInRole role);
        string Delete(string id);
        Task<BaseResponseJson> RemoveAllRole(string userid);
        Task<BaseResponseJson> SetUserRole(string userid, string role);
    }
}
