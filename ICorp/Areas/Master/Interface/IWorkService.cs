using PlanCorp.Areas.Master.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Master.Interface
{
    public interface IWorkService
    {
        Task<BaseResponseJson> GetWorksByID(int workID);

        ResponseJson DeleteWorks(int workID);

        ResponseJson SaveOrUpdate(Works work);

        Task<BaseResponseJson> GetAllWorks();
    }
}