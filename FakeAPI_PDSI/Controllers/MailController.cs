using FakeAPI_PDSI.Model;
using FakeAPI_PDSI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace FakeAPI_PDSI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class MailController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMailService _service;
        public MailController(ILogger<AccountController> logger, IMailService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("send")]
        [AllowAnonymous]
        public async Task<MailResultModel> Send([FromForm]MailModel data)
        {
            MailResultModel result = new MailResultModel();
            try
            {
                _service.SendEmail(data);
                result.Status = "S";
                result.Message = "Send Mail Success";
            }
            catch (Exception ex)
            {
                result.Status = "F";
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
