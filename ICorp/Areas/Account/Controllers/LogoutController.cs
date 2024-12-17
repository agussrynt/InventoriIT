using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace InventoryIT.Areas.Account.Controllers
{
    [AllowAnonymous]
    [Area("Account")]
    public class LogoutController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LogoutController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("account/logout")]
        public async Task<JsonResult> doLogout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.Clear();
                return Json(new
                {
                    Success = true,
                    Url = Url.Content("~/ ")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Response = ex.Message
                });
            }

        }


        [HttpGet]
        [Route("account/accesdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
