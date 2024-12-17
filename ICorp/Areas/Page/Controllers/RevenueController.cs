using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using OfficeOpenXml;
using InventoryIT.Areas.Master.Models;
using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Data;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;
using System.Threading.Channels;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/revenue")]
    public class RevenueController : Controller
    {

        private readonly PlanCorpDbContext _context;
        private readonly IRevenueService _revenueService;
        private readonly ConnectionDB _connectionDB;

        public RevenueController(PlanCorpDbContext context, IRevenueService revenueService, ConnectionDB connectionDB)
        {
            _context = context;
            _revenueService = revenueService;
            _connectionDB = connectionDB;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("edit")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        [Route("amount")]
        public IActionResult Amount()
        {
            return View();
        }

        [HttpGet]
        [Route("amount-input")]
        public IActionResult AmountInput()
        {
            return View();
        }

        [HttpPost]
        [Route("get-header-revenue")]
        public JsonResult GetDataHeaderRevenue()
        {
            try
            {
                var list = _revenueService.GetAllHeader();
                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data
                });

            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("create-header-ajax")]
        public JsonResult CreateHeader([FromBody] HeaderRevenue param)
        {
            try
            {
                param.CreatedBy = HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
                {
                    throw new InvalidOperationException("Username is not found in session.");
                }
                param.CreatedTime = DateTime.Now;
                var r = _revenueService.SaveOrUpdate(param);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
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
        [Route("get-project-revenue")]
        public JsonResult GetDataProjectRevenue(int idHeader)
        {
            Debug.WriteLine("id Headernya"+idHeader);
            if (idHeader <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid idHeader value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetProjectRevenue(idHeader);
                var headerEdit = _revenueService.GetDetailHeaderRevenue(idHeader);
                return Json(new
                {
                    Success = true,
                    Data = new
                    {
                        list = list.Result.Data,
                        headerEdit = headerEdit.Result.Data,
                    }
                    //Data = list.Result.Data
                });

            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }


        [HttpGet]
        [Route("get-projectDD-ajax")]
        public JsonResult GetProjectDD()
        {
            try
            {
                var list = _revenueService.GetProjectExist();

                return Json(new
                {
                    Success = true,
                    Data = list
                });
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);

                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }

        [HttpPost]
        [Route("get-project-exist")]
        public JsonResult GetProjectExistByID(int ProjectID)
        {
            Debug.WriteLine("id Projectnya" + ProjectID);
            if (ProjectID <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid idProject value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetProjectExistById(ProjectID);
                
                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,
            
                });

            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }

        [HttpPost]
        [Route("get-costcenter-fill")]
        public JsonResult GetCostCenterFill(int AssetID)
        {
            Debug.WriteLine("id Projectnya" + AssetID);
            if (AssetID <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid ID Asset value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetCostCenterFill(AssetID);

                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,

                });

            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }

        [HttpPost]
        [Route("get-regional-fill")]
        public JsonResult GetRegionalFill(int CustomerID)
        {
            Debug.WriteLine("id Projectnya" + CustomerID);
            if (CustomerID <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid ID Customer value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetRegionalFill(CustomerID);

                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,

                });

            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("create-mapping-project")]
        public JsonResult CrateMappingProject([FromBody] MappingProjectRevenue param)
        {
            try
            {
                param.CreatedBy = HttpContext.Session.GetString("username");
                var r = _revenueService.SaveMappingProject(param);
                return Json(new
                {
                    Success = r.Success,
                    Message = r.Message
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
        [Route("get-detail-revenue")]
        public JsonResult GetDetailRevenue(int idHeader)
        {
            Debug.WriteLine("id Headernya" + idHeader);
            if (idHeader <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Invalid idHeader value di Controller."
                });
            }

            try
            {
                var list = _revenueService.GetDetailRevenue(idHeader);
                return Json(new
                {
                    Success = true,
                    Data = list.Result.Data,
             
                });

            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
                return Json(new
                {
                    Success = false,
                    Message = ex.InnerException
                });
            }
        }

        [HttpPost]
        [Route("upload-project-ajax")]
        public async Task<JsonResult> UploadProjectAjax(IFormFile FileUpload, int IDHeader)
        {
            List<ImportLog> importLogs = new List<ImportLog>();
            try
            {
                if (FileUpload != null && FileUpload.Length > 0 && IDHeader != 0)
                {
                    var stream = FileUpload.OpenReadStream();
                    

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheetProject = package.Workbook.Worksheets["Input Data Project"];
                        int totalRowsProject = workSheetProject.Dimension.Rows;
                        using(IDbConnection conn = _connectionDB.Connection)
                        {
                            conn.Open();
                            for (int i = 2; i <= totalRowsProject; i++)
                            {
                                int idHeader = IDHeader;
                                string? namaProject = workSheetProject.Cells[i, 2]?.Value?.ToString();
                                string? pekerjaan = workSheetProject.Cells[i, 3]?.Value?.ToString();
                                string? sumur = workSheetProject.Cells[i, 4]?.Value?.ToString();
                                string? controlProject = workSheetProject.Cells[i, 5]?.Value?.ToString();
                                string? probability = workSheetProject.Cells[i, 6]?.Value?.ToString();
                                string? segmen = workSheetProject.Cells[i, 7]?.Value?.ToString();
                                string? asset = workSheetProject.Cells[i, 8]?.Value?.ToString();
                                string? customer = workSheetProject.Cells[i, 9]?.Value?.ToString();
                                string? contract = workSheetProject.Cells[i, 10]?.Value?.ToString();
                                string? sbt = workSheetProject.Cells[i, 11]?.Value?.ToString();
                                if ( string.IsNullOrEmpty(namaProject) 
                                    && string.IsNullOrEmpty(pekerjaan) 
                                    && string.IsNullOrEmpty(sumur) 
                                    && string.IsNullOrEmpty(controlProject) 
                                    && string.IsNullOrEmpty(probability)
                                    && string.IsNullOrEmpty(segmen)
                                    && string.IsNullOrEmpty(asset)
                                    && string.IsNullOrEmpty(customer)
                                    && string.IsNullOrEmpty(contract)
                                    && string.IsNullOrEmpty(sbt)
                                ) continue;
                            //int idProject = 0;

                            var log = new ImportLog
                                {
                                    RowNumber = i,
                                    NamaProject = namaProject
                                };

                                try
                                {
                                    // Step 1: Upsert Project
                                    var projectParams = new DynamicParameters();
                                    projectParams.Add("@IDHeader", IDHeader);
                                    projectParams.Add("@NamaProject", namaProject);
                                    projectParams.Add("@Pekerjaan", pekerjaan);
                                    projectParams.Add("@Sumur", sumur);
                                    projectParams.Add("@ControlProject", controlProject);
                                    projectParams.Add("@Probability", probability);
                                    projectParams.Add("@Segmen", segmen);
                                    projectParams.Add("@Asset", asset);
                                    projectParams.Add("@Customer", customer);
                                    projectParams.Add("@Contract", contract);
                                    projectParams.Add("@SBT", sbt);
                                    projectParams.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                                    
              

                                    conn.Execute("usp_Post_ImportProjectMapping", projectParams, commandType: CommandType.StoredProcedure);
                                    //idProject = projectParams.Get<int>("@IDProject");

                                    // Step 2: Insert Mapping
                                    //var mappingParams = new DynamicParameters();
                                    //mappingParams.Add("@IDHeader", IDHeader);
                                    //mappingParams.Add("@IDProject", idProject);
                                    //

                                    //conn.Execute("usp_InsertMapping", mappingParams, commandType: CommandType.StoredProcedure);
                                    bool success = projectParams.Get<bool>("@Success");

                                    // Logging results
                                    log.Status = success ? "Success" : "Duplicate";
                                    log.Message = success
                                        ? $"Project '{namaProject}' successfully mapped."
                                        : $"Project '{namaProject}' mapping already exists.";
                                }
                                catch( Exception ex ) 
                                {
                                    log.Status = "Error";
                                    log.Message = $"Error processing project '{namaProject}': {ex.Message}";
                                }
                                importLogs.Add(log);

                            }
                        }

                    }
                    return Json(new
                    {
                        Success = true,
                        Message = "Processing completed.",
                        Logs = importLogs
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "File not found or empty.",
                    Logs = importLogs
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
        [Route("save-change-amount")]
        public async Task<JsonResult> SaveAmountRevenue([FromBody] List<AmountRevenue> changes)
        {
            List<AmountLog> importLogs = new List<AmountLog>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    foreach (var change in changes)
                    {
                        int idProject = change.Project;
                        int idHeader = change.IDHeader;

                        foreach (var columnChange in change.Changes)
                        {
                            string month = columnChange.Key;
                            decimal value = columnChange.Value;
                            decimal valueUsd = value/15000;

                            var log = new AmountLog
                            {
                                idProject = idProject,
                                Month = month
                            };

                            try
                            {
                                // Step 1: Upsert Project
                                var projectParams = new DynamicParameters();
                                projectParams.Add("@pIDHeader", idHeader);
                                projectParams.Add("@pIDProject", idProject);
                                projectParams.Add("@pMonth", month);
                                projectParams.Add("@pAmount", value);
                                projectParams.Add("@pAmountUSD", valueUsd);
                                projectParams.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                conn.Execute("usp_Post_AmountRevenue", projectParams, commandType: CommandType.StoredProcedure);
                                
                                bool success = projectParams.Get<bool>("@Success");

                                // Logging results
                                log.Status = success ? "Success" : "Failed";
                                log.Message = success
                                    ? $"Project '{idProject}' successfully update."
                                    : $"Project '{idProject}' failed to update.";
                            }
                            catch (Exception ex)
                            {
                                log.Status = "Error";
                                log.Message = $"Error processing project '{idProject}': {ex.Message}";
                            }
                            importLogs.Add(log);
                        }  
                    }
                }

                return Json(new
                {
                    Success = true,
                    Message = "Processing completed.",
                    Logs = importLogs
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
        [Route("delete-header-revenue")]
        public JsonResult DeleteHeaderRevenue(int IDHeader)
        {
            try
            {
                var result = _revenueService.DeleteHeaderRevenue(IDHeader);
                return Json(new
                {
                    Success = result.Success,
                    Message = result.Message
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
        [Route("delete-project-mapping")]
        public JsonResult DeleteProjectMapping(int IDHeader, int IDProject)
        {
            Debug.WriteLine("id Headernya " + IDHeader + " " + IDProject);
            try
            {
                var result = _revenueService.DeleteProjectMapping(IDHeader, IDProject);
                return Json(new
                {
                    Success = result.Success,
                    Message = result.Message
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
