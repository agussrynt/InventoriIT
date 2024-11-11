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

        public async Task<BaseResponseJson> GetAllHeader()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<HeaderView> HeaderList = new List<HeaderView>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@p", null);

                    conn.Open();
                    HeaderList = (List<HeaderView>)await conn.QueryAsync<HeaderView>("usp_Get_Data_Header", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (HeaderList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = HeaderList;
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

        public ResponseJson SaveOrUpdate (HeaderRevenue headerRevenue)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_HeaderRevenue", new
                    {
                        @ID = headerRevenue.ID,
                        @Tahun = headerRevenue.Tahun,
                        @RJPPNextSta = headerRevenue.RJPPNextSta,
                        @RKAPYearSta = headerRevenue.RKAPYearSta,
                        @Prognosa = headerRevenue.Prognosa,
                        @RealisasiBackYear = headerRevenue.RealisasiBackYear,
                        @CreatedTime = headerRevenue.CreatedTime,
                        @CreatedBy = headerRevenue.CreatedBy,
                    }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;

                return responseJson;
            }
        }

    }
}
