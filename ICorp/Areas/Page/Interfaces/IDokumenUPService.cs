using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;

namespace InventoryIT.Areas.Page.Interfaces
{
    public interface IDokumenUPService
    {
        List<DokumenList> GetList(string userName);
        List<DokumenUP> GetDetailList(string userName, string year);
        ResponseJson UploadDocumentUP(DokumenUpload dokumenUpload);
        bool TestUpload(UploadFile uploadFile);
    }
}
