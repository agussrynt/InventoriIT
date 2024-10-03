using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IAuditExternalMonitoring
    {
        Task<List<AuditExternalFollowUpList>> GetList();
        Task<BaseResponseJson> Create(AuditExternalFollowUp data);
    }
}
