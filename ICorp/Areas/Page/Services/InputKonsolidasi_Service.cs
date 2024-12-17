using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using InventoryIT.Areas.Master.Controllers;
using InventoryIT.Areas.Master.Models;
using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Data;
using InventoryIT.Models;
using System.Data;

namespace InventoryIT.Areas.Page.Services
{
    public class InputKonsolidasi_Service : IInput_Konsolidasi_Service
    {
        private PlanCorpDbContext _dbContext;
        private readonly ConnectionDB _connectionDB;

        public InputKonsolidasi_Service(PlanCorpDbContext dbContext, ConnectionDB connectionDB)
        {
            _dbContext = dbContext;
            _connectionDB = connectionDB;
        }


        public async Task<(List<PendapatanUsaha_Revenue_RJPP> pendapatan, List<BebanUsaha_HPP_RJPP> beban, List<BebanUmum_Administrasi_RJPP> administrasi)> GetAll_InputKonsolidasi()
        {
            using (IDbConnection conn = _connectionDB.Connection)
            {
                try
                {
                    conn.Open();

                    var pendapatan = await conn.QueryAsync<PendapatanUsaha_Revenue_RJPP>("SELECT * FROM [dbo].[TblT_RJPP_PendapatanUsaha_Revenue]");
                    var beban = await conn.QueryAsync<BebanUsaha_HPP_RJPP>("SELECT * FROM [dbo].[TblT_RJPP_BebanUsaha_HPP]");
                    var administrasi = await conn.QueryAsync<BebanUmum_Administrasi_RJPP>("SELECT * FROM [dbo].[TblT_RJPP_BebanUmum_Administrasi]");

                    return (pendapatan.ToList(), beban.ToList(), administrasi.ToList());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return (new List<PendapatanUsaha_Revenue_RJPP>(), new List<BebanUsaha_HPP_RJPP>(), new List<BebanUmum_Administrasi_RJPP>());
                }
            }
        }

        public async Task<(List<PendapatanUsaha_Revenue_RJPP> TahunP, List<BebanUsaha_HPP_RJPP> TahunB)> GetSub_InputKonsolidasi(int[] tahunArrayP, int[] tahunArrayB)
        {
            using (IDbConnection conn = _connectionDB.Connection)
            {
                try
                {
                    conn.Open();

                    var tahunPTable = ConvertArrayToDataTable(tahunArrayP);
                    var tahunBTable = ConvertArrayToDataTable(tahunArrayB);

                    //1 = Rev, 2 = hpp, 3 = GA
                    //int category;

                    //var tahunArrayTable = ConvertArrayToDataTable(tahunArray);

                    var tahunPData = await conn.QueryAsync<PendapatanUsaha_Revenue_RJPP>(
                        "[dbo].[usp_Get_subData_Total_PDSI_RevHPPGA]",
                        new {
                            TahunArray = tahunPTable.AsTableValuedParameter("TahunTableType"),
                            category   = 1
                            },
                        commandType: CommandType.StoredProcedure
                    );

                    var tahunBData = await conn.QueryAsync<BebanUsaha_HPP_RJPP>(
                        "[dbo].[usp_Get_subData_Total_PDSI_RevHPPGA]",
                        new {
                            TahunArray = tahunBTable.AsTableValuedParameter("TahunTableType"),
                            category   = 2
                            },
                        commandType: CommandType.StoredProcedure
                    );

                    // Kembalikan hasil
                    return (tahunPData.ToList(), tahunBData.ToList());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return (new List<PendapatanUsaha_Revenue_RJPP>(), new List<BebanUsaha_HPP_RJPP>());
                }
            }
        }

        private DataTable ConvertArrayToDataTable(int[] array)
        {
            var table = new DataTable();
            table.Columns.Add("Tahun", typeof(int));

            foreach (var item in array)
            {
                table.Rows.Add(item);
            }

            return table;
        }

        public async Task<BaseResponseJson> UploadExcelAsync(IFormFile file, string? userName)
        {
            var response = new BaseResponseJson();
            var revenueList = new List<PendapatanUsaha_Revenue_RJPP>();
            var hppList = new List<BebanUsaha_HPP_RJPP>();
            var administrasiList = new List<BebanUmum_Administrasi_RJPP>();

            if (file == null || file.Length == 0)
            {
                response.Success = false;
                response.Message = "File tidak valid untuk input konsolidasi.";
                return response;
            }

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];

                // Parsing data dari Excel
                for (int column = 5; column <= worksheet.Dimension.Columns; column++)
                {
                    // Parsing Revenue
                    if (int.TryParse(worksheet.Cells[4, column].Text, out int tahun) &&
                        int.TryParse(worksheet.Cells[6, column].Text, out int pdcPendapatanUsaha) &&
                        int.TryParse(worksheet.Cells[7, column].Text, out int pdcEliminasi) &&
                        int.TryParse(worksheet.Cells[8, column].Text, out int maPendapatanUsaha) &&
                        int.TryParse(worksheet.Cells[9, column].Text, out int maEliminasi))
                    {
                        revenueList.Add(new PendapatanUsaha_Revenue_RJPP
                        {
                            Tahun = tahun,
                            PDC_PendapatanUsaha = pdcPendapatanUsaha,
                            PDC_Eliminasi = pdcEliminasi,
                            MA_PendapatanUsaha = maPendapatanUsaha,
                            MA_Eliminasi = maEliminasi,
                            Total_PendapatanUsaha = pdcPendapatanUsaha + pdcEliminasi + maPendapatanUsaha + maEliminasi,
                            created_by = userName,
                            created_time = DateTime.Now
                        });
                    }

                    if (int.TryParse(worksheet.Cells[12, column].Text, out int pdcBebanUsaha) &&
                        int.TryParse(worksheet.Cells[13, column].Text, out int pdcElim) &&
                        int.TryParse(worksheet.Cells[14, column].Text, out int maBebanUsaha) &&
                        int.TryParse(worksheet.Cells[15, column].Text, out int maElim))
                    {
                        hppList.Add(new BebanUsaha_HPP_RJPP
                        {
                            Tahun = tahun,
                            PDC_BebanUsaha = pdcBebanUsaha,
                            PDC_Eliminasi = pdcElim,
                            MA_BebanUsaha = maBebanUsaha,
                            MA_Eliminasi = maElim,
                            Total_BebanUsaha = pdcBebanUsaha + pdcElim + maBebanUsaha + maElim,
                            created_by = userName,
                            created_time = DateTime.Now
                        });
                    }

                    // Parsing Administrasi
                    if (int.TryParse(worksheet.Cells[18, column].Text, out int pdsiBebanUmum) &&
                        int.TryParse(worksheet.Cells[19, column].Text, out int pdcBebanUmum) &&
                        int.TryParse(worksheet.Cells[20, column].Text, out int maBebanUmum))
                    {
                        administrasiList.Add(new BebanUmum_Administrasi_RJPP
                        {
                            Tahun = tahun,
                            PDSI_BebanUmum = pdsiBebanUmum,
                            PDC_BebanUmum = pdcBebanUmum,
                            MA_BebanUmum = maBebanUmum,
                            Total_BebanUmum = pdsiBebanUmum + pdcBebanUmum + maBebanUmum,
                            created_by = userName,
                            created_time = DateTime.Now
                        });
                    }
                }

                // Penyimpanan ke database
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();

                    //delete table
                    string deleteRevenueQuery = "DELETE FROM [dbo].[TblT_RJPP_PendapatanUsaha_Revenue]";
                    string deleteHPPQuerry = "DELETE FROM [dbo].[TblT_RJPP_BebanUsaha_HPP]";
                    string deleteAdministrasiQuery = "DELETE FROM [dbo].[TblT_RJPP_BebanUmum_Administrasi]";

                    await conn.ExecuteAsync(deleteRevenueQuery);
                    await conn.ExecuteAsync(deleteHPPQuerry);
                    await conn.ExecuteAsync(deleteAdministrasiQuery);

                    // Insert Revenue
                    if (revenueList.Any())
                    {
                        string insertRevenueQuery = @"INSERT INTO [dbo].[TblT_RJPP_PendapatanUsaha_Revenue] 
                        (Tahun, PDC_PendapatanUsaha, PDC_Eliminasi, MA_PendapatanUsaha, MA_Eliminasi, Total_PendapatanUsaha, created_by, created_time) 
                        VALUES (@Tahun, @PDC_PendapatanUsaha, @PDC_Eliminasi, @MA_PendapatanUsaha, @MA_Eliminasi, @Total_PendapatanUsaha, @created_by, @created_time)";
                        await conn.ExecuteAsync(insertRevenueQuery, revenueList);
                    }

                    //hppList.Clear();

                    if (hppList.Any())
                    {
                        string insertHppQuery = @"INSERT INTO [dbo].[TblT_RJPP_BebanUsaha_HPP] 
                        (Tahun, PDC_BebanUsaha, PDC_Eliminasi, MA_BebanUsaha, MA_Eliminasi, Total_BebanUsaha, created_by, created_time) 
                        VALUES (@Tahun, @PDC_BebanUsaha, @PDC_Eliminasi, @MA_BebanUsaha, @MA_Eliminasi, @Total_BebanUsaha, @created_by, @created_time)";
                        await conn.ExecuteAsync(insertHppQuery, hppList);
                    }

                    // Insert Administrasi
                    if (administrasiList.Any())
                    {
                        string insertAdministrasiQuery = @"INSERT INTO [dbo].[TblT_RJPP_BebanUmum_Administrasi] 
                        (Tahun, PDSI_BebanUmum, PDC_BebanUmum, MA_BebanUmum, Total_BebanUmum, created_by, created_time) 
                        VALUES (@Tahun, @PDSI_BebanUmum, @PDC_BebanUmum, @MA_BebanUmum, @Total_BebanUmum, @created_by, @created_time)";
                        await conn.ExecuteAsync(insertAdministrasiQuery, administrasiList);
                    }
                }

                response.Success = true;
                response.Message = "File berhasil diunggah dan data disimpan ke database.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Terjadi kesalahan: {ex.Message}";
            }

            return response;
        }
    }
}
