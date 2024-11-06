using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Data;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/revenue")]
    public class RevenueController : Controller
    {

        private readonly PlanCorpDbContext _context;
        private readonly IRevenueService _revenueService;

        public RevenueController(PlanCorpDbContext context, IRevenueService revenueService)
        {
            _context = context;
            _revenueService = revenueService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("edit")]
        public IActionResult Edit()
        {
            return View();
        }

        
        [HttpPost]
        [Route("get-header-revenue")]
        public JsonResult GetDataHeaderRevenue()
        {
            try
            {
                var list = _revenueService.GetAllHeader();
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
