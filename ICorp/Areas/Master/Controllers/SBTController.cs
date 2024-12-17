using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryIT.Areas.Master.Interface;
using InventoryIT.Data;

namespace InventoryIT.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/SBT")]
    public class SBTController : Controller
    {
        private readonly PlanCorpDbContext _context;
        private readonly IProjectService _projectService;

        public SBTController(PlanCorpDbContext context, IProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("get-SBT-list")]
        public JsonResult GetSBTDD()
        {
            try
            {
                var list = _projectService.GetSBTProject();

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
    }
}