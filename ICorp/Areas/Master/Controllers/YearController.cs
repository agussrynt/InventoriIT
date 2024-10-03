using PlanCorp.Areas.Master.Interface;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Master.Controllers
{
    [Area("Master")]
    [Route("master/year")]
    public class YearController : Controller
    {
        private readonly IYearService _yearService;

        public YearController(IYearService yearService)
        {
            _yearService = yearService;
        }

        [Route("list")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("get-list-year-ajax")]
        public JsonResult GetListYearAjax()
        {
            try
            {
                var list = _yearService.GetAll();

                return Json(new
                {
                    Success = true,
                    Data = list
                });
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
