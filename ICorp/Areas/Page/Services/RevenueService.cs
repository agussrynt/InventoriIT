using Dapper;
using NuGet.ContentModel;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;

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

        public async Task<BaseResponseJson> GetProjectRevenue(int idHeader)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<ProjectRevenue> projectRevenues = new List<ProjectRevenue>();
            // Validasi awal parameter
            if (idHeader <= 0)
            {
                response.Success = false;
                response.Message = "Invalid idHeader value di service.";
                return response;
            }
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IDHeader", idHeader);

                    conn.Open();
                    projectRevenues = (List<ProjectRevenue>)await conn.QueryAsync<ProjectRevenue>("usp_Get_Data_ProjectRevenue", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (projectRevenues.Count > 0)
                    {
                        response.Success = true;
                        response.Data = projectRevenues;
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
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }

        public async Task<BaseResponseJson> GetDetailHeaderRevenue(int idHeader)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<HeaderRevenue> projectRevenues = new List<HeaderRevenue>();
            // Validasi awal parameter
            if (idHeader <= 0)
            {
                response.Success = false;
                response.Message = "Invalid idHeader value di service.";
                return response;
            }
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@p", idHeader);

                    conn.Open();
                    projectRevenues = (List<HeaderRevenue>)await conn.QueryAsync<HeaderRevenue>("usp_Get_Data_Header", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (projectRevenues.Count > 0)
                    {
                        response.Success = true;
                        response.Data = projectRevenues;
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
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }

        public List<ProjectDD> GetProjectExist()
        {
            List<ProjectDD> projectList = new List<ProjectDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID] ,[NamaProject] FROM [PDSI_PLANCORP].[dbo].[TblT_ProjectRevenue]";
                    projectList = (List<ProjectDD>)conn.Query<ProjectDD>(sql);
                    conn.Close();

                    result = projectList.Count > 0;

                    return projectList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return projectList;
            }
        }

        public async Task<BaseResponseJson> GetProjectExistById(int ProjectID)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<ProjectExist> ProjectList = new List<ProjectExist>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pID", ProjectID);

                    conn.Open();
                    ProjectList = (List<ProjectExist>)await conn.QueryAsync<ProjectExist>("usp_Get_Data_Project", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (ProjectList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = ProjectList;
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

        public async Task<BaseResponseJson> GetCostCenterFill(int AssetID)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<CostCenterFill>  costCenterList = new List<CostCenterFill>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pAssetID", AssetID);

                    conn.Open();
                    costCenterList = (List<CostCenterFill>)await conn.QueryAsync<CostCenterFill>("usp_Get_Fill_CostCenter", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (costCenterList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = costCenterList;
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

        public async Task<BaseResponseJson> GetRegionalFill(int CustomerID)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<RegionalFill> regionalResult = new List<RegionalFill>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    string sql = @"SELECT [ID],[Customer] ,[Regional] FROM [PDSI_PLANCORP].[dbo].[TblM_Customer] WHERE [ID] = @CustomerID";
                    var parameters = new { CustomerID = CustomerID };
                    conn.Open();
                    regionalResult = (List<RegionalFill>)await conn.QueryAsync<RegionalFill>(sql, parameters);
                    conn.Close();

                    if (regionalResult.Count > 0)
                    {
                        response.Success = true;
                        response.Data = regionalResult;
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

        public ResponseJson SaveMappingProject(MappingProjectRevenue mappingProjectRevenue)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Mapping_ProjectRevenue", new
                    {
                        @pIDHeader = mappingProjectRevenue.IDHeader,
                        @pIDProject = mappingProjectRevenue.IDProject,
                        @pSegmen = mappingProjectRevenue.Segmen,
                        @pAsset = mappingProjectRevenue.Asset,
                        @pCustomer = mappingProjectRevenue.Customer,
                        @pContract = mappingProjectRevenue.Contract,
                        @pPekerjaan = mappingProjectRevenue.Pekerjaan,
                        @pSBT = mappingProjectRevenue.SBT,
                        @pNamaProject = mappingProjectRevenue.NamaProject,
                        @pProbability = mappingProjectRevenue.Probability,
                        @pSumur = mappingProjectRevenue.Sumur,
                        @pControlProject = mappingProjectRevenue.ControlProject,
                        @pCreated_by = mappingProjectRevenue.CreatedBy
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

        public async Task<BaseResponseJson> GetDetailRevenue(int idHeader)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<DetailRevenue> detailRevenues = new List<DetailRevenue>();
            // Validasi awal parameter
            if (idHeader <= 0)
            {
                response.Success = false;
                response.Message = "Invalid idHeader value di service.";
                return response;
            }
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pIDHeader", idHeader);

                    conn.Open();
                    detailRevenues = (List<DetailRevenue>)await conn.QueryAsync<DetailRevenue>("usp_Get_Detail_Revenue", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (detailRevenues.Count > 0)
                    {
                        response.Success = true;
                        response.Data = detailRevenues;
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
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }


    }

}
