using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IMonitoringService
    {
        List<MonitoringList> GetList(string userName);
        List<MonitoringDetail> GetDetailList(string year);
        ResponseJson SetNewDueDate(int idUP, DateTime newDate);
    }
}
