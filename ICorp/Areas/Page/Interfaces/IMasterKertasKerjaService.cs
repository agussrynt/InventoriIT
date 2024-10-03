using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IMasterKertasKerjaService
    {
        ResponseJson StoreMasterKertasKerja(StoreModel storeModel);
        //List<MasterKertasKerja> GetClone(int year);
        List<Dropdown> GetDropdown(int paramsId, int option);

        BaseResponseJson GetDataMasterKertasKerja(int year);
    }
}
