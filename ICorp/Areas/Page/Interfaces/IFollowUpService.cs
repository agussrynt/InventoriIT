using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IFollowUpService
    {
        List<FollowUpList> GetList(string userName);
        List<FollowUpDetail> GetDetailList(string year);
        ResponseJson FollowUPDocument(FollowUpUpload followUpUpload);
    }
}
