using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Services;
using PlanCorp.Data;

namespace PlanCorp.Areas.Page.Controllers
{
  [Authorize]
	[Area("Page")]
	[Route("page/revhppga")]
	public class RevHPPGAController : Controller
	{

        private readonly PlanCorpDbContext _context;
        private readonly IRevenue_RJPP_Service _revenue_RJPP_Service;

        public RevHPPGAController(PlanCorpDbContext context, IRevenue_RJPP_Service revenue_RJPP_Service)
        {
            _context = context;
            _revenue_RJPP_Service = revenue_RJPP_Service;
        }

        public IActionResult Index()
        {
          return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("get-all-revhppga")]
        public JsonResult GetData_Revenue_HPP_GA()
        {
            try
            {
                var list = _revenue_RJPP_Service.GetAllRevenue();

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

        [AllowAnonymous]
        [HttpPost]
        [Route("upload-file-revhppga")]
        public async Task<JsonResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Please upload a valid file."
                    });
                }

                // Logika proses file
                return Json(new
                {
                    Success = true,
                    Message = "File uploaded successfully!"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return Json(new
                {
                    Success = false,
                    Data = 5 ,
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

    }
}