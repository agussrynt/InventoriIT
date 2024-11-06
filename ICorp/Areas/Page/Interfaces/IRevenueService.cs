using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IRevenueService
    {
        Task<BaseResponseJson> GetAllHeader();
    }
}
