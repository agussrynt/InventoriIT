using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/data-asset")]
    public class AssetController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
