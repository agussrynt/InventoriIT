using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/monitoring")]
    public class MonitoringController : Controller
    {
        private readonly IMonitoringService _monitoringService;

        public MonitoringController(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }
        public IActionResult List()
        {
            return View();
        }

        [Route("detail")]
        public IActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        [Route("get-monitoring-list-ajax")]
        public JsonResult GetMonitoringListAjax()
        {
            try
            {
                string userName = HttpContext.Session.GetString("username");
                var list = _monitoringService.GetList(userName);
                return Json(new
                {
                    Success = true,
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = true,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get-monitoring-detail-ajax")]
        public JsonResult GetMonitoringDetailAjax(string year)
        {
            try
            {
                var list = _monitoringService.GetDetailList(year);
                return Json(new
                {
                    Success = true,
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = true,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("post-newduedate-ajax")]
        public JsonResult ResponseIsReadyComplete(int idUP, DateTime newDate)
        {
            try
            {
                var _params = _monitoringService.SetNewDueDate(idUP, newDate);
                //var _params = new ResponseJson();

                return Json(new
                {
                    Success = _params.Success,
                    Message = _params.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = true,
                    Message = ex.Message
                });
            }
        }
    }
}
