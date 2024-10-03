using Dapper;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Master.Service
{
    public class RoleService : IRoleService
    {
        PlanCorpDbContext _context = null;
        private readonly ConnectionDB _connectionDB;

        public RoleService(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }

        public ResponseJson SaveOrUpdate(RoleParam role)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Roles", new
                    {
                        id = role.Id,
                        name = role.Name,
                        normalizedName = role.Name.ToUpper(),
                        concurrencyStamp = Guid.NewGuid().ToString().ToLower()
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

        public ResponseJson DeleteRole(string id)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Delete_Roles", new
                    {
                        id = id
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
