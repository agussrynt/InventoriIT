using InventoryIT.Areas.Master.Models;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IRevenue_RJPP_Service
    {
        Task<BaseResponseJson> GetRevenueByID(int RevenueID);

        Task<BaseResponseJson> GetAllRevenue();

        Task<BaseResponseJson> GetSumRevHPPGA();

        ResponseJson DeleteExistingData(string idDetail);

        ResponseJson InsertDetail(RevHPPGADetail detail);

        ResponseJson InsertRevenue(Revenue_RJPP revenue);

        ResponseJson InsertHPP(HPP_RJPP hpp);
    }
}