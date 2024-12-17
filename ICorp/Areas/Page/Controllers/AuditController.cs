using InventoryIT.Areas.Page.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/audits")]
    public class AuditController : Controller
    {
        private readonly IAuditProcessService _auditProcessService;

        public AuditController(IAuditProcessService auditProcessService)
        {
            _auditProcessService = auditProcessService;
        }

        [Route("process")]
        public IActionResult List()
        {
            return View();
        }

        [Route("detail")]
        public IActionResult Detail()
        {
            return View();
        }

        [Route("review")]
        public IActionResult Review()
        {
            return View();
        }

        [Route("review-detail")]
        public IActionResult ReviewDetail()
        {
            return View();
        }

        [HttpPost]
        [Route("get-auction-process-ajax")]
        public JsonResult GetAuctionProcess()
        {
            try
            {
                var list = _auditProcessService.GetList();
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
        [Route("post-update-score-ajax")]
        public JsonResult UpdateScore(int UpId, float Score)
        {
            try
            {
                var list = _auditProcessService.UpdateScore(UpId, Score, HttpContext.Session.GetString("username"));
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
        [Route("get-auction-review-ajax")]
        public JsonResult GetAuctionReview()
        {
            try
            {
                var list = _auditProcessService.GetListReview();
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
        [Route("get-auction-detail-ajax")]
        public JsonResult GetAuctionProcessDetail(string year)
        {
            try
            {
                var list = _auditProcessService.GetDetailList(year);
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
        [Route("response-author-ajax")]
        public JsonResult ResponseAuthorAjax(int UpId, int responseType, string Remarks, string Recommendation, int IsRecommendation, float Score)
        {
            try
            {
                var _params = _auditProcessService.AuthorResponse(UpId, responseType, Remarks, Score, Recommendation, IsRecommendation, HttpContext.Session.GetString("username"));
                
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

        [HttpPost]
        [Route("response-isreadycomplete-ajax")]
        public JsonResult ResponseIsReadyComplete(int year)
        {
            try
            {
                var _params = _auditProcessService.IsReadyComplete(year);

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

        [HttpPost]
        [Route("response-setfinishaudit-ajax")]
        public JsonResult ResponseSetFinishAudit(int year)
        {
            try
            {

                var _params = _auditProcessService.IsReadyComplete(year);

                if (_params.Success && _params.Message == "OK")
                {
                    var _params2 = _auditProcessService.SetFinishAudit(year);
                    return Json(new
                    {
                        Success = _params2.Success,
                        Message = _params2.Message
                    });
                }

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
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
