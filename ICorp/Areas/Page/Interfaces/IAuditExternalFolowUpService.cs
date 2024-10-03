using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IAuditExternalFolowUpService
    {
        Task<List<AuditExternalFollowUpList>> GetList(string username);
        Task<BaseResponseJson> Create(AuditExternalFollowUp data);
    }
}
