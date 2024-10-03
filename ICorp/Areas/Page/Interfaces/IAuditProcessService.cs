using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IAuditProcessService
    {
        List<Audit> GetList();
        List<AuditReview> GetListReview();
        bool AuthorResponseBackup(int UpId, int responseType, string Remarks, float Score, string Username);
        ResponseJson UpdateScore(int UpId, float Score, string Username);
        ResponseJson AuthorResponse(int UpId, int Status, string Remarks, float Score, string Recommendation, int IsRecommendation, string Username);
        List<AuditDetail> GetDetailList(string year);
        ResponseJson IsReadyComplete(int year);
        ResponseJson SetFinishAudit(int year);

    }
}
