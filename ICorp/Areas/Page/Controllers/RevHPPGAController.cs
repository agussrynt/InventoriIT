using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Services;
using PlanCorp.Data;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/rev-hpp-ga")]
    public class RevHPPGAController : Controller
    {
        private readonly PlanCorpDbContext _context;
        private readonly IRevHPPGAService _revHPPGAService;

        public RevHPPGAController(PlanCorpDbContext context, IRevHPPGAService revHPPGAService)
        {
            _context = context;
            _revHPPGAService = revHPPGAService;
        }

        public IActionResult Index()
        {
            return View();
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
                                    if (workSheetIndikator.Cells[i, 1].Value == null &&
                                        workSheetIndikator.Cells[i, 2].Value == null &&
                                        workSheetIndikator.Cells[i, 3].Value == null)
                                    {
                                        break;
                                    }
                                    else
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
                            }

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetFukUp = package.Workbook.Worksheets["FUK - UP"];
                                int totalRowsFukUp = workSheetFukUp.Dimension.Rows;
                                for (int i = 2; i <= totalRowsFukUp; i++)
                                {
                                    var score = "0.613";
                                    var cell4 = "-";
                                    if (workSheetFukUp.Cells[i, 4].Value != null)
                                    {
                                        //cell4 = !String.IsNullOrEmpty(workSheetFukUp.Cells[i, 4].Value.ToString()) ? workSheetFukUp.Cells[i, 4].Value.ToString() : "-";
                                        if (string.IsNullOrEmpty(workSheetFukUp.Cells[i, 4].Value.ToString()))
                                        {
                                            cell4 = "-";
                                        }
                                        else
                                        {
                                            cell4 = workSheetFukUp.Cells[i, 4].Value.ToString();
                                        }
                                    }

                                    if (workSheetFukUp.Cells[i, 1].Value == null &&
                                        workSheetFukUp.Cells[i, 2].Value == null &&
                                        workSheetFukUp.Cells[i, 3].Value == null)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        fukup.Add(new FUKUP
                                        {
                                            NoParameter = workSheetFukUp.Cells[i, 1].Value == null ? "-" : workSheetFukUp.Cells[i, 1].Value.ToString(),
                                            Seq = workSheetFukUp.Cells[i, 2].Value == null ? "-" : workSheetFukUp.Cells[i, 2].Value.ToString(),
                                            FUK = workSheetFukUp.Cells[i, 3].Value == null ? "-" : workSheetFukUp.Cells[i, 3].Value.ToString(),
                                            UP = cell4
                                        });
                                    }
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
        [Route("new-upload-ajax")]
        public JsonResult NewUploadAjax(IFormFile FileUpload)
        {
            try
            {
                string strIdYear = HttpContext.Session.GetString("IdYear");
                if (FileUpload != null)
                {
                    if (FileUpload.Length > 0)
                    {
                        var stream = FileUpload.OpenReadStream();
                        try
                        {
                            List<IndikatorParameter> indikator = new List<IndikatorParameter>();
                            List<FUKUP> fukup = new List<FUKUP>();

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetIndikator = package.Workbook.Worksheets["Indikator - Parameter"];
                                ExcelWorksheet workSheetFukUp = package.Workbook.Worksheets["FUK - UP"];
                                int totalRowsIndikator = workSheetIndikator.Dimension.Rows;
                                int totalRowsFukUp = workSheetFukUp.Dimension.Rows;
                                string compIndikator = "";
                                int incIndikator = 0;
                                int IdIndikator = 0;

                                for (int i = 2; i <= totalRowsIndikator; i++)
                                {
                                    if (workSheetIndikator.Cells[i, 1].Value == null &&
                                        workSheetIndikator.Cells[i, 2].Value == null &&
                                        workSheetIndikator.Cells[i, 3].Value == null)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        indikator.Add(new IndikatorParameter
                                        {
                                            IndikatorCategory = workSheetIndikator.Cells[i, 1].Value.ToString(),
                                            NoParameter = workSheetIndikator.Cells[i, 2].Value.ToString(),
                                            Parameter = workSheetIndikator.Cells[i, 3].Value.ToString(),
                                            Bobot = Convert.ToDecimal(workSheetIndikator.Cells[i, 4].Value.ToString())
                                        });
                                    }

                                    string strIndikator = workSheetIndikator.Cells[i, 1].Value.ToString();
                                    string strParameter = workSheetIndikator.Cells[i, 2].Value.ToString();

                                    if (compIndikator != strIndikator)
                                    {
                                        compIndikator = strIndikator;
                                        incIndikator++;

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
                                               " '" + IdYear + "' " +
                                               ", '" + incIndikator + "' " +
                                               ", '" + workSheetIndikator.Cells[i, 1].Value.ToString() + "' " +
                                               ", NULL " +
                                               ", 'System' " +
                                               ", getdate() " +
                                               ", 'System' " +
                                               ", getdate() " +
                                               "); " +
                                               "SELECT SCOPE_IDENTITY();";

                                        sqlConnection.Open();
                                        using (SqlCommand mySqlCommand = new SqlCommand(query_indikator, sqlConnection))
                                        {
                                            // Execute the query and get the inserted ID
                                            var result = mySqlCommand.ExecuteScalar(); // Use ExecuteScalar to get the ID directly

                                            if (result != null)
                                            {
                                                IdIndikator = Convert.ToInt32(result);
                                                Console.WriteLine("Inserted Record ID: " + IdIndikator);
                                            }
                                            else
                                            {
                                                Console.WriteLine("No ID was returned from the query.");
                                            }
                                        }
                                        sqlConnection.Close();
                                    }

                                    var query_parameter =
                                        " INSERT INTO [dbo].[tbl_M_Parameter] " +
                                        " (" +
                                        " [ID_INDICATOR]" +
                                        ",[ID_PARAM_EXCEL]" +
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
                                        " '" + IdIndikator + "' " + //cek cara get last record
                                        ",'" + strParameter + "' " +
                                        ",'" + workSheetIndikator.Cells[i, 2].Value.ToString() + "' " +
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
                                int noParam = 0;
                                int IdFUK = 0;

                                for (int i = 2; i <= totalRowsFukUp; i++)
                                {
                                    var score = "0.613";
                                    var cell4 = "";
                                    if (workSheetFukUp.Cells[i, 4].Value != null)
                                    {
                                        //cell4 = !String.IsNullOrEmpty(workSheetFukUp.Cells[i, 4].Value.ToString()) ? workSheetFukUp.Cells[i, 4].Value.ToString() : "-";
                                        if (string.IsNullOrEmpty(workSheetFukUp.Cells[i, 4].Value.ToString()))
                                        {
                                            cell4 = "-";
                                        }
                                        else
                                        {
                                            cell4 = workSheetFukUp.Cells[i, 4].Value.ToString();
                                        }
                                    }

                                    if (workSheetFukUp.Cells[i, 1].Value == null &&
                                        workSheetFukUp.Cells[i, 2].Value == null &&
                                        workSheetFukUp.Cells[i, 3].Value == null)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        fukup.Add(new FUKUP
                                        {
                                            NoParameter = workSheetFukUp.Cells[i, 1].Value == null ? "-" : workSheetFukUp.Cells[i, 1].Value.ToString(),
                                            Seq = workSheetFukUp.Cells[i, 2].Value == null ? "-" : workSheetFukUp.Cells[i, 2].Value.ToString(),
                                            FUK = workSheetFukUp.Cells[i, 3].Value == null ? "-" : workSheetFukUp.Cells[i, 3].Value.ToString(),
                                            UP = cell4
                                        });
                                    }

                                    var listIDParameter = _masterKertasKerjaService.GetIDParameterByNoParamAndYears(Int32.Parse(workSheetFukUp.Cells[i, 1].Value.ToString()), IdYear);

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
                                           " '" + listIDParameter[0].IdParameter + "' " +
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
                                           "); " +
                                           "SELECT SCOPE_IDENTITY();";

                                    sqlConnection.Open();
                                    using (SqlCommand mySqlCommand_Fuk = new SqlCommand(query_fuk, sqlConnection))
                                    {
                                        // Execute the query and get the inserted ID
                                        var result = mySqlCommand_Fuk.ExecuteScalar(); // Use ExecuteScalar to get the ID directly

                                        if (result != null)
                                        {
                                            IdFUK = Convert.ToInt32(result);
                                            Console.WriteLine("Inserted Record ID: " + IdFUK);
                                        }
                                        else
                                        {
                                            Console.WriteLine("No ID was returned from the query.");
                                        }
                                    }
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
                                          " '" + IdFUK + "' " +
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
                    Message = "Gagal Submit, File Gagal Baca dan Tahun"
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