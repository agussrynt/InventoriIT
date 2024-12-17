using InventoryIT.Areas.Master.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Master.Interface
{
    public interface IUserService
    {
        List<UserDetail> GetUsers(string userId);
        UserDetail FindUser(string Email);
        List<PIC> GetPICDropdown(int paramsId);
        void SaveOrUpdate(UserDetail user);
        void Delete(string userId);

        bool IsActiveUser(string username);
        bool ActivateUser(string username, bool status);
        Task<BaseResponseJson> Gets();
    }
}
