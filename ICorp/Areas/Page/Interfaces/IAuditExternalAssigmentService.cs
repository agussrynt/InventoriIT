using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IAuditExternalAssigmentService
    {
        Task<List<AuditExternalAssigmentRecomendationList>> GetList(int year);
        Task<BaseResponseJson> Create(AuditExternalAssigmentRecomendation data);

    }
}
