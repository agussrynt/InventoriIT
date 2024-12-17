using InventoryIT.Areas.Master.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Master.Interface
{
    public interface IProjectService
    {
        Task<BaseResponseJson> GetProjectsByID(int ProjectID);

        List<SegmenDD> GetSegmenProject();

        List<AssetDD> GetAssetProject();

        List<CustomerDD> GetCustomerProject();

        List<PekerjaanDD> GetPekerjaanProject();

        List<TipeContractsDD> GetContractTypeProject();

        List<SBTDD> GetSBTProject();

        ResponseJson DeleteProject(int ProjectID);

        ResponseJson SaveOrUpdate(Projects Project);

        Task<BaseResponseJson> GetAllProject();
    }
}