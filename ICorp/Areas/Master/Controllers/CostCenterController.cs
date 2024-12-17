using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/cost-center")]
    public class CostCenterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
