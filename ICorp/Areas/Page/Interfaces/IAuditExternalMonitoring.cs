using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IAuditExternalMonitoring
    {
        Task<List<AuditExternalFollowUpList>> GetList();
        Task<BaseResponseJson> Create(AuditExternalFollowUp data);
    }
}
