using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IAuditExternalFolowUpService
    {
        Task<List<AuditExternalFollowUpList>> GetList(string username);
        Task<BaseResponseJson> Create(AuditExternalFollowUp data);
    }
}
