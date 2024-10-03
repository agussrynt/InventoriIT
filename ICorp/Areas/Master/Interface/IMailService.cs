using PlanCorp.Models;

namespace PlanCorp.Areas.Master.Interface
{
    public interface IMailService
    {
        Task<MailResultModel> SendMail(MailModel mail);
        List<MailAccModel> GetEmailAcc(string paramsId);
    }
}
