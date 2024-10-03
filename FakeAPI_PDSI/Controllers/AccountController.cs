using FakeAPI_PDSI.Model;
using FakeAPI_PDSI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI_PDSI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService, ILogger<AccountController> logger)
        {
            _logger = logger;
            _identityService = identityService;
        }


        [HttpPost]
        [Route("DoAction")]
        public async Task<TokenResultModel> Login([FromForm]LoginModel login)
        {
            TokenResultModel result = new TokenResultModel();
            ResponseModel res = await _identityService.Login(login);
            if (res.IsSuccess)
            {
                result.Token = res.Data;
                result.Status = "S";
                result.Message = "Login Success";
            }

            return result;
        }

        [HttpPost]
        [Route("test")]
        [Authorize]
        public async Task<TokenResultModel> Test()
        {
            TokenResultModel result = new TokenResultModel();
            result.Status = "S";
            result.Message = "Login Success";

            return result;
        }

    }
}
