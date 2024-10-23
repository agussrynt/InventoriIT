using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/GLAccount")]
    public class GLAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
