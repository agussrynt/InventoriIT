using Dapper;
using Microsoft.AspNetCore.Mvc;
using PlanCorp.Areas.Master.Models;
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
            List<RevHPPGAView> revHPPGAList = new List<RevHPPGAView>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();

                    conn.Open();
                    var result = await conn.QueryAsync<dynamic>("usp_Get_Data_RevHPPGA", commandType: CommandType.StoredProcedure);
                    conn.Close();

                    foreach (var item in result)
                    {
                        var revItem = new RevHPPGAView
                        {
                            ID = item.ID,
                            SegmentRJPP = item.SegmentRJPP,
                            NamaCostCenter = item.NamaCostCenter,
                            HP = item.HP,
                            KategoriRIG = item.KategoriRIG,
                            UniqueCode = item.UniqueCode,
                            PIC = item.PIC,
                            Costumer = item.Costumer,
                            Project = item.Project,
                            HPPSales = item.HPPSales,
                            GASales = item.GASales
                        };

                        // Map the dynamic revenue columns to the dictionary
                        foreach (var prop in item)
                        {
                            if (prop.Key.StartsWith("Revenue_"))
                            {
                                revItem.Revenues[prop.Key] = prop.Value as int?;
                            }

                            if (prop.Key.StartsWith("HPP_"))
                            {
                                revItem.HPPs[prop.Key] = prop.Value as int?;
                            }
                        }

                        revHPPGAList.Add(revItem);
                    }

                    if (revHPPGAList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = revHPPGAList;
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
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponseJson> GetSumRevHPPGA()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<SummaryView> sumList = new List<SummaryView>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();

                    conn.Open();
                    var result = await conn.QueryAsync<dynamic>("usp_Get_Data_SUMRevHPPGA", commandType: CommandType.StoredProcedure);
                    conn.Close();

                    foreach (var item in result)
                    {
                        var sumItem = new SummaryView
                        {
                            Category = item.Category
                        };

                        // Map the dynamic revenue columns to the dictionary
                        foreach (var prop in item)
                        {
                            if (prop.Key.StartsWith("RJPP_"))
                            {
                                sumItem.RJPP[prop.Key] = prop.Value as int?;
                            }
                        }

                        sumList.Add(sumItem);
                    }

                    if (sumList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = sumList;
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
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ResponseJson InsertDetail(RevHPPGADetail DetailRev)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Detail_RevHPPGA", new
                    {
                        @pSegmentRJPP = DetailRev.SegmentRJPP,
                        @pNamaCostCenter = DetailRev.NamaCostCenter,
                        @pHP = DetailRev.HP,
                        @pUniqueCode = DetailRev.UniqueCode,
                        @pKategoriRig = DetailRev.KategoriRIG,
                        @pPIC = DetailRev.PIC,
                        @pCostumer = DetailRev.Costumer,
                        @pProject = DetailRev.Project,
                        @pHPPSales = DetailRev.HPPSales,
                        @pGASales = DetailRev.GASales,
                        @pCreatedBy = DetailRev.CreatedBy
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

        public ResponseJson InsertRevenue(Revenue_RJPP Revenue)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Data_Revenue", new
                    {
                        @pIDDetailRev = Revenue.Id_DetailRevHPPGA,
                        @pRevenue = Revenue.Revenue,
                        @pTahun = Revenue.Tahun,
                        @pCreatedBy = Revenue.CreatedBy
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

        public ResponseJson InsertHPP(HPP_RJPP HPP)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Data_HPP", new
                    {
                        @pIDDetailRev = HPP.Id_DetailRevHPPGA,
                        @pHPP = HPP.HPP,
                        @pTahun = HPP.Tahun,
                        @pCreatedBy = HPP.CreatedBy
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

        public ResponseJson DeleteExistingData(string idDetail)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_DeleteExistingData", new
                    {
                        @pIDDetailRev = idDetail
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