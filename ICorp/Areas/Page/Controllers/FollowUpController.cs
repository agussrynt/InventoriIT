using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/follow-up")]
    public class FollowUpController : Controller
    {
        private readonly IFollowUpService _followUpService;
        private readonly IWebHostEnvironment _environment;

        public FollowUpController(IFollowUpService followUpService, IWebHostEnvironment environment)
        {
            _followUpService = followUpService;
            _environment = environment;
        }

        public IActionResult List()
        {
            return View();
        }

        [Route("detail")]
        public IActionResult Detail()
        {
            return View();
        }

        [HttpPost]
        [Route("get-follow-up-list-ajax")]
        public JsonResult GetFollowUpListAjax()
        {
            try
            {
                string userName = HttpContext.Session.GetString("username");
                var list = _followUpService.GetList(userName);
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
        [Route("get-follow-up-detail-ajax")]
        public JsonResult GetFollowUpDetailAjax(string year)
        {
            try
            {
                var list = _followUpService.GetDetailList(year);
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
        [Route("post-follow-up-response-ajax")]
        public JsonResult PostFollowUpResponseAjax(int Id, string Remarks, IFormFile FileUpload)
        {
            try
            {
                if (FileUpload != null)
                {
                    if (FileUpload.Length > 0)
                    {
                        string wwwPath = _environment.WebRootPath;
                        string contentPath = _environment.ContentRootPath;

                        var fileName = Path.GetFileName(FileUpload.FileName);
                        var fileExtension = Path.GetExtension(fileName);
                        string userName = HttpContext.Session.GetString("username");

                        string docName = fileName.Split(".")[0];
                        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        docName = docName + "-" + timeStamp + fileExtension;

                        string pathDocument = Path.Combine(wwwPath, "Documents");
                        string pathFile = Path.Combine(pathDocument, docName);
                        string path = "\\Documents\\" + docName;

                        var objfiles = new FollowUpUpload
                        {
                            UpID = Id,
                            Remarks = Remarks,
                            FileName = fileName,
                            FileType = fileExtension,
                            FileSize = FileUpload.Length,
                            userName = userName,
                            FilePath = path,
                        };
                        using (var target = new MemoryStream())
                        {
                            FileUpload.CopyTo(target);
                            objfiles.FileData = target.ToArray();
                        }

                        using (FileStream stream = new FileStream(pathFile, FileMode.Create))
                        {
                            FileUpload.CopyTo(stream);
                        }

                        var response = _followUpService.FollowUPDocument(objfiles);
                    }
                }

                return Json(new
                {
                    Success = true,
                    Data = String.Empty
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Data = ex.Message
                });
            }
        }
    }
}
