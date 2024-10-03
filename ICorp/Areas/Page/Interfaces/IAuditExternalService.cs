using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IAuditExternalService
    {
        Task<List<AuditExternalList>> GetList();
        Task<ResponseJson> Create(AuditExternal data);
        Task<BaseResponseJson> Get(string id);
        Task<BaseResponseJson> GetListScore(string id);
        Task<BaseResponseJson> GetListRecomendation(string id);
        Task<List<AuditExternal>> Find(string id);
    }
}
