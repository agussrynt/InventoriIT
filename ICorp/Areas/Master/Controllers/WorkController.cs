using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryIT.Areas.Master.Interface;
using InventoryIT.Areas.Master.Models;
using InventoryIT.Areas.Master.Service;
using InventoryIT.Data;

namespace InventoryIT.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/works")]
    public class WorkController : Controller
    {
        private readonly PlanCorpDbContext _context;
        private readonly IWorkService _workService;

        public WorkController(PlanCorpDbContext context, IWorkService workService)
        {
            _context = context;
            _workService = workService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("get-work-list")]
        public JsonResult GetDataWorks()
        {
            try
            {
                var list = _workService.GetAllWorks();

                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data
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

        // GET: WorkController
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("create-work-ajax")]
        public JsonResult CreateWork(Works param)
        {
            try
            {
                param.IsAktif = 1;
                param.CreatedBy = HttpContext.Session.GetString("username");

                var r = _workService.SaveOrUpdate(param);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("update-work-ajax")]
        public JsonResult UpdateWork(Works param)
        {
            try
            {
                var r = _workService.SaveOrUpdate(param);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("delete-work-ajax")]
        public JsonResult DeleteDataWork(int IdWork)
        {
            try
            {
                var r = _workService.DeleteWorks(IdWork);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}