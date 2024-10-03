using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/role-management")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRoleService roleService;

        public RoleController(RoleManager<IdentityRole> roleManager, IRoleService roleService)
        {
            this.roleManager = roleManager;
            this.roleService = roleService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-role-list")]
        public async Task<JsonResult> GetAllRole()
        {
            try
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                foreach (var role in this.roleManager.Roles.ToList())
                {
                    if (role.Id != "e9d69d30-6861-46f5-8fb5-b091145cb99b")
                    {
                        roles.Add(role);
                    }
                }

                return Json(new
                {
                    success = true,
                    data = roles
                });
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                return Json(new
                {
                    success = false,
                    data = ex.InnerException
                });
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("create-role-ajax")]
        public JsonResult CreateRoleAjax([FromBody] RoleParam param)
        {
            try
            {
                param.Id = Guid.NewGuid().ToString().ToLower();
                var r = roleService.SaveOrUpdate(param);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("update-role-ajax")]
        public JsonResult UpdateRoleAjax([FromBody] RoleParam param)
        {
            try
            {
                var r = roleService.SaveOrUpdate(param);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("delete-role-ajax")]
        public JsonResult DeleteRoleAjax([FromBody] RoleParam param)
        {
            try
            {
                var r = roleService.DeleteRole(param.Id);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
