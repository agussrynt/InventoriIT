using InventoryIT.Areas.Master.Models;

namespace InventoryIT.Areas.Master.Interface
{
    public interface IYearService
    {
        List<YearData> GetAll();
    }
}
