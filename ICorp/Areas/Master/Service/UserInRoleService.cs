using PlanCorp.Areas.Identity;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;
using PlanCorp.Models;

namespace PlanCorp.Areas.Master.Service
{
    public class UserInRoleService : IUserInRoleService
    {
        //private readonly PlanCorpDbContext _context;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly UserManager<PlanCorpUser> _userManager;

        //public UserInRoleService(
        //    PlanCorpDbContext context,
        //    RoleManager<IdentityRole> roleManager,
        //    UserManager<PlanCorpUser> userManager)
        //{
        //    _context = context;
        //    _roleManager = roleManager;
        //    _userManager = userManager;
        //}
        PlanCorpDbContext _context = null;
        private readonly ConnectionDB _connectionDB;

        public UserInRoleService(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }
        public string Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserInRole>> GetAllUserInRoles()
        {
            List<UserInRole> getUser = new List<UserInRole>();
            try
            {
                getUser = await _context.UserInRoles.FromSqlRaw("EXEC usp_Get_User_List").ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return getUser;
        }

        public int GetUserFPPByEmpNumber(string empNumber)
        {
            int check = 0;
            try
            {
                var data = _context.TOKLANG_PEJABATFPP.FirstOrDefault(a => a.Nopek == empNumber);
                if (data != null)
                {
                    check = 1;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return check;
        }

        public UserInRole GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(UserInRole role)
        {
            throw new NotImplementedException();
        }

        public void Update(UserInRole role)
        {
            throw new NotImplementedException();
        }

        //Task<List<UserInRole>> IUserInRoleService.GetAllUserInRoles()
        //{
        //    throw new NotImplementedException();
        //}

        UserInRole IUserInRoleService.GetById(string id)
        {
            throw new NotImplementedException();
        }

        //public void Save(UserInRole role)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(UserInRole role)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<BaseResponseJson> RemoveAllRole(string userid)
        {
            BaseResponseJson responseJson = new BaseResponseJson();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                responseJson = await conn.QueryFirstAsync<BaseResponseJson>("usp_Delete_User_Role", new
                {
                    UserId = userid
                },commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return responseJson;
        }

        public async Task<BaseResponseJson> SetUserRole(string userid, string role)
        {
            BaseResponseJson responseJson = new BaseResponseJson();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                responseJson = await conn.QueryFirstAsync<BaseResponseJson>("usp_Set_User_Role", new
                {
                    UserId = userid,
                    Role = role
                }, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return responseJson;
        }
        //
    }
}