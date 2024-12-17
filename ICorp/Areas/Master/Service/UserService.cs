using Dapper;
using InventoryIT.Areas.Master.Interface;
using InventoryIT.Areas.Master.Models;
using InventoryIT.Data;
using InventoryIT.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InventoryIT.Areas.Master.Service
{
    public class UserService : IUserService
    {
        PlanCorpDbContext _context = null;
        private readonly ConnectionDB _connectionDB;

        public UserService(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }

        public void Delete(string userId)
        {
            throw new NotImplementedException();
        }

        public UserDetail FindUser(string email)
        {
            UserDetail userDetail = new UserDetail();
            try
            {
                if (email != null)
                {

                    var params1 = new SqlParameter("@email", email);
                    userDetail = _context.UserDetail.FromSqlRaw("exec usp_Get_User_Detail @email", params1).FirstOrDefault();
                }

                return userDetail;
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.Message);
                return userDetail;
            }
        }

        public List<PIC> GetPICDropdown(int paramsId)
        {
            List<PIC> picList = new List<PIC>();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@paramsId", paramsId);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    picList = (List<PIC>)conn.Query<PIC>("usp_Get_User_PIC_DropDown", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return picList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return picList;
            }
        }

        public List<UserDetail> GetUsers(string userId)
        {
            try
            {
                var userLogin = new SqlParameter("@UserId", userId);
                var allusers = _context.UserDetail.FromSqlRaw("exec usp_Get_User_List @UserId", userLogin).ToList();

                return allusers;
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                return new List<UserDetail>();
            }
        }

        public void SaveOrUpdate(UserDetail user)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponseJson> Gets()
        {
            BaseResponseJson response = new BaseResponseJson();
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
                        response.Success = true;
                        response.Data = usrList;
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
        public bool IsActiveUser(string username)
        {
            using (IDbConnection conn = _connectionDB.Connection)
            {
                var result = false;
                List<Profile> temp = new List<Profile>();
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM [dbo].[Profiles] WHERE UserName = '"+ username +"' AND IsActive = 1";
                    temp = (List<Profile>)conn.Query<Profile>(sql);
                    conn.Close();

                    result = temp.Count > 0;

                    return result;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return result;
                }
            }
        }

        public bool ActivateUser(string username, bool status)
        {
            using (IDbConnection conn = _connectionDB.Connection)
            {
                var result = false;
                List<Profile> temp = new List<Profile>();
                try
                {
                    string stat = status ? "1" : "0";
                    conn.Open();
                    string sql = "UPDATE [PDSI-GCG].[dbo].[Profiles] SET IsActive = "+stat+" WHERE UserName = '"+ username +"'";
                    temp = (List<Profile>)conn.Query<Profile>(sql);
                    conn.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return result;
                }
            }
        }

    }
}
