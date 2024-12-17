using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
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
        Task<BaseResponseJson> GetCostCenterFill(int AssetID);
        Task<BaseResponseJson> GetRegionalFill(int CustomerID);
        ResponseJson SaveMappingProject(MappingProjectRevenue mappingProjectRevenue);
        Task<BaseResponseJson> GetDetailRevenue(int idHeader);
        //ResponseJson SaveAmountRevenue(AmountRevenue amountRevenue);
        ResponseJson DeleteHeaderRevenue(int idHeader);
        ResponseJson DeleteProjectMapping(int idHeader, int idProject);
    }
}
