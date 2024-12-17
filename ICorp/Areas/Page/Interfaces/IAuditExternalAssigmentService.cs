using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IAuditExternalAssigmentService
    {
        Task<List<AuditExternalAssigmentRecomendationList>> GetList(int year);
        Task<BaseResponseJson> Create(AuditExternalAssigmentRecomendation data);

    }
}
