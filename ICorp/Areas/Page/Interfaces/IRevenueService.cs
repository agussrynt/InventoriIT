using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IRevenueService
    {
        Task<BaseResponseJson> GetAllHeader();
        ResponseJson SaveOrUpdate(HeaderRevenue headerRevenue);
        //ResponseJson DeleteHeader(int headerId);
        Task<BaseResponseJson> GetProjectRevenue(int idHeader);
        Task<BaseResponseJson> GetDetailHeaderRevenue(int idHeader);
        List<ProjectDD> GetProjectExist();
        Task<BaseResponseJson> GetProjectExistById(int ProjectID);
    }
}
