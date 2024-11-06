using Dapper;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    
    public class RevenueService : IRevenueService
    {
        private PlanCorpDbContext _context;
        private readonly ConnectionDB _connectionDB;
        public RevenueService(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }

        public async Task<BaseResponseJson> GetHeaderRevenue()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<HeaderRevenue> headerRevenues = new List<HeaderRevenue>();

            try
            {
                using(IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID],[Tahun],[RJPPNextSta],[RKAPYearSta],[Prognosa], [RealisasiBackYear], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy] FROM [dbo].[TblT_Pekerjaan] " +
                        "ORDER BY CreatedTime DESC";
                    headerRevenues = (List<HeaderRevenue>)await conn.QueryAsync<HeaderRevenue>(sql);
                    conn.Close();
                    if (headerRevenues.Count > 0)
                    {
                        response.Success = true;
                        response.Data = headerRevenues;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Data Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = true;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
