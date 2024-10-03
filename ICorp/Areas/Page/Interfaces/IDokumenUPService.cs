using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IDokumenUPService
    {
        List<DokumenList> GetList(string userName);
        List<DokumenUP> GetDetailList(string userName, string year);
        ResponseJson UploadDocumentUP(DokumenUpload dokumenUpload);
        bool TestUpload(UploadFile uploadFile);
    }
}
