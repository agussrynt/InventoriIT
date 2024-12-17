using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize(Roles = "Admin,Auditor")]
    [Area("Page")]
    [Route("page/auditexternalassigment")]
    public class AuditExternalAssigmentController : Controller
    {
        public readonly IAuditExternalAssigmentService _service;
        public AuditExternalAssigmentController(IAuditExternalAssigmentService service) 
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("get-list-ajax")]
        public async Task<BaseResponseJson> GetList(int year)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AuditExternalAssigmentRecomendationList> result = new List<AuditExternalAssigmentRecomendationList>();
            try
            {
                result = await _service.GetList(year);
                response.Data = result;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [Route("post-ajax")]
        public async Task<BaseResponseJson> Create([FromBody] AuditExternalAssigmentRecomendation data)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AuditExternalAssigmentRecomendationList> result = new List<AuditExternalAssigmentRecomendationList>();
            try
            {
                data.CREATEDBY = User.Identity.Name;

                response = await _service.Create(data);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
