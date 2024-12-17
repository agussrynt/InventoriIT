using InventoryIT.Areas.Master.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
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