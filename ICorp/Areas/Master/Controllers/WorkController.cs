using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/work")]
    public class WorkController : Controller
    {
        // GET: WorkController
        public IActionResult Index()
        {
            return View();
        }

    }
}