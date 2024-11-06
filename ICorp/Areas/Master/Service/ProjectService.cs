using Dapper;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using NuGet.ContentModel;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PlanCorp.Areas.Master.Service
{
    public class ProjectService : IProjectService
    {
        private PlanCorpDbContext _context;
        private readonly ConnectionDB _connectionDB;

        public ProjectService(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }

        public List<SegmenDD> GetSegmenProject()
        {
            List<SegmenDD> SegmenList = new List<SegmenDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID] ,[Segmen] FROM [PDSI_PLANCORP].[dbo].[TblM_Segmen]";
                    SegmenList = (List<SegmenDD>)conn.Query<SegmenDD>(sql);
                    conn.Close();

                    result = SegmenList.Count > 0;

                    return SegmenList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return SegmenList;
            }
        }

        public List<AssetDD> GetAssetProject()
        {
            List<AssetDD> AssetDDList = new List<AssetDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID] ,[Asset] FROM [PDSI_PLANCORP].[dbo].[TblT_AssetRevenue]";
                    AssetDDList = (List<AssetDD>)conn.Query<AssetDD>(sql);
                    conn.Close();

                    result = AssetDDList.Count > 0;

                    return AssetDDList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return AssetDDList;
            }
        }

        public List<CustomerDD> GetCustomerProject()
        {
            List<CustomerDD> CustomerDDList = new List<CustomerDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID] ,[Customer], [Regional] FROM [PDSI_PLANCORP].[dbo].[TblM_Customer]";
                    CustomerDDList = (List<CustomerDD>)conn.Query<CustomerDD>(sql);
                    conn.Close();

                    result = CustomerDDList.Count > 0;

                    return CustomerDDList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return CustomerDDList;
            }
        }

        public List<PekerjaanDD> GetPekerjaanProject()
        {
            List<PekerjaanDD> PekerjaanDDList = new List<PekerjaanDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID] ,[Pekerjaan] FROM [PDSI_PLANCORP].[dbo].[TblT_Pekerjaan]";
                    PekerjaanDDList = (List<PekerjaanDD>)conn.Query<PekerjaanDD>(sql);
                    conn.Close();

                    result = PekerjaanDDList.Count > 0;

                    return PekerjaanDDList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return PekerjaanDDList;
            }
        }

        public List<TipeContractsDD> GetContractTypeProject()
        {
            List<TipeContractsDD> TipeContractsDDList = new List<TipeContractsDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID] ,[Contract] FROM [PDSI_PLANCORP].[dbo].[TblM_TipeContract]";
                    TipeContractsDDList = (List<TipeContractsDD>)conn.Query<TipeContractsDD>(sql);
                    conn.Close();

                    result = TipeContractsDDList.Count > 0;

                    return TipeContractsDDList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return TipeContractsDDList;
            }
        }

        public List<SBTDD> GetSBTProject()
        {
            List<SBTDD> SBTDDList = new List<SBTDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID],[SbtIndex], [Level1], [Level2], [Level3], [Penjelasan] FROM [PDSI_PLANCORP].[dbo].[TblM_SBT]";
                    SBTDDList = (List<SBTDD>)conn.Query<SBTDD>(sql);
                    conn.Close();

                    result = SBTDDList.Count > 0;

                    return SBTDDList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return SBTDDList;
            }
        }

        public async Task<BaseResponseJson> GetProjectsByID(int ProjectID)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<ProjectsView> ProjectList = new List<ProjectsView>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pID", ProjectID);

                    conn.Open();
                    ProjectList = (List<ProjectsView>)await conn.QueryAsync<ProjectsView>("usp_Get_Data_Project", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<BaseResponseJson> GetAllProject()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<ProjectsView> ProjectsList = new List<ProjectsView>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pID", null);

                    conn.Open();
                    ProjectsList = (List<ProjectsView>)await conn.QueryAsync<ProjectsView>("usp_Get_Data_Project", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (ProjectsList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = ProjectsList;
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

        public ResponseJson SaveOrUpdate(Projects Project)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Project", new
                    {
                        @pID = Project.ID,
                        @pNamaProject = Project.NamaProject,
                        @pSegmen = Project.Segmen,
                        @pAsset = Project.Asset,
                        @pCustomer = Project.Customer,
                        @pContract = Project.Contract,
                        @pProbability = Project.Probability,
                        @pSumur = Project.Sumur,
                        @pControlProject = Project.ControlProject,
                        @pPekerjaan = Project.Pekerjaan,
                        @pSBT = Project.SBT,
                        @pCreated_by = Project.CreatedBy
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

        public ResponseJson DeleteProject(int ProjectID)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "DELETE FROM [dbo].[TblT_ProjectRevenue] WHERE ID =" + ProjectID;
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