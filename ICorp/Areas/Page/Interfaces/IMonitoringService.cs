using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IMonitoringService
    {
        List<MonitoringList> GetList(string userName);
        List<MonitoringDetail> GetDetailList(string year);
        ResponseJson SetNewDueDate(int idUP, DateTime newDate);
    }
}
