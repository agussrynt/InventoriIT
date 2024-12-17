using InventoryIT.Areas.Master.Models;

namespace InventoryIT.Areas.Master.Interface
{
    public interface IFungsiService
    {
        List<Fungsi> GetAll();

        List<Fungsi> Gets();
        Fungsi Get(string id);
    }
}
