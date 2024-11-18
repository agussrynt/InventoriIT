using PlanCorp.Areas.Master.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IRevHPPGAService
    {
        Task<BaseResponseJson> GetAssetsByID(int assetID);

        List<TipeAssets> GetAssetTypeDropdown();

        List<CostCenterDD> GetCostCenterDropdown();

        ResponseJson DeleteAsset(int assetID);

        ResponseJson SaveOrUpdate(Assets asset);

        Task<BaseResponseJson> GetAllAsset();
    }
}