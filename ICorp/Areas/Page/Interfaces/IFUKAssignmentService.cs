using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IFUKAssignmentService
    {
        List<FUKAssignment> GetList(int parameterId);
        ResponseJson SetAssignment(FUKAssignment fUKAssignment);

    }
}
