using Microsoft.AspNetCore.Mvc;
using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;
using Microsoft.AspNetCore.Authorization;

namespace InventoryIT.Areas.Page.Controllers
{
    [Area("Page")]
    [Authorize(Roles = "Admin,Auditor")]
    [Route("page/auditexternalmonitoring")]
    public class AuditExternalMonitoringController : Controller
    {
        public readonly IAuditExternalMonitoring _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuditExternalMonitoringController(IAuditExternalMonitoring service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("get-list-ajax")]
        public async Task<BaseResponseJson> GetList()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AuditExternalFollowUpList> result = new List<AuditExternalFollowUpList>();
            try
            {
                string username = User.Identity.Name;
                result = await _service.GetList();
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
        public async Task<BaseResponseJson> Create([FromBody] AuditExternalFollowUp data)
        {
            BaseResponseJson response = new BaseResponseJson();
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
