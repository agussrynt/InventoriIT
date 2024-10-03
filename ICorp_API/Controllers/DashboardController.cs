using PlanCorp_API.Models;
using PlanCorp_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("response-internal-temuan-ajax")]
        public async Task<ResponseModel> ResponseInternalTemuanAjax()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _service.GetTemuanAuditInternal();
                response.Data = result;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("response-external-temuan-ajax")]
        public async Task<ResponseModel> ResponseExternalTemuanAjax()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _service.GetTemuanAuditExternal();
                response.Data = result;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("response-internal-followup-ajax")]
        public async Task<ResponseModel> ResponseInternalFollowUpAjax()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _service.GetFollowUpAuditInternal();
                response.Data = result;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("response-external-followup-ajax")]
        public async Task<ResponseModel> ResponseExternalFollowUpAjax()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _service.GetFollowUpAuditExternal();
                response.Data = result;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
