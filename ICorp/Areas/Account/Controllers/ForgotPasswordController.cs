using InventoryIT.Areas.Account.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace InventoryIT.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("account/forgot-password")]
    public class ForgotPasswordController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public ForgotPasswordController(
            UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/confirmation")]
        public IActionResult Confirmation()
        {
            return View();
        }

        [Route("on-post-ajax")]
        [HttpPost]
        public async Task<IActionResult> onPostAjax([Bind("Email")] ForgotPassword Input)
        {
            var user = await this.userManager.FindByEmailAsync(Input.Email);
            if (user == null || !(await this.userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/account/reset-password",
                pageHandler: null,
                values: new { area = "Account", code },
                protocol: Request.Scheme);

            //await this.emailSender.SendEmailAsync(
            //    Input.Email,
            //    "Reset Password",
            //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToAction("Confirmation");
        }
    }
}
