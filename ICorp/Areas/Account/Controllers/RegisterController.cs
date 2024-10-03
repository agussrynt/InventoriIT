using PlanCorp.Areas.Account.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace PlanCorp.Areas.Account.Controllers
{
    [AllowAnonymous]
    [Area("Account")]
    [Route("account/register")]
    public class RegisterController : Controller
    {
        private readonly PlanCorpDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IUserEmailStore<IdentityUser> emailStore;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterController(
            PlanCorpDbContext context,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.userStore = userStore;
            this.emailStore = GetEmailStore();
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [Route("")]
        public IActionResult Index(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            ViewBag.ReturnUrl = returnUrl;
            if (this.signInManager.IsSignedIn(User))
            {
                return LocalRedirect(String.IsNullOrEmpty(returnUrl) ? Url.Content("~/") : returnUrl);
            }

            return View();
        }

        [HttpPost]
        [Route("on-post-ajax")]
        public async Task<IActionResult> Store([Bind("FirstName", "LastName", "UserName", "Email", "Password", "ConfirmPassword")] Register register)
        {
            try
            {
                var setRole = "Admin";
                if (this.roleManager.Roles.Count() < 1)
                {
                    //setRole = "SuperAdmin";
                    List<string> roles = new List<string>()
                {
                    "Admin",
                    "Audity",
                    "Auditor"
                };
                    for (int i = 0; i < 4; i++)
                    {
                        await this.roleManager.CreateAsync(new IdentityRole(roles[i].Trim()));
                    }
                }

                string[] split = register.Email.Split('@');
                //var user = CreateUser();
                var user = new IdentityUser()
                {
                    UserName = split[0],
                    //NormalizedUserName = register.FirstName + ' ' + register.LastName,
                    NormalizedUserName = split[0],
                    Email = register.Email
                };

                await this.userStore.SetUserNameAsync(user, split[0], CancellationToken.None);
                await this.emailStore.SetEmailAsync(user, register.Email, CancellationToken.None);
                var result = await this.userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    // _logger.LogInformation("User created a new account with password."); pertamina@2022
                    var createdAt = DateTime.Now;
                    var dataProfile = new Profile
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        UserType = UserType.SITE,
                        UserName = user.UserName,
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        CreatedAt = createdAt,
                        CreatedBy = "System",
                        UpdatedAt = createdAt,
                        UpdatedBy = "System"
                    };
                    await this.context.Profiles.AddAsync(dataProfile);

                    var userId = await this.userManager.GetUserIdAsync(user);
                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/account/confirm-email",
                        pageHandler: null,
                        //values: new { area = "Account", controller = "ConfirmEmail", userId = userId, code = code, returnUrl = returnUrl },
                        values: new { area = "Account", userId = userId, code = code, returnUrl = string.IsNullOrEmpty(ViewBag.ReturnUrl) ? Url.Content("~/") : ViewBag.ReturnUrl },
                        protocol: Request.Scheme);

                    //await this.emailSender.SendEmailAsync(register.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return Json(new
                        {
                            Success = true,
                            Option = "RequireConfirmedAccount",
                            Message = new
                            {
                                email = register.Email,
                                returnUrl = string.IsNullOrEmpty(ViewBag.ReturnUrl) ? Url.Content("~/") : ViewBag.ReturnUrl
                            },
                            UrlResponse = "account/register/confirmation"
                        });
                    }
                    else
                    {
                        try
                        {
                            await this.userManager.AddToRoleAsync(user, setRole);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return Json(new
                        {
                            Success = true,
                            Option = "",
                            Message = "Entering page!",
                            UrlResponse = string.IsNullOrEmpty(ViewBag.ReturnUrl) ? Url.Content("~/") : ViewBag.ReturnUrl
                        });
                    }
                }

                // If we got this far, something failed, redisplay form
                throw new Exception("Something wrong when register!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);

                // If we got this far, something failed, redisplay form
                return Json(new
                {
                    Success = false,
                    Option = "",
                    Message = ex.Message,
                    UrlResponse = string.IsNullOrEmpty(ViewBag.ReturnUrl) ? Url.Content("~/") : ViewBag.ReturnUrl
                });
            }
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)this.userStore;
        }
    }
}
