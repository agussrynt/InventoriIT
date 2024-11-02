using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlanCorp.Areas.Identity;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Areas.Master.Service;
using PlanCorp.Data;
using PlanCorp.Models;

namespace PlanCorp.Areas.Master.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Master")]
    [Route("master/asset-revenue")]
    public class AssetController : Controller
    {
        private readonly PlanCorpDbContext _context;
        private readonly IAssetService _assetService;

        public AssetController(PlanCorpDbContext context, IAssetService assetService)
        {
            _context = context;
            _assetService = assetService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("get-asset-list")]
        public JsonResult GetDataAssets()
        {
            try
            {
                var list = _assetService.GetAllAsset();

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("get-assetType-ajax")]
        public JsonResult GetAssetType()
        {
            try
            {
                var list = _assetService.GetAssetTypeDropdown();

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

        [HttpGet]
        [Route("get-CostCenterDD-ajax")]
        public JsonResult GetCostCenterDD()
        {
            try
            {
                var list = _assetService.GetCostCenterDropdown();

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
        [Authorize(Roles = "Admin")]
        [Route("create-asset-ajax")]
        public JsonResult CreateAsset(Assets param)
        {
            try
            {
                param.CreatedBy = HttpContext.Session.GetString("username");
                var r = _assetService.SaveOrUpdate(param);
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
        [Route("update-asset-ajax")]
        public JsonResult UpdateAsset(Assets param)
        {
            try
            {
                var r = _assetService.SaveOrUpdate(param);
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
        [Route("delete-asset-ajax")]
        public JsonResult DeleteDataAsset(int IdAsset)
        {
            try
            {
                var r = _assetService.DeleteAsset(IdAsset);
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