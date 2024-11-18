using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/inputkonsolidasi")]
    public class InputKonsolidasiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
