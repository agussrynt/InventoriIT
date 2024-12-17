using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IFUKAssignmentService
    {
        List<FUKAssignment> GetList(int parameterId);
        ResponseJson SetAssignment(FUKAssignment fUKAssignment);

    }
}
