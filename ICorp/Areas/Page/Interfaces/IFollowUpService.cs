using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IFollowUpService
    {
        List<FollowUpList> GetList(string userName);
        List<FollowUpDetail> GetDetailList(string year);
        ResponseJson FollowUPDocument(FollowUpUpload followUpUpload);
    }
}
