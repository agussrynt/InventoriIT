using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
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
