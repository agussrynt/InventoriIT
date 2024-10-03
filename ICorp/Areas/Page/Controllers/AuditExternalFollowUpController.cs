using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize(Roles = "Auditee")]
    [Area("Page")]
    [Route("page/auditexternalfollowup")]
    public class AuditExternalFollowUpController : Controller
    {
        public readonly IAuditExternalFolowUpService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuditExternalFollowUpController(IAuditExternalFolowUpService service, IWebHostEnvironment webHostEnvironment)
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
                result = await _service.GetList(username);
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

        [Route("upload-file")]
        public async Task<ResponseJson> Upload_MainFile()
        {
            ResponseJson response = new ResponseJson();
            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    string filename = formFile.Name.Trim('"');

                    filename = this.EnsureCorrectFilename(filename);
                    string newFileName = Guid.NewGuid() + "_" + filename;
                    string path = "\\Documents\\ExternalAuditFollowUp\\" + newFileName;
                    string filePath = this.GetPathAndFilename(path);

                    using (FileStream output = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(output);
                    }

                    response.Success = true;
                    response.Message = filename;
                    response.UrlResponse = path;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
            }
            return response;
        }

        public IActionResult Index()
        {
            return View();
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            string path = "";
            path = Path.Combine(webRootPath, "CSS");
            return webRootPath + filename;
        }

    }
}
