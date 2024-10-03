using Dapper;
using FakeAPI_PDSI.Model;
using System.Data;

namespace FakeAPI_PDSI.Services
{
    public interface IDataAccessService
    {
        Task<ResponseModel> Gets();
    }
    public class DataAccessService : IDataAccessService
    {
        private readonly DBConnection _connectionDB;

        public DataAccessService(DBConnection connectionDB)
        {
            _connectionDB = connectionDB;
        }


        public async Task<ResponseModel> Gets()
        {
            ResponseModel response = new ResponseModel();
            List<UserInRoleView> usrList = new List<UserInRoleView>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    usrList = (List<UserInRoleView>)await conn.QueryAsync<UserInRoleView>("usp_Get_User_List", null, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (usrList.Count > 0)
                    {
                        response.IsSuccess = true;
                        response.Data = usrList;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = true;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
