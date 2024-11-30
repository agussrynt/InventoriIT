using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Services;
using PlanCorp.Data;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Controllers
{
    //[Authorize]
    [Area("Page")]
    [Route("page/inputkonsolidasi")]
    public class InputKonsolidasiController : Controller
    {
        private readonly PlanCorpDbContext _context;
        private readonly IInput_Konsolidasi_Service _input_konsolidasi_service;

        public InputKonsolidasiController(PlanCorpDbContext context, IInput_Konsolidasi_Service input_konsolidasi_service) 
        {
            _context = context;
            _input_konsolidasi_service = input_konsolidasi_service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("get_list_inputkonsolidasi")]
        public async Task<JsonResult> GetDataInputKonsolidasi()
        {
            try
            {
                // Ambil data awal
                var data = await _input_konsolidasi_service.GetAll_InputKonsolidasi();
                var pendapatan = data.pendapatan;
                var beban = data.beban;
                var administrasi = data.administrasi;

                // Ambil array tahun yang unik
                var tahunArrayP = pendapatan.Select(p => p.Tahun).Distinct().ToArray();
                var tahunArrayB = beban.Select(b => b.Tahun).Distinct().ToArray();

                // Panggil service untuk mendapatkan data tambahan
                var subdata = await _input_konsolidasi_service.GetSub_InputKonsolidasi(tahunArrayP, tahunArrayB);

                // Kembalikan data sebagai JSON
                return Json(new
                {
                    Success = true,
                    Pendapatan = pendapatan,
                    Beban = beban,
                    Administrasi = administrasi,
                    SubPendapatan = subdata.TahunP,
                    SubBeban = subdata.TahunB
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("upload-excel-ajax")]
        public async Task<JsonResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new JsonResult(new BaseResponseJson
                {
                    Success = false,
                    Message = "File tidak diterima atau kosong."
                });
            }

            try
            {
                string? userName = HttpContext.Session.GetString("username");
                var response = await _input_konsolidasi_service.UploadExcelAsync(file, userName);

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return new JsonResult(new BaseResponseJson
                {
                    Success = false,
                    Message = $"Terjadi kesalahan: {ex.Message}"
                });
            }
        }

    }
}
