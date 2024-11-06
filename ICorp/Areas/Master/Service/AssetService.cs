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

namespace PlanCorp.Areas.Master.Service
{
    public class AssetService : IAssetService
    {
        private PlanCorpDbContext _context;
        private readonly ConnectionDB _connectionDB;

        public AssetService(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }

        public List<TipeAssets> GetAssetTypeDropdown()
        {
            List<TipeAssets> assetTypeList = new List<TipeAssets>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [ID] ,[TipeAsset] FROM [PDSI_PLANCORP].[dbo].[TblM_TipeAsset]";
                    assetTypeList = (List<TipeAssets>)conn.Query<TipeAssets>(sql);
                    conn.Close();

                    result = assetTypeList.Count > 0;

                    return assetTypeList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return assetTypeList;
            }
        }

        public List<CostCenterDD> GetCostCenterDropdown()
        {
            List<CostCenterDD> CostCenterDDList = new List<CostCenterDD>();
            var result = false;

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "SELECT [Client] ,[FundsCenter], [Name] FROM [PDSI_PLANCORP].[dbo].[TblM_CostCenter]";
                    CostCenterDDList = (List<CostCenterDD>)conn.Query<CostCenterDD>(sql);
                    conn.Close();

                    result = CostCenterDDList.Count > 0;

                    return CostCenterDDList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return CostCenterDDList;
            }
        }

        public async Task<BaseResponseJson> GetAssetsByID(int assetID)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AssetView> assetList = new List<AssetView>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pID", assetID);

                    conn.Open();
                    assetList = (List<AssetView>)await conn.QueryAsync<AssetView>("usp_Get_Data_Asset", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (assetList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = assetList;
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

        public async Task<BaseResponseJson> GetAllAsset()
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AssetView> assetsList = new List<AssetView>();

            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pID", null);

                    conn.Open();
                    assetsList = (List<AssetView>)await conn.QueryAsync<AssetView>("usp_Get_Data_Asset", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (assetsList.Count > 0)
                    {
                        response.Success = true;
                        response.Data = assetsList;
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

        public ResponseJson SaveOrUpdate(Assets asset)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Asset", new
                    {
                        @pID = asset.ID,
                        @pAsset = asset.Asset,
                        @pKeterangan = asset.Keterangan,
                        @pAssetType = asset.TipeAsset,
                        @pCostCenter = asset.CostCenter,
                        @pCreated_by = asset.CreatedBy
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

        public ResponseJson DeleteAsset(int assetID)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    string sql = "DELETE FROM [dbo].[TblT_AssetRevenue] WHERE ID =" + assetID;
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