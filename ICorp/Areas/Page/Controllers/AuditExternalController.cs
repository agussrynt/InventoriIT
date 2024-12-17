using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize(Roles = "Admin,Auditor")]
    [Area("Page")]
    [Route("page/auditexternal")]
    public class AuditExternalController : Controller
    {
        private readonly IAuditExternalService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuditExternalController(IAuditExternalService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("detail")]
        public IActionResult Display()
        {
            return View();
        }


        [HttpPost]
        [Route("get-list-ajax")]
        public async Task<JsonResult> Gets()
        {
            try
            {
                var list = await _service.GetList();
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

        [Route("upload-score")]
        public async Task<JsonResult> Upload_Score(IList<IFormFile> files, CancellationToken cancellationToken)
        {
            var list = new List<AuditExternalDataScore>();
            string LogError = "";
            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    string filename = formFile.Name.Trim('"');

                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream, cancellationToken);

                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                try
                                {
                                    list.Add(new AuditExternalDataScore
                                    {
                                        ID_AUDIT_EXTERNAL_DATA_SCORE = row,
                                        INDIKATOR = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                        JUMLAH_PARAMATER = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                                        BOBOT = decimal.Parse(worksheet.Cells[row, 4].Value.ToString().Trim()),
                                        SCORE = decimal.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                                        CAPAIAN = int.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
                                    });
                                }
                                catch (Exception ex)
                                {
                                    LogError += row.ToString() + ",";
                                }
                            }
                        }
                    }
                }

                if (LogError == "")
                {
                    return Json(new
                    {
                        Success = true,
                        Data = list
                    });
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        Data = "Error on Row : " + LogError
                    }); ;
                }
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

        [Route("upload-recomendation")]
        public async Task<JsonResult> Upload_Recomendation(IList<IFormFile> files, CancellationToken cancellationToken)
        {
            var list = new List<AuditExternalDataRecomendation>();
            string LogError = "";
            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    string filename = formFile.Name.Trim('"');

                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream, cancellationToken);

                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                try
                                {
                                    list.Add(new AuditExternalDataRecomendation
                                    {
                                        ID_AUDIT_EXTERNAL_DATA_RECOMENDATION = row,
                                        REKOMENDASI = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                        ASPEK = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    });
                                }
                                catch (Exception ex)
                                {
                                    LogError += row.ToString() + ",";
                                }
                            }
                        }
                    }
                }
                if (LogError == "")
                {
                    return Json(new
                    {
                        Success = true,
                        Data = list
                    });
                }
                else 
                {
                    return Json(new
                    {
                        Success = false,
                        Data = "Error on Row : " + LogError
                    }); ;
                }
                
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
                    string path = "\\Documents\\ExternalAudit\\" + newFileName;
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
                response.Message = ex.Message;
            }
            return response;
        }


        [Route("post-create")]
        public async Task<ResponseJson> Post(AuditExternal data)
        {
            ResponseJson response = new ResponseJson();
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


        [Route("post-list-score")]
        public async Task<BaseResponseJson> GetListScore(string id)
        {
            BaseResponseJson response = new BaseResponseJson();
            response = await _service.GetListScore(id);
            return response;
        }

        [Route("post-list-reco")]
        public async Task<BaseResponseJson> GetListRecomendation(string id)
        {
            BaseResponseJson response = new BaseResponseJson();
            response = await _service.GetListRecomendation(id);
            return response;
        }

        [Route("find-item-audit")]
        public async Task<BaseResponseJson> Find(string id)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AuditExternal> lst = new List<AuditExternal>();
            AuditExternal obj = new AuditExternal();
            try 
            {
                lst = await _service.Find(id);
                if (lst.Count > 0)
                {
                    obj = lst.FirstOrDefault();
                    response.Success = true;
                    response.Data = obj;
                }
                else 
                {
                    response.Success = false;
                    response.Message = "Data Not Found";
                }
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
