using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IMasterKertasKerjaService
    {
        ResponseJson StoreMasterKertasKerja(StoreModel storeModel);
        //List<MasterKertasKerja> GetClone(int year);
        List<Dropdown> GetDropdown(int paramsId, int option);

        BaseResponseJson GetDataMasterKertasKerja(int year);
    }
}
