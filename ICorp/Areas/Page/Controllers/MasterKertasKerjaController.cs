using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize(Roles = "Admin,Auditor")]
    [Area("Page")]
    [Route("page/master-kertas-kerja")]
    public class MasterKertasKerjaController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IMasterKertasKerjaService _masterKertasKerjaService;

        public MasterKertasKerjaController(IWebHostEnvironment environment, IMasterKertasKerjaService masterKertasKerjaService)
        {
            _environment = environment;
            _masterKertasKerjaService = masterKertasKerjaService;
        }

        [HttpPost]
        [Route("get-dropdown-ajax")]
        //public JsonResult Store([FromBody] int year, List<ParameterHeader> parameterHeaders, List<ParameterContent> parameterContents)
        public JsonResult GetDropwdon(int ParamsId = 0, int Option = 0)
        {
            try
            {
                var list = _masterKertasKerjaService.GetDropdown(ParamsId, Option);

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

        [HttpGet]
        [Route("get-datamasterkertaskerja-ajax/{year}")]
        //public JsonResult Store([FromBody] int year, List<ParameterHeader> parameterHeaders, List<ParameterContent> parameterContents)
        public BaseResponseJson GetDataMasterKertasKerja(int year)
        {
            BaseResponseJson responseJson = new BaseResponseJson();
            try
            {
                responseJson = _masterKertasKerjaService.GetDataMasterKertasKerja(year);
            }
            catch (Exception ex)
            {
                responseJson.Success = false;
                responseJson.Message = ex.Message;
            }
            return responseJson;
        }

        //GetDataMasterKertasKerja

        [Route("")]
        public IActionResult List()
        {
            return View();
        }

        [Route("add")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("update")]
        public IActionResult Edit()
        {
            return View();
        }

        [Route("_partialJS")]
        public IActionResult _PartialJS()
        {
            return PartialView();
        }

        [HttpPost]
        [Route("store-ajax")]
        //public JsonResult Store([FromBody] int year, List<ParameterHeader> parameterHeaders, List<ParameterContent> parameterContents)
        //public IActionResult Store([FromBody] StoreModel storeModel)
         public IActionResult Store([FromBody] StoreModel data)
        {
            try
            {
                if (data == null)
                    throw new Exception("Parameter is null!");
                
                var _params = data;
                var UserName = HttpContext.Session.GetString("username");
                _params.UserName = String.IsNullOrEmpty(UserName) ? "iman.ramadhan" : UserName;
                var response = _masterKertasKerjaService.StoreMasterKertasKerja(_params);

                return Json(new
                {
                    Success = response.Success,
                    Message = response.Message
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
        

        [Route("import")]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [Route("upload-ajax")]
        public JsonResult UploadAjax(IFormFile FileUpload)
        {
            try
            {
                if (FileUpload != null)
                {
                    if (FileUpload.Length > 0)
                    {
                        var stream = FileUpload.OpenReadStream();
                        try
                        {
                            var sqlConnection = new SqlConnection(@"Data Source=192.168.3.250;" +
                                    "Initial Catalog=PDSI-GCG;" +
                                    "User id=sa;" +
                                    "Password=password.1;");

                            List<IndikatorParameter> indikator = new List<IndikatorParameter>();
                            List<FUKUP> fukup = new List<FUKUP>();
                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetIndikator = package.Workbook.Worksheets["Indikator - Parameter"];
                                //ExcelWorksheet workSheetFukUp = package.Workbook.Worksheets["FUK - UP"];
                                int totalRowsIndikator = workSheetIndikator.Dimension.Rows;
                                //int totalRowsFukUp = workSheetFukUp.Dimension.Rows;
                                for (int i = 2; i <= totalRowsIndikator; i++)
                                {
                                    indikator.Add(new IndikatorParameter
                                    {
                                        IndikatorCategory = workSheetIndikator.Cells[i, 1].Value.ToString(),
                                        NoParameter = workSheetIndikator.Cells[i, 2].Value.ToString(),
                                        Parameter = workSheetIndikator.Cells[i, 3].Value.ToString(),
                                        Bobot = Convert.ToDecimal(workSheetIndikator.Cells[i, 4].Value.ToString())
                                    });
                               }
                            }

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetFukUp = package.Workbook.Worksheets["FUK - UP"];
                                int totalRowsFukUp = workSheetFukUp.Dimension.Rows;
                                for (int i = 2; i <= totalRowsFukUp; i++)
                                {
                                    var score = "0.613";
                                    var cell4 = "";
                                    //cell4 = !String.IsNullOrEmpty(workSheetFukUp.Cells[i, 4].Value.ToString()) ? workSheetFukUp.Cells[i, 4].Value.ToString() : "-";
                                    if (string.IsNullOrEmpty(workSheetFukUp.Cells[i, 4].Value.ToString())){
                                        cell4 = "-";
                                    } else {
                                        cell4 = workSheetFukUp.Cells[i, 4].Value.ToString();
                                    }

                                    fukup.Add(new FUKUP
                                    {
                                        NoParameter = workSheetFukUp.Cells[i, 1].Value.ToString(),
                                        Seq = workSheetFukUp.Cells[i, 2].Value.ToString(),
                                        FUK = workSheetFukUp.Cells[i, 3].Value.ToString(),
                                        UP = cell4
                                    });
                               }
                            }
                            return Json(new
                            {
                                Success = true,
                                Data = new
                                {
                                    Indicator = indikator,
                                    Fuk = fukup,
                                    Status = true
                                }
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

                return Json(new
                {
                    Success = false,
                    Message = String.Empty
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
        [Route("import-excel")]
        private string Impr(IFormFile file)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string wwwPath = _environment.WebRootPath;
                    string contentPath = _environment.ContentRootPath;

                    string pathDocument = Path.Combine(wwwPath, "Documents");
                    if (!Directory.Exists(pathDocument))
                    {
                        Directory.CreateDirectory(pathDocument);
                    }
                    // Getting FileName
                    string fileName = Path.GetFileName(file.FileName);
                    string docName = fileName.Split(".")[0];
                    // Getting File Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // Make TimeStamp
                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                    // Custom File Name
                    docName = docName + "-" + timeStamp + fileExtension;
                    using (FileStream stream = new FileStream(Path.Combine(pathDocument, docName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return docName;
                }
            }

            return null;
        }

        [HttpPost]
        [Route("new-upload-ajax")]
        public JsonResult NewUploadAjax(IFormFile FileUpload)
        {
            try
            {
                if (FileUpload != null)
                {
                    if (FileUpload.Length > 0)
                    {
                        var stream = FileUpload.OpenReadStream();
                        try
                        {
                            var sqlConnection = new SqlConnection(@"Data Source=192.168.3.250;" +
                                    "Initial Catalog=PDSI-GCG;" +
                                    "User id=sa;" +
                                    "Password=password.1;");

                            List<IndikatorParameter> indikator = new List<IndikatorParameter>();
                            List<FUKUP> fukup = new List<FUKUP>();
                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetIndikator = package.Workbook.Worksheets["Indikator - Parameter"];
                                ExcelWorksheet workSheetFukUp = package.Workbook.Worksheets["FUK - UP"];
                                int totalRowsIndikator = workSheetIndikator.Dimension.Rows;
                                int totalRowsFukUp = workSheetFukUp.Dimension.Rows;
                                for (int i = 2; i <= totalRowsIndikator; i++)
                                {
                                    indikator.Add(new IndikatorParameter
                                    {
                                        IndikatorCategory = workSheetIndikator.Cells[i, 1].Value.ToString(),
                                        NoParameter = workSheetIndikator.Cells[i, 2].Value.ToString(),
                                        Parameter = workSheetIndikator.Cells[i, 3].Value.ToString(),
                                        Bobot = Convert.ToDecimal(workSheetIndikator.Cells[i, 4].Value.ToString())
                                    });

                                    #region
                                    var query_indikator =
                                           "INSERT INTO tbl_M_Indicator " +
                                           "(" +
                                           " [ID_YEARDATA] " +
                                           ",[SEQ]    " +
                                           ",[INDICATORNAME]" +
                                           ",[BOBOT]" +
                                           ",[CREATEDBY]" +
                                           ",[CREATEDDATE] " +
                                           ",[LASTUPDATEDBY] " +
                                           ",[LASTUPDATEDDATE]" +
                                           " )" +
                                           "VALUES  ( " +
                                           " '" + 14 + "' " +
                                           ", '" + 1 + "' " +
                                           ", '" + workSheetIndikator.Cells[i, 1].Value.ToString() + "' " +
                                           ", NULL " +
                                           ", 'System' " +
                                           ", getdate() " +
                                           ", 'System' " +
                                           ", getdate() " +
                                           ") ";

                                    var mySqlCommand = new SqlCommand
                                    {
                                        Connection = sqlConnection,
                                        CommandText = query_indikator,
                                    };
                                    sqlConnection.Open();
                                    var mySqlDataReader = mySqlCommand.ExecuteReader();
                                    sqlConnection.Close();

                                    var query_parameter =
                                        " INSERT INTO [dbo].[tbl_M_Parameter] " +
                                        " (" +
                                        " [ID_INDICATOR]" +
                                        ",[SEQ] " +
                                        ",[PARAMETERDESC] " +
                                        ",[BOBOT] " +
                                        ",[CREATEDBY] " +
                                        ",[CREATEDDATE] " +
                                        ",[LASTUPDATEDBY] " +
                                        ",[LASTUPDATEDDATE]" +
                                        ") " +
                                        " VALUES" +
                                        " (" +
                                        " 2024 " + //cek cara get last record
                                        ",0 " +
                                        ", '" + workSheetIndikator.Cells[i, 3].Value.ToString() + "' " +
                                        ", '" + Convert.ToDecimal(workSheetIndikator.Cells[i, 4].Value.ToString()) + "' " +
                                        ",'System' " +
                                        ",getdate() " +
                                        ",'System' " +
                                        ",getdate() " +
                                        ")";
                                    var mySqlCommand_Parameter = new SqlCommand
                                    {
                                        Connection = sqlConnection,
                                        CommandText = query_parameter,
                                    };
                                    sqlConnection.Open();
                                    var mySqlDataReader_Parameter = mySqlCommand_Parameter.ExecuteReader();
                                    sqlConnection.Close();
                                    #endregion
                                }

                            }

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetFukUp = package.Workbook.Worksheets["FUK - UP"];
                                int totalRowsFukUp = workSheetFukUp.Dimension.Rows;
                                for (int i = 2; i <= totalRowsFukUp; i++)
                                {
                                    var score = "0.613";
                                    var cell4 = "";
                                    cell4 = !String.IsNullOrEmpty(workSheetFukUp.Cells[i, 4].Value.ToString()) ? workSheetFukUp.Cells[i, 4].Value.ToString() : "-";

                                    fukup.Add(new FUKUP
                                    {
                                        NoParameter = workSheetFukUp.Cells[i, 1].Value.ToString(),
                                        Seq = workSheetFukUp.Cells[i, 2].Value.ToString(),
                                        FUK = workSheetFukUp.Cells[i, 3].Value.ToString(),
                                        UP = cell4
                                    });

                                    #region
                                    var query_fuk =
                                           "INSERT INTO [tbl_M_FUK] " +
                                           "(" +
                                           " [ID_PARAMETER] " +
                                           ",[SEQ]    " +
                                           ",[PARENT]" +
                                           ",[FUKDESC]" +
                                           ",[BULLETNUMBER]" +
                                           ",[STATUS]" +
                                           //",[SCORE]" +
                                           ",[REMARKS]" +
                                           ",[CREATEDBY]" +
                                           ",[CREATEDDATE] " +
                                           ",[LASTUPDATEDBY] " +
                                           ",[LASTUPDATEDDATE]" +
                                           " )" +
                                           "VALUES  ( " +
                                           " '" + 1058 + "' " +
                                           ", '" + workSheetFukUp.Cells[i, 2].Value.ToString() + "' " +
                                           ", '" + 0 + "' " +
                                           ", '" + workSheetFukUp.Cells[i, 3].Value.ToString() + "' " +
                                           ", 'a' " +
                                           ", NULL " +
                                           //", " + Convert.ToDecimal(score) + " " +
                                           ", NULL " +
                                           ", 'System' " +
                                           ", getdate() " +
                                           ", 'System' " +
                                           ", getdate() " +
                                           ") ";

                                    var mySqlCommand_Fuk = new SqlCommand
                                    {
                                        Connection = sqlConnection,
                                        CommandText = query_fuk,
                                    };
                                    sqlConnection.Open();
                                    var mySqlDataReader_Fuk = mySqlCommand_Fuk.ExecuteReader();
                                    sqlConnection.Close();

                                    var query_up =
                                          "INSERT INTO [tbl_M_UnsurPemenuhan] " +
                                          "(" +
                                          " [ID_FUK] " +
                                          ",[SEQ]    " +
                                          ",[UPDESC]" +
                                          ",[STATUS]" +
                                          //",[SCORE]" +
                                          ",[CREATEDBY]" +
                                          ",[CREATEDDATE] " +
                                          ",[LASTUPDATEDBY] " +
                                          ",[LASTUPDATEDDATE]" +
                                          " )" +
                                          "VALUES  ( " +
                                          " '" + 1024 + "' " +
                                          ", '" + workSheetFukUp.Cells[i, 2].Value.ToString() + "' " +
                                          ", '" + workSheetFukUp.Cells[i, 4].Value.ToString() + "' " +
                                          ", NULL " +
                                          //", " + Convert.ToDecimal(score) + " " +
                                          ", 'System' " +
                                          ", getdate() " +
                                          ", 'System' " +
                                          ", getdate() " +
                                          ") ";

                                    var mySqlCommand_Up = new SqlCommand
                                    {
                                        Connection = sqlConnection,
                                        CommandText = query_up,
                                    };
                                    sqlConnection.Open();
                                    var mySqlDataReader_Up = mySqlCommand_Up.ExecuteReader();
                                    sqlConnection.Close();
                                    #endregion
                                }
                            }
                            return Json(new
                            {
                                Success = true,
                                Data = new
                                {
                                    Message = "Data has been inserted successfully",
                                    Status = true
                                }
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

                return Json(new
                {
                    Success = false,
                    Message = String.Empty
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
        [Route("cloning-data")] 
        public JsonResult CloningData()
        //public JsonResult CloningData(string year)
        {
            try
            {
                //string yr = year;
                return Json(new
                {
                    Success = true,
                    Data = new
                    {
                        Year = "2022",
                        Status = true
                    }
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

        private string SaveFile(IFormFile file)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string wwwPath = _environment.WebRootPath;
                    string contentPath = _environment.ContentRootPath;

                    string pathDocument = Path.Combine(wwwPath, "Documents");
                    if (!Directory.Exists(pathDocument))
                    {
                        Directory.CreateDirectory(pathDocument);
                    }
                    // Getting FileName
                    string fileName = Path.GetFileName(file.FileName);
                    string docName = fileName.Split(".")[0];
                    // Getting File Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // Make TimeStamp
                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                    // Custom File Name
                    docName = docName + "-" + timeStamp + fileExtension;
                    using (FileStream stream = new FileStream(Path.Combine(pathDocument, docName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return docName;
                }
            }

            return null;
        }
    }

    public class IndikatorParameter
    {
        public string IndikatorCategory { get; set; }
        public string NoParameter { get; set; }
        public string Parameter { get; set; }
        public decimal Bobot { get; set; }
    }

    public class FUKUP
    {
        public string NoParameter { get; set; }
        public string Seq { get; set; }
        public string FUK { get; set; }
        public string UP { get; set; }
    }

    //public class YearParam
    //{
    //    public int Id { get; set; }
    //}
}


