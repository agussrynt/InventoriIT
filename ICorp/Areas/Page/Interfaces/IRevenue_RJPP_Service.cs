using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IRevenue_RJPP_Service
    {
        Task<BaseResponseJson> GetRevenueByID(int RevenueID);
        Task<BaseResponseJson> GetAllRevenue();
    }
}
