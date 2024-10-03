using PlanCorp_API.Helper;
using PlanCorp_API.Models;
using System.Data;
using Dapper;

namespace PlanCorp_API.Services
{
    public interface IDashboardService
    {
        Task<List<JumlahTemuanChartModel>> GetTemuanAuditInternal();
        Task<List<JumlahTemuanChartModel>> GetTemuanAuditExternal();
        Task<List<FollowUpComparerItem>> GetFollowUpAuditInternal();
        Task<List<FollowUpComparerItem>> GetFollowUpAuditExternal();
    }
    public class DashboardService : IDashboardService
    {
        private readonly ConnectionDB _connectionDB;

        public DashboardService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public async Task<List<JumlahTemuanChartModel>> GetTemuanAuditInternal()
        {
            List<JumlahTemuanChartModel> result = new List<JumlahTemuanChartModel>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<JumlahTemuanChartModel>)await conn.QueryAsync<JumlahTemuanChartModel>("usp_Get_Dashboard_Temuan_Audit_Internal", commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }
        public async Task<List<JumlahTemuanChartModel>> GetTemuanAuditExternal()
        {
            List<JumlahTemuanChartModel> result = new List<JumlahTemuanChartModel>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<JumlahTemuanChartModel>)await conn.QueryAsync<JumlahTemuanChartModel>("usp_Get_Dashboard_Temuan_Audit_External", commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }

        public async Task<List<FollowUpComparerItem>> GetFollowUpAuditInternal()
        {
            List<FollowUpComparerItem> result = new List<FollowUpComparerItem>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<FollowUpComparerItem>)await conn.QueryAsync<FollowUpComparerItem>("usp_Get_Dashboard_FollowUP_Internal", commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }

        public async Task<List<FollowUpComparerItem>> GetFollowUpAuditExternal()
        {
            List<FollowUpComparerItem> result = new List<FollowUpComparerItem>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<FollowUpComparerItem>)await conn.QueryAsync<FollowUpComparerItem>("usp_Get_Dashboard_FollowUP_External", commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }
    }
}
