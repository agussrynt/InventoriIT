using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/document-upload")]
    public class DocumentUploadController : Controller
    {
        private readonly IDokumenUPService _dokumenUPservice;
        private readonly IWebHostEnvironment _environment;
        public DocumentUploadController(IDokumenUPService dokumenUPservice, IWebHostEnvironment environment)
        {
            _dokumenUPservice = dokumenUPservice;
            _environment = environment;
        }

        [Route("")]
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
        [Route("get-dokumen-ajax")]
        public JsonResult GetDokumentAjax()
        {
            try
            {
                string userName = HttpContext.Session.GetString("username");
                var list = _dokumenUPservice.GetList(userName);
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
                    Success = false,
                    Data = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get-dokumen-detail-ajax")]
        public JsonResult GetDokumentDetailAjax(string year)
        {
            try
            {
                string? userName = HttpContext.Session.GetString("username");
                var list = _dokumenUPservice.GetDetailList(userName, year);
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
                    Success = false,
                    Data = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("upload-ajax")]
        public JsonResult UploadDokumentAjax(int Id, string Remarks, IFormFile FileUpload)
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
                        string docName = fileName.Split(".")[0];
                        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string userName = HttpContext.Session.GetString("username");
                        docName = docName + "-" + timeStamp + fileExtension;

                        string pathDocument = Path.Combine(wwwPath, "Documents");
                        string pathFile = Path.Combine(pathDocument, docName);
                        string path = "\\Documents\\" + docName;

                        if (!Directory.Exists(pathDocument))
                        {
                            Directory.CreateDirectory(pathDocument);
                        }

                        var objfiles = new DokumenUpload
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

                        var response = _dokumenUPservice.UploadDocumentUP(objfiles);
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

        [HttpGet]
        [Route("download-file-ajax")]
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_environment.WebRootPath, "Documents\\ExternalAudit\\Template") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
