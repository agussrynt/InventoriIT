using InventoryIT.Areas.Account.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InventoryIT.Areas.Master.Interface;
using Microsoft.Extensions.Options;
using InventoryIT.Models;
using Newtonsoft.Json;
using System.Text;
using InventoryIT.Helpers;
using System.Net.Http.Headers;
using InventoryIT.Areas.Master.Controllers;

namespace InventoryIT.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("account/login")]
    public class LoginController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private readonly IUserService _userService;
        private readonly IOptions<Setting> setting;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMailService _mail;
        private string _uriLDAP { get; set; }
        private string _token { get; set; }
        private string _method { get; set; }
        private string _aplicationId { get; set; }

        public LoginController(
            IUserService userService,
            IOptions<Setting> setting,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMailService mail)
        {
            this._userService = userService;
            this.setting = setting;
            this._uriLDAP = this.setting.Value.LDAPConnection;
            this._token = this.setting.Value.Token;
            this._method = this.setting.Value.IsProduction ? "login" : "validate";
            this._aplicationId = this.setting.Value.ActiveSSO;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _mail = mail;
        }

        [Route("")]
        [AllowAnonymous]
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


        [Route("on-post-ajax")]
        [HttpPost]
        public async Task<IActionResult> doLoginAjax([Bind("UserName", "Password", "RememberMe")] Login login)
        {
            try
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                if (string.IsNullOrEmpty(login.UserName) && string.IsNullOrEmpty(login.Password))
                {
                    return Json(new { Success = false, Message = "Invalid login attempt.", UrlResponse = "" });
                }

                var _params = new LoginLDAP
                {
                    UserName = login.UserName,
                    Password = login.Password,
                    Method = this._method
                };

                var validLDAP = await LoginLDAP(_params);

                if (validLDAP.Success)
                {

                    var user = await this.userManager.FindByNameAsync(login.UserName);
                    if (!String.IsNullOrEmpty(user.Id))
                    {
                        bool isactive = _userService.IsActiveUser(login.UserName);
                        if (isactive)
                        {
                            await this.signInManager.SignInAsync(user, false);
                            validLDAP.Data.UserId = user.Id;
                            validLDAP.Data.UserName = user.UserName;
                            this.setSession(validLDAP.Data);
                            HttpContext.Session.SetString("password", login.Password);
                            return Json(new { Success = validLDAP.Success, Message = "Entering page!", UrlResponse = string.IsNullOrEmpty(ViewBag.ReturnUrl) ? Url.Content("~/") : ViewBag.ReturnUrl });
                        }
                        else 
                        {
                            return Json(new { Success = false, Message = "Inactive User", UrlResponse = "" });
                        }
                    }
                }

                return Json(new { Success = false, Message = "Username or password is incorrect.", UrlResponse = "" });

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message, UrlResponse = "" });
            }

        }

        private async Task<JsonResponse> LoginLDAP(LoginLDAP login)
        {
            JsonResponse? jsonResponse = new JsonResponse();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Token", this._token);

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            using (var response = await client.PostAsync(this._uriLDAP, content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                // If hit API LDAP is success
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                    if (jsonResponse != null)
                    {
                        jsonResponse.Success = jsonResponse.Status == "00" ? true : false;
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                    if (jsonResponse != null)
                    {
                        jsonResponse.Success = false;
                        throw new Exception(jsonResponse.Message);
                    }
                }
                else
                {
                    jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                    if (jsonResponse != null)
                    {
                        jsonResponse.Success = false;
                        throw new Exception(jsonResponse.Message);
                    }
                }
            }

            return jsonResponse;
        }

        private async Task<JsonResponse> LoginLDAP_(LoginLDAP login)
        {
            JsonResponse? jsonResponse = new JsonResponse();
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Token", this._token);

                var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync(this._uriLDAP, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // If hit API LDAP is success
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                        if (jsonResponse != null)
                        {
                            jsonResponse.Success = jsonResponse.Status == "00" ? true : false;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                        if (jsonResponse != null)
                        {
                            jsonResponse.Success = false;
                        }
                    }
                    else
                    {
                        jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                        if (jsonResponse != null)
                        {
                            jsonResponse.Success = false;
                        }
                    }
                }

                return jsonResponse;
            }
            catch (Exception ex)
            {
                jsonResponse.Success = false; jsonResponse.Message = ex.Message;
                return jsonResponse;
            }
        }

        private void setSession(ResponseLDAP data)
        {
            try
            {
                var username = data.Email.Split('@')[0];
                HttpContext.Session.SetString("userId", data.UserId);
                HttpContext.Session.SetString("fullname", data.NamaLengkap);
                HttpContext.Session.SetString("email", data.Email);
                HttpContext.Session.SetString("username", data.UserName);
                HttpContext.Session.SetString("userType", data.IsPDSI ? "PDSI" : "Mitra");
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        private async Task<LoginAPIResponse> LoginAPI(string username, string password)
        {
            LoginAPIResponse? apiresponse = new LoginAPIResponse();

            var data = new[]
            {
                new KeyValuePair<string, string>("method", "Login"),
                new KeyValuePair<string, string>("UserName", username),
                new KeyValuePair<string, string>("Password", password),
            };

            try
            {
                client.DefaultRequestHeaders.Clear();
                var content = new FormUrlEncodedContent(data);
                using (var response = await client.PostAsync(setting.Value.PDSI_Auth_Login, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // If hit API Login is success
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = JsonConvert.DeserializeObject<LoginAPIResponse>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                apiresponse.message = ex.Message;
            }
            return apiresponse;
        }

        private async Task<LoginAPIResponse> ValidateLoginLDAP(string username)
        {
            LoginAPIResponse? apiresponse = new LoginAPIResponse();

            var data = new[]
            {
                new KeyValuePair<string, string>("method", "validate"),
                new KeyValuePair<string, string>("UserName", username),
            };

            try
            {
                client.DefaultRequestHeaders.Clear();
                var content = new FormUrlEncodedContent(data);
                client.DefaultRequestHeaders.Add("Token", this._token);
                using (var response = await client.PostAsync(setting.Value.LDAPConnection, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // If hit API Login is success
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = JsonConvert.DeserializeObject<LoginAPIResponse>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                apiresponse.message = ex.Message;
            }
            return apiresponse;
        }
    }
}
