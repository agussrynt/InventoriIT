using Dapper;
using Microsoft.AspNetCore.Mvc;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    public class Revenue_RJPP_Service : IRevenue_RJPP_Service
    {
        private PlanCorpDbContext _dbContext;
        private readonly ConnectionDB _connectionDB;

        public Revenue_RJPP_Service(PlanCorpDbContext dbContext, ConnectionDB connectionDB)
        {
            _dbContext = dbContext;
            _connectionDB = connectionDB;
        }

        public async Task<BaseResponseJson> GetRevenueByID(int RevenueID)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<Revenue_RJPP> revenueList = new List<Revenue_RJPP>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    //string sql = "SELECT [ID],[Pekerjaan],[IsAktif],[CreatedTime],[CreatedBy] FROM [dbo].[TblT_Revenue_RJPP] WHERE ID =" + RevenueID;
                    string sql = "SELECT * FROM [dbo].[TblT_Revenue_RJPP] WHERE ID =" + RevenueID;
                    revenueList = (List<Revenue_RJPP>)await conn.QueryAsync<Revenue_RJPP>(sql);
                    conn.Close();

                    if (revenueList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = revenueList;
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

        public async Task<BaseResponseJson> GetAllRevenue()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<Revenue_RJPP> workList = new List<Revenue_RJPP>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    //string sql = "SELECT [ID],[Pekerjaan],[IsAktif],[CreatedTime],[CreatedBy] FROM [dbo].[TblT_Revenue_RJPP] " +
                    //    "ORDER BY CreatedTime DESC";
                    string sql = "SELECT * FROM [dbo].[TblT_Revenue_RJPP] " +
                        "ORDER BY CreatedTime DESC";
                    workList = (List<Revenue_RJPP>)await conn.QueryAsync<Revenue_RJPP>(sql);
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

    }
}
