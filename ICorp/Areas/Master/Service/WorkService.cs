using Dapper;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Master.Service
{
    public class WorkService : IWorkService
    {
        private PlanCorpDbContext _context;
        private readonly ConnectionDB _connectionDB;

        public WorkService(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }

        public async Task<BaseResponseJson> GetWorksByID(int workID)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<Works> workList = new List<Works>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID],[Pekerjaan],[IsAktif],[CreatedTime],[CreatedBy] FROM [dbo].[TblT_Pekerjaan] WHERE ID =" + workID;
                    workList = (List<Works>)await conn.QueryAsync<Works>(sql);
                    conn.Close();

                    if (workList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = workList;
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

        public async Task<BaseResponseJson> GetAllWorks()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<Works> workList = new List<Works>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID],[Pekerjaan],[IsAktif],[CreatedTime],[CreatedBy] FROM [dbo].[TblT_Pekerjaan] " + 
                        "ORDER BY CreatedTime DESC";
                    workList = (List<Works>)await conn.QueryAsync<Works>(sql);
                    conn.Close();

                    if (workList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = workList;
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

        public ResponseJson SaveOrUpdate(Works work)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Pekerjaan", new
                    {
                        @pID = work.ID,
                        @pPekerjaan = work.Pekerjaan,
                        @pIsAktif = work.IsAktif,
                        @pCreated_by = work.CreatedBy
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

        public ResponseJson DeleteWorks(int workID)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "UPDATE [dbo].[TblT_Pekerjaan] SET [IsAktif] = 0 WHERE ID =" + workID;
                    responseJson = conn.QueryFirst<ResponseJson>(sql);
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