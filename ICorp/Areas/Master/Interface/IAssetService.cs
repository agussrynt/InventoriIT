using PlanCorp.Areas.Master.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Master.Interface
{
    public interface IAssetService
    {
        Task<BaseResponseJson> GetAssetsByID(int assetID);

        List<TipeAssets> GetAssetTypeDropdown();

        List<CostCenterDD> GetCostCenterDropdown();

        ResponseJson DeleteAsset(int assetID);

        ResponseJson SaveOrUpdate(Assets asset);

        Task<BaseResponseJson> GetAllAsset();
    }
}