using InventoryIT.Areas.Master.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Master.Interface
{
    public interface IWorkService
    {
        Task<BaseResponseJson> GetWorksByID(int workID);

        ResponseJson DeleteWorks(int workID);

        ResponseJson SaveOrUpdate(Works work);

        Task<BaseResponseJson> GetAllWorks();
    }
}