using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;

namespace PlanCorp.Areas.Page.Interfaces
{
    public interface IInput_Konsolidasi_Service
    {
        Task<(List<PendapatanUsaha_Revenue_RJPP> pendapatan, List<BebanUsaha_HPP_RJPP> beban, List<BebanUmum_Administrasi_RJPP> administrasi)> GetAll_InputKonsolidasi();
        Task<BaseResponseJson> UploadExcelAsync(IFormFile file, string? userName);
        Task<(List<PendapatanUsaha_Revenue_RJPP> TahunP, List<BebanUsaha_HPP_RJPP> TahunB)> GetSub_InputKonsolidasi(int[] tahunArrayP, int[] tahunArrayB);

        //Task<BaseResponseJson> GetSub_InputKonsolidasi(List<PendapatanUsaha_Revenue_RJPP> tahunArrayP, List<BebanUsaha_HPP_RJPP> tahunArrayB)>;

    }
}
