using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/profit-and-loss")]
    public class ProfitLossController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
