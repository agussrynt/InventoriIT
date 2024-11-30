using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using OfficeOpenXml;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Areas.Page.Services;
using PlanCorp.Data;
using System.Data.SqlClient;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/revhppga")]
    public class RevHPPGAController : Controller
    {
        private readonly PlanCorpDbContext _context;
        private readonly IRevenue_RJPP_Service _revenue_RJPP_Service;
        private SqlConnection sqlConnection;

        public RevHPPGAController(PlanCorpDbContext context, IRevenue_RJPP_Service revenue_RJPP_Service, IConfiguration _configuration)
        {
            _context = context;
            _revenue_RJPP_Service = revenue_RJPP_Service;
            sqlConnection = new SqlConnection(_configuration.GetConnectionString("PlanCorpDbContextConnection"));
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("get-all-revhppga")]
        public JsonResult GetData_Revenue_HPP_GA()
        {
            try
            {
                var list = _revenue_RJPP_Service.GetAllRevenue();

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

        [AllowAnonymous]
        [HttpPost]
        [Route("get-sum-revhppga")]
        public JsonResult GetData_SUMRevHPPGA()
        {
            try
            {
                var list = _revenue_RJPP_Service.GetSumRevHPPGA();

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

        [Route("importRevHPPGA")]
        public IActionResult ImportRevHPPGA()
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
                            List<RevHPPGADetail> revHPPGADetails = new List<RevHPPGADetail>();
                            List<Revenue_RJPP> revenue = new List<Revenue_RJPP>();
                            List<HPP_RJPP> hpprev = new List<HPP_RJPP>();

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetRev = package.Workbook.Worksheets["RevHPPGA"];
                                if (workSheetRev != null)
                                {
                                    int totalRowsRev = workSheetRev.Dimension.Rows;
                                    for (int i = 3; i <= totalRowsRev; i++)
                                    {
                                        if (workSheetRev.Cells[i, 1].Value == null &&
                                            workSheetRev.Cells[i, 2].Value == null &&
                                            workSheetRev.Cells[i, 3].Value == null)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            revHPPGADetails.Add(new RevHPPGADetail
                                            {
                                                SegmentRJPP = workSheetRev.Cells[i, 1].Value.ToString(),
                                                NamaCostCenter = workSheetRev.Cells[i, 2].Value.ToString(),
                                                HP = workSheetRev.Cells[i, 3].Value.ToString(),
                                                UniqueCode = workSheetRev.Cells[i, 4].Value.ToString(),
                                                KategoriRIG = workSheetRev.Cells[i, 5].Value.ToString(),
                                                PIC = workSheetRev.Cells[i, 6].Value.ToString(),
                                                Costumer = workSheetRev.Cells[i, 7].Value.ToString(),
                                                Project = workSheetRev.Cells[i, 8].Value.ToString(),
                                                HPPSales = workSheetRev.Cells[i, 9].Value.ToString(),
                                                GASales = workSheetRev.Cells[i, 10].Value.ToString()
                                            });
                                        }

                                        for (int j = 11; j <= 21; j++)
                                        {
                                            if (workSheetRev.Cells[i, 11].Value == null &&
                                            workSheetRev.Cells[i, 12].Value == null &&
                                            workSheetRev.Cells[i, 13].Value == null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                revenue.Add(new Revenue_RJPP
                                                {
                                                    Tahun = Convert.ToInt32(workSheetRev.Cells[2, j].Value.ToString()),
                                                    Revenue = Convert.ToInt32(workSheetRev.Cells[i, j].Value.ToString())
                                                });
                                            }
                                        }

                                        for (int k = 22; k <= 32; k++)
                                        {
                                            if (workSheetRev.Cells[i, 11].Value == null &&
                                            workSheetRev.Cells[i, 12].Value == null &&
                                            workSheetRev.Cells[i, 13].Value == null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                hpprev.Add(new HPP_RJPP
                                                {
                                                    Tahun = Convert.ToInt32(workSheetRev.Cells[2, k].Value.ToString()),
                                                    HPP = Convert.ToInt32(workSheetRev.Cells[i, k].Value.ToString()),
                                                    TotalHPP = Convert.ToInt32(workSheetRev.Cells[i, k].Value.ToString())
                                                });
                                            }
                                        }
                                    }
                                }
                                else {
                                    return Json(new
                                    {
                                        Success = false,
                                        Message = "Template Excell yang diupload salah. Harap Periksa Template Excell yang diUpload"
                                    });
                                }
                                
                            }

                            return Json(new
                            {
                                Success = true,
                                Data = new
                                {
                                    Detail = revHPPGADetails,
                                    Revenue = revenue,
                                    Hpp = hpprev,
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
        public JsonResult NewUploadAjax(IFormFile FileUpload, string IDRev)
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
                            List<RevHPPGADetail> revHPPGADetails = new List<RevHPPGADetail>();
                            List<Revenue_RJPP> revenue = new List<Revenue_RJPP>();
                            List<HPP_RJPP> hpprev = new List<HPP_RJPP>();

                            List<int> insertedIds = new List<int>();
                            var years = new List<int>();

                            int detailRow = 3; // Baris mulai untuk data detail

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetRev = package.Workbook.Worksheets["RevHPPGA"];
                                int totalRowsRev = workSheetRev.Dimension.Rows;
                                int totalColumnRev = workSheetRev.Dimension.Columns;

                                // Convert list of IDs to comma-separated string
                                string idArray = string.Join(",", IDRev);

                                if (idArray != "")
                                {
                                    _revenue_RJPP_Service.DeleteExistingData(idArray);
                                }

                                #region INSERT DETAIL

                                for (int i = detailRow; i <= totalRowsRev; i++)
                                {
                                    if (workSheetRev.Cells[i, 1].Value == null) break;

                                    var detailRJPP = new RevHPPGADetail
                                    {
                                        SegmentRJPP = workSheetRev.Cells[i, 1].Value.ToString(),
                                        NamaCostCenter = workSheetRev.Cells[i, 2].Value.ToString(),
                                        HP = workSheetRev.Cells[i, 3].Value.ToString(),
                                        UniqueCode = workSheetRev.Cells[i, 4].Value.ToString(),
                                        KategoriRIG = workSheetRev.Cells[i, 5].Value.ToString(),
                                        PIC = workSheetRev.Cells[i, 6].Value.ToString(),
                                        Costumer = workSheetRev.Cells[i, 7].Value.ToString(),
                                        Project = workSheetRev.Cells[i, 8].Value.ToString(),
                                        HPPSales = workSheetRev.Cells[i, 9].Value.ToString(),
                                        GASales = workSheetRev.Cells[i, 10].Value.ToString(),
                                        CreatedBy = "SYSTEM"
                                    };

                                    string strSegment = workSheetRev.Cells[i, 1].Value.ToString();

                                    if (strSegment != "")
                                    {
                                        var postDetailRJPP = _revenue_RJPP_Service.InsertDetail(detailRJPP);
                                        if (postDetailRJPP.Success != false)
                                        {
                                            int insertedId = Convert.ToInt32(postDetailRJPP.Message);
                                            insertedIds.Add(insertedId);  // Simpan ID yang dihasilkan
                                        }
                                    }
                                }

                                #endregion INSERT DETAIL

                                #region INSERT REVENUE

                                // Baca data revenue
                                for (int row = 3; row <= totalRowsRev; row++)
                                {
                                    if (workSheetRev.Cells[row, 11].Value == null) break;

                                    for (int col = 11; col <= 21; col++)
                                    {
                                        var revenueRJPP = new Revenue_RJPP
                                        {
                                            Id_DetailRevHPPGA = insertedIds[(row - 3) % insertedIds.Count],
                                            Revenue = Convert.ToInt32(workSheetRev.Cells[row, col].Value.ToString()),
                                            Tahun = Convert.ToInt32(workSheetRev.Cells[2, col].Value.ToString()),
                                            CreatedBy = "SYSTEM"
                                        };

                                        _revenue_RJPP_Service.InsertRevenue(revenueRJPP);
                                    }
                                }

                                #endregion INSERT REVENUE

                                #region INSERT HPP

                                for (int row = 3; row <= totalRowsRev; row++)
                                {
                                    if (workSheetRev.Cells[row, 22].Value == null) break;

                                    for (int z = 22; z <= 32; z++)
                                    {
                                        var HPPRJPP = new HPP_RJPP
                                        {
                                            Id_DetailRevHPPGA = insertedIds[(row - 3) % insertedIds.Count],
                                            HPP = Convert.ToInt32(workSheetRev.Cells[row, z].Value.ToString()),
                                            Tahun = Convert.ToInt32(workSheetRev.Cells[2, z].Value.ToString()),
                                            CreatedBy = "SYSTEM"
                                        };

                                        _revenue_RJPP_Service.InsertHPP(HPPRJPP);
                                    }
                                }

                                #endregion INSERT HPP
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
                    Message = "Gagal Submit, File Gagal Baca"
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
        [Route("edit-upload-ajax")]
        public JsonResult EditUploadAjax(IFormFile FileUpload)
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
                            List<RevHPPGADetail> revHPPGADetails = new List<RevHPPGADetail>();
                            List<Revenue_RJPP> revenue = new List<Revenue_RJPP>();
                            List<HPP_RJPP> hpprev = new List<HPP_RJPP>();

                            List<int> insertedIds = new List<int>();
                            var years = new List<int>();

                            int detailRow = 3; // Baris mulai untuk data detail

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet workSheetRev = package.Workbook.Worksheets["RevHPPGA"];
                                int totalRowsRev = workSheetRev.Dimension.Rows;
                                int totalColumnRev = workSheetRev.Dimension.Columns;

                                #region INSERT DETAIL

                                for (int i = detailRow; i <= totalRowsRev; i++)
                                {
                                    if (workSheetRev.Cells[i, 1].Value == null) break;

                                    var detailRJPP = new RevHPPGADetail
                                    {
                                        SegmentRJPP = workSheetRev.Cells[i, 1].Value.ToString(),
                                        NamaCostCenter = workSheetRev.Cells[i, 2].Value.ToString(),
                                        HP = workSheetRev.Cells[i, 3].Value.ToString(),
                                        UniqueCode = workSheetRev.Cells[i, 4].Value.ToString(),
                                        KategoriRIG = workSheetRev.Cells[i, 5].Value.ToString(),
                                        PIC = workSheetRev.Cells[i, 6].Value.ToString(),
                                        Costumer = workSheetRev.Cells[i, 7].Value.ToString(),
                                        Project = workSheetRev.Cells[i, 8].Value.ToString(),
                                        HPPSales = workSheetRev.Cells[i, 9].Value.ToString(),
                                        GASales = workSheetRev.Cells[i, 10].Value.ToString(),
                                        CreatedBy = "SYSTEM"
                                    };

                                    string strSegment = workSheetRev.Cells[i, 1].Value.ToString();

                                    if (strSegment != "")
                                    {
                                        var postDetailRJPP = _revenue_RJPP_Service.InsertDetail(detailRJPP);
                                        if (postDetailRJPP.Success != false)
                                        {
                                            int insertedId = Convert.ToInt32(postDetailRJPP.Message);
                                            insertedIds.Add(insertedId);  // Simpan ID yang dihasilkan
                                        }
                                    }
                                }

                                #endregion INSERT DETAIL

                                #region INSERT REVENUE

                                // Baca data revenue
                                for (int row = 3; row <= totalRowsRev; row++)
                                {
                                    if (workSheetRev.Cells[row, 11].Value == null) break;

                                    for (int col = 11; col <= 21; col++)
                                    {
                                        var revenueRJPP = new Revenue_RJPP
                                        {
                                            Id_DetailRevHPPGA = insertedIds[(row - 3) % insertedIds.Count],
                                            Revenue = Convert.ToInt32(workSheetRev.Cells[row, col].Value.ToString()),
                                            Tahun = Convert.ToInt32(workSheetRev.Cells[2, col].Value.ToString()),
                                            CreatedBy = "SYSTEM"
                                        };

                                        _revenue_RJPP_Service.InsertRevenue(revenueRJPP);
                                    }
                                }

                                #endregion INSERT REVENUE

                                #region INSERT HPP

                                for (int row = 3; row <= totalRowsRev; row++)
                                {
                                    if (workSheetRev.Cells[row, 22].Value == null) break;

                                    for (int z = 22; z <= 32; z++)
                                    {
                                        var HPPRJPP = new HPP_RJPP
                                        {
                                            Id_DetailRevHPPGA = insertedIds[(row - 3) % insertedIds.Count],
                                            HPP = Convert.ToInt32(workSheetRev.Cells[row, z].Value.ToString()),
                                            Tahun = Convert.ToInt32(workSheetRev.Cells[2, z].Value.ToString()),
                                            CreatedBy = "SYSTEM"
                                        };

                                        _revenue_RJPP_Service.InsertHPP(HPPRJPP);
                                    }
                                }

                                #endregion INSERT HPP
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
                    Message = "Gagal Submit, File Gagal Baca"
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