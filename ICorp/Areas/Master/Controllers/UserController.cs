using InventoryIT.Areas.Master.Models;
using InventoryIT.Areas.Master.Interface;
using InventoryIT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using InventoryIT.Models;
using InventoryIT.Areas.Identity;
using InventoryIT.Helpers;
using InventoryIT.Areas.Account.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Text;

namespace InventoryIT.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/user-management")]
    public class UserController : Controller
    {
        private readonly PlanCorpDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOptions<Setting> setting;

        //new add
        //private readonly TokoLanggananContext _context;
        private readonly IUserInRoleService _userInRoleSevice;
        private readonly RoleManager<PlanCorpUser> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IOptions<Setting> _setting;
        //private readonly UserManager<TokoLanggananUser> _userManager;
        static HttpClient client = new HttpClient();


        public UserController(
            PlanCorpDbContext context,
            IUserService userService,
            //IUserStore<PlanCorpUser> userStore, // new add
            //UserManager<IdentityUser> userManager,
            UserManager<IdentityUser> userManager,
            IOptions<Setting> setting,
            IUserStore<IdentityUser> userStore,
            IUserInRoleService userInRoleSevice)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            this._userStore = userStore;
            this._setting = setting;
            this._token = this._setting.Value.Token;
            this._uriLDAP = this._setting.Value.LDAPConnection;
            this._method = this._setting.Value.IsProduction ? "login" : "validate";
            _emailStore = GetEmailStore();
            _userInRoleSevice = userInRoleSevice;
        }
        private string _token { get; set; }
        private string _uriLDAP { get; set; }
        private string _method { get; set; }
        public IActionResult Index()
        {
            //Fetching Skills Records in LIST Collection format.  
            //var listRole = (from role in _context.Roles orderby role.Name select role).ToList();
            //ViewBag.ListRole = listRole;
            return View();
        }

        [HttpPost]
        [Route("fetch-data-user")]
        public async Task<IActionResult> FetchUserAjax()
        {
            try
            {
                BaseResponseJson res = new BaseResponseJson();
                List<UserInRoleView> listUser = new List<UserInRoleView>();
                res = await _userService.Gets();
                listUser = res.Data;
                return Json(new
                {
                    success = true,
                    data = listUser
                });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return Json(new
                {
                    success = false,
                    data = ex.InnerException
                });
            }

        }

        //[HttpPost]
        //[Route("fetch-data-user")]
        //public async Task<IActionResult> FetchUserAjax()
        //{
        //    try
        //    {
        //        List<UserInRole> getUser = new List<UserInRole>();
        //        //var userLogin = await _userManager.GetUserAsync(HttpContext.User);
        //        getUser = await _context.UserInRoles.FromSqlRaw("EXEC usp_Get_User_List").ToListAsync();
        //        var allusers = getUser;
        //        //var allusers = await _userInRoleSevice.GetAllUserInRoles();

        //        return Json(new
        //        {
        //            success = true,
        //            data = getUser
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex);
        //        return Json(new
        //        {
        //            success = false,
        //            data = ex.InnerException
        //        });
        //    }

        //}

        [AllowAnonymous]
        [HttpPost]
        [Route("get-pic-ajax")]
        public JsonResult GetPIC([FromBody] ParameterData parameterData)
        {
            try
            {
                var list = _userService.GetPICDropdown(parameterData.ParamsId);

                return Json(new
                {
                    Success = true,
                    Data = list
                });
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }

        [HttpGet]
        [Route("on-list-api-pdsi-ajax")]
        public IActionResult GetValidateLDAP()
        {
            string url = "https://api.logikatekno.com/testing";
            var result = RESTHelper.Get<Response_UserFPP>(url); //
            var data = _context.TOKLANG_PEJABATFPP.FirstOrDefault(a => a.Nopek == result.Data.EmpNumber);
            //if (data != null)
            //{
            //    //check = 1;
            //}
            //var fixResult = _userInRoleSevice.GetUserFPPByEmpNumber(result.Data.EmpNumber);
            int status = 0;
            if (data != null)
            {
                status = 1;
            }
            var details = new { list = result, _role = status, pejabat_fpp = data };
            return Json(details);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("create-administrator-ajax")]
        public async Task<JsonResult> CreateAdministratorAjax(AddUser addUser)
        {
            try
            {
                //fungsi insertnya di pindah ke controller, ga bisa di servis
                //sementara ini solusinya seperti validate LDAP

                var user = CreateUser();
                user.Email = addUser.Email;
                user.UserName = addUser.UserName;
                addUser.Password = "Random123";


                //insert ke tabel AspNetUser
                await _userStore.SetUserNameAsync(user, addUser.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, addUser.Password);

                ////insert ke tabel profile
                if (result.Succeeded)
                {
                    var createdAt = DateTime.Now;
                    var dataProfile = new Profile
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        UserType = 0,
                        UserName = addUser.UserName,
                        FirstName = addUser.FirstName,
                        LastName = addUser.LastName,
                        FungsiId = addUser.FungsiId,
                        CreatedAt = createdAt,
                        CreatedBy = HttpContext.Session.GetString("username"),
                        UpdatedAt = createdAt,
                        UpdatedBy = HttpContext.Session.GetString("username"),
                        IsActive = true
                    };
                    _context.Profiles.Add(dataProfile);
                    _context.Entry(dataProfile).State = EntityState.Added;
                    await _context.SaveChangesAsync();

                   // await _context.Profiles.AddAsync(dataProfile);

                    //insert ke tabel [AspNetUserRoles]
                    BaseResponseJson rl = await _userInRoleSevice.RemoveAllRole(user.Id);
                    foreach (var role in addUser.Roles.ToList())
                    {
                        //await _userManager.AddToRoleAsync(user, role);
                        await _userInRoleSevice.SetUserRole(user.Id, role);
                    }

                    return Json(new
                    {
                        Success = true,
                        Message = "Successfully create user!",
                    });
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Failed create user!",
                    });
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                return Json(new
                {
                    Success = false,
                    data = ex.InnerException
                });
            }

        }

        [HttpPost]
        [Route("edit-administrator-ajax")]
        public async Task<JsonResult> EditAdministratorAjax(AddUser addUser)
        {
            try
            {
                var user = new IdentityUser();
                user.Email = addUser.Email;
                user.UserName = addUser.UserName;
                user.Id = addUser.IdUser;
                addUser.Password = "Random123";

                //await _userStore.SetUserNameAsync(user, addUser.UserName, CancellationToken.None);
                //await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
                //var result = await _userManager.UpdateAsync(user);

                //if (result.Succeeded)
                //{
                //Profile dataProfile = _context.Profiles.Where(s => s.UserId == user.Id).FirstOrDefault();
                Profile dataProfile = _context.Profiles.Find(Convert.ToInt32(addUser.IdProfile));
                var createdAt = DateTime.Now;

                dataProfile.Email = user.Email;
                dataProfile.UserType = 0;
                dataProfile.UserName = addUser.UserName;
                dataProfile.FirstName = addUser.FirstName;
                dataProfile.LastName = addUser.LastName;
                dataProfile.UpdatedAt = createdAt;
                dataProfile.FungsiId = addUser.FungsiId;
                dataProfile.UpdatedBy = HttpContext.Session.GetString("username");

                _context.Profiles.Add(dataProfile);
                _context.Entry(dataProfile).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                BaseResponseJson rl = await _userInRoleSevice.RemoveAllRole(user.Id);

                if (rl.Success)
                {
                    foreach (var role in addUser.Roles.ToList())
                    {
                        await _userInRoleSevice.SetUserRole(user.Id, role);
                    }
                }

                return Json(new
                {
                    Success = true,
                    Message = "Successfully create user!",
                });
                //}
                //else
                //{
                //    return Json(new
                //    {
                //        Success = false,
                //        Message = "Failed create administrator!",
                //    });
                //}
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                return Json(new
                {
                    Success = false,
                    data = ex.InnerException
                });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("create-validateldap-ajax")]
        public async Task<JsonResponse> validateDAP(LoginLDAP login)
        {
            JsonResponse jsonResponse = new JsonResponse();
            try
            {
                var _params = new LoginLDAP
                {
                    UserName = login.UserName,
                    Password = login.Password,
                    Method = this._method
                };

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Token", this._token);

                var content = new StringContent(JsonConvert.SerializeObject(_params), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync(this._uriLDAP, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // If hit API LDAP is success
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                        jsonResponse.Success = jsonResponse.Status == "00" ? true : false;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                        jsonResponse.Success = false;
                    }
                    else
                    {
                        jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(apiResponse);
                        jsonResponse.Success = false;
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

        [HttpPost]
        [Route("activate-ajax")]
        public async Task<JsonResult> ActiveteUserAjax(ActiveteUser param)
        {
            try
            {
                bool srv = _userService.ActivateUser(param.UserName, param.IsActivate);
                return Json(new
                {
                    Success = false
                });
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                return Json(new
                {
                    Success = false,
                    data = ex.InnerException
                });
            }

        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
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

    }
    public class Rootobject
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public bool value { get; set; }
        public string EmpNumber { get; set; }
        public string NamaLengkap { get; set; }
        public string Email { get; set; }
        public string PosID { get; set; }
        public string PosText { get; set; }
        public string DirID { get; set; }
        public string DirText { get; set; }
        public string DivID { get; set; }
        public string DivText { get; set; }
        public string DepID { get; set; }
        public string DepText { get; set; }
        public bool IsMitra { get; set; }
        public bool IsPDSI { get; set; }
    }
}

