using FakeAPI_PDSI.Model;
using System.Net;
using System.Net.Mail;

namespace FakeAPI_PDSI.Services
{
    public interface IMailService
    {
        void SendEmail(MailModel data);
    }
    public class MailService : IMailService
    {

        public void SendEmail(MailModel data)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("omegaTest001@outlook.com"); //omegaTest001@outlook.com
            string[] arrEmail = data.recipient.Split(',');
            foreach (string email in arrEmail)
            {
                message.To.Add(new MailAddress(email.Trim()));
            }
            message.Subject = data.subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = data.body;
            smtp.Port = 587;
            smtp.Host = "smtp.office365.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("omegaTest001@outlook.com", "createpassword");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}
