using InventoryIT.Areas.Page.Interfaces;
using Dapper;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Data;
using InventoryIT.Models;
using System.Data;
using System.Data.SqlClient;

namespace InventoryIT.Areas.Page.Services
{
    public class AuditExternalService : IAuditExternalService
    {
        private readonly ConnectionDB _connectionDB;

        public AuditExternalService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        #region Upload Score & Recomendation Region
        public async Task<BaseResponseJson> Get(string id)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AuditExternalList> audits = new List<AuditExternalList>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    audits = (List<AuditExternalList>)await conn.QueryAsync<AuditExternalList>("usp_Get_Audit_External", new { ID_AUDIT_EXTERNAL = id }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    if (audits.Count > 0)
                    {
                        response.Success = true;
                        response.Data = audits.FirstOrDefault();
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
        public async Task<List<AuditExternalList>> GetList()
        {
            List<AuditExternalList> audits = new List<AuditExternalList>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    audits = (List<AuditExternalList>)await conn.QueryAsync<AuditExternalList>("usp_Get_List_Audit_External", commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return audits;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return audits;
            }
        }

        //
        public async Task<BaseResponseJson> GetListScore(string id)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AuditExternalDataScore> audits = new List<AuditExternalDataScore>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    audits = (List<AuditExternalDataScore>)await conn.QueryAsync<AuditExternalDataScore>("usp_Get_List_Audit_External_Data_Score", new { ID_AUDIT_EXTERNAL = id }, commandType: CommandType.StoredProcedure);
                    conn.Close();
                    if (audits.Count > 0)
                    {
                        response.Success = true;
                        response.Data = audits;
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
                System.Diagnostics.Debug.WriteLine(ex.Message);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponseJson> GetListRecomendation(string id)
        {
            BaseResponseJson response = new BaseResponseJson();
            List<AuditExternalDataRecomendation> audits = new List<AuditExternalDataRecomendation>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    audits = (List<AuditExternalDataRecomendation>)await conn.QueryAsync<AuditExternalDataRecomendation>("usp_Get_List_Audit_External_Data_Recomendation", new { ID_AUDIT_EXTERNAL = id }, commandType: CommandType.StoredProcedure);
                    conn.Close();
                    if (audits.Count > 0)
                    {
                        response.Success = true;
                        response.Data = audits;
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
                System.Diagnostics.Debug.WriteLine(ex.Message);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseJson> Create(AuditExternal data)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var dtScore = new DataTable();
                dtScore.Columns.Add("INDIKATOR", typeof(string));
                dtScore.Columns.Add("BOBOT", typeof(decimal));
                dtScore.Columns.Add("JUMLAH_PARAMATER", typeof(int));
                dtScore.Columns.Add("SCORE", typeof(decimal));
                dtScore.Columns.Add("CAPAIAN", typeof(int));

                foreach (var item in data.dataScores)
                {
                    DataRow DR = dtScore.NewRow();
                    DR[0] = item.INDIKATOR;
                    DR[1] = item.BOBOT;
                    DR[2] = item.JUMLAH_PARAMATER;
                    DR[3] = item.SCORE;
                    DR[4] = item.CAPAIAN;
                    dtScore.Rows.Add(DR);
                }

                var dtReco = new DataTable();
                dtReco.Columns.Add("REKOMENDASI", typeof(string));
                dtReco.Columns.Add("ASPEK", typeof(string));

                foreach (var item in data.dataRecomendations)
                {
                    DataRow DR = dtReco.NewRow();
                    DR[0] = item.REKOMENDASI;
                    DR[1] = item.ASPEK; ;
                    dtReco.Rows.Add(DR);
                }

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = await conn.QueryFirstAsync<ResponseJson>("usp_Post_ExternalAudit", new
                    {
                        NamaAuditor = data.AUDITOR_NAME,
                        Date = data.DATE,
                        Attachmen = data.ATTACHMENT,
                        AttachmenName = data.ATTACHMENT_NAME,
                        CreatedBy = data.CREATEDBY,
                        tbl_DataScore = dtScore.AsTableValuedParameter("type_Audit_ExternalDataScore"),
                        tbl_DataRecomendation = dtReco.AsTableValuedParameter("type_Audit_ExternalDataRecomendation")
                    },
                        commandType: CommandType.StoredProcedure);
                    conn.Close();
                }
                //responseJson.Success = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;
            }
            return responseJson;
        }


        public async Task<List<AuditExternal>> Find(string id)
        {
            List<AuditExternal> list = new List<AuditExternal>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                list = conn.Query<AuditExternal>("SELECT  [ID_AUDIT_EXTERNAL],[AUDITOR_NAME],[DATE],[ATTACHMENT],[ATTACHMENT_DATA_SCORE],[ATTACHMENT_RECOMENDATION],[ATTACHMENT_NAME],[ATTACHMENT_DATA_SCORE_NAME],[ATTACHMENT_RECOMENDATION_NAME] "
                                                   + " FROM [dbo].[tbl_T_Audit_External] "
                                                   + " WHERE [ID_AUDIT_EXTERNAL] = " + id).ToList();
                conn.Close();

            }
            return list;
        }

        #endregion

    }
}
