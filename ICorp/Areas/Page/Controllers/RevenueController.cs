using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using System.Diagnostics;

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

        [HttpGet]
        [Route("amount")]
        public IActionResult Amount()
        {
            return View();
        }

        [HttpGet]
        [Route("amount-input")]
        public IActionResult AmountInput()
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("create-header-ajax")]
        public JsonResult CreateHeader([FromBody] HeaderRevenue param)
        {
            try
            {
                param.CreatedBy = HttpContext.Session.GetString("username");
                param.CreatedTime = DateTime.Now;
                var r = _revenueService.SaveOrUpdate(param);
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
        [Route("get-project-revenue")]
        public JsonResult GetDataProjectRevenue(int idHeader)
        {
            Debug.WriteLine("id Headernya"+idHeader);
            if (idHeader <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid idHeader value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetProjectRevenue(idHeader);
                var headerEdit = _revenueService.GetDetailHeaderRevenue(idHeader);
                return Json(new
                {
                    Success = true,
                    Data = new
                    {
                        list = list.Result.Data,
                        headerEdit = headerEdit.Result.Data,
                    }
                    //Data = list.Result.Data
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


        [HttpGet]
        [Route("get-projectDD-ajax")]
        public JsonResult GetProjectDD()
        {
            try
            {
                var list = _revenueService.GetProjectExist();

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

        [HttpPost]
        [Route("get-project-exist")]
        public JsonResult GetProjectExistByID(int ProjectID)
        {
            Debug.WriteLine("id Projectnya" + ProjectID);
            if (ProjectID <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid idProject value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetProjectExistById(ProjectID);
                
                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,
            
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

        [HttpPost]
        [Route("get-costcenter-fill")]
        public JsonResult GetCostCenterFill(int AssetID)
        {
            Debug.WriteLine("id Projectnya" + AssetID);
            if (AssetID <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid ID Asset value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetCostCenterFill(AssetID);

                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,

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

        [HttpPost]
        [Route("get-regional-fill")]
        public JsonResult GetRegionalFill(int CustomerID)
        {
            Debug.WriteLine("id Projectnya" + CustomerID);
            if (CustomerID <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid ID Customer value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetRegionalFill(CustomerID);

                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("create-mapping-project")]
        public JsonResult CrateMappingProject([FromBody] MappingProjectRevenue param)
        {
            try
            {
                param.CreatedBy = HttpContext.Session.GetString("username");
                var r = _revenueService.SaveMappingProject(param);
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
        [Route("get-detail-revenue")]
        public JsonResult GetDetailRevenue(int idHeader)
        {
            Debug.WriteLine("id Headernya" + idHeader);
            if (idHeader <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid idHeader value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetDetailRevenue(idHeader);
                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,
             
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
