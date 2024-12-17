using InventoryIT.Models;

namespace InventoryIT.Areas.Master.Interface
{
    public interface IMailService
    {
        Task<MailResultModel> SendMail(MailModel mail);
        List<MailAccModel> GetEmailAcc(string paramsId);
    }
}
