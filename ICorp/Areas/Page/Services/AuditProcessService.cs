using Dapper;
using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Data;
using InventoryIT.Models;
using System.Data;

namespace InventoryIT.Areas.Page.Services
{
    public class AuditProcessService : IAuditProcessService
    {
        private readonly ConnectionDB _connectionDB;

        public AuditProcessService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public ResponseJson AuthorResponse(int UpId, int Status, string Remarks, float Score, string Recommendation, int IsRecommendation, string Username)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RECOMMENDATION", Recommendation);
                parameters.Add("@USERNAME", Username);
                parameters.Add("@ID_UP", UpId);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    var storeAudit = this.StoreAudit(UpId, Status, Remarks, Score, Username);
                    if (IsRecommendation == 1 && storeAudit.Success)
                        return conn.QueryFirst<ResponseJson>("usp_Post_Set_Follow_Up", parameters, commandType: CommandType.StoredProcedure);

                    responseJson = storeAudit;
                    conn.Close();
                }

                return responseJson;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return responseJson;
            }
        }



        private ResponseJson StoreAudit(int UpId, int Status, string Remarks, float Score, string Username)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                string RESPTYPE = Status == 1 ? "Approved" : "Rejected";
                var parameters = new DynamicParameters();
                parameters.Add("@RESPTYPE", RESPTYPE);
                parameters.Add("@STATUS", Status);
                parameters.Add("@SCORE", Score);
                parameters.Add("@REMARKS", Remarks);
                parameters.Add("@USERNAME", Username);
                parameters.Add("@ID_UP", UpId);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Audit", parameters, commandType: CommandType.StoredProcedure);
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

        public bool AuthorResponseBackup(int UpId, int responseType, string Remarks = null, float Score = 0, string Username = "")
        {
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    if (String.IsNullOrEmpty(Username))
                        throw new Exception("Gagal");

                    string RESPTYPE = responseType == 1 ? "Approved" : "Rejected";
                    DateTime RESPDATE = DateTime.Now;
                    conn.Open();
                    var query = @"UPDATE tbl_T_Audit SET RESPONDATE = @RESPONDATE, RESPONTYPE = @RESPTYPE, STATUS = @STATUS, SCOREUP = @SCORE, DOCUMENTREVIEW = @REMARKS, LASTUPDATEDBY = @USERNAME, LASTUPDATEDDATE = @DATENOW WHERE ID_UP = @ID_UP";
                    var respo = conn.Execute(query, new { 
                        RESPONDATE = RESPDATE, 
                        RESPTYPE = RESPTYPE, 
                        STATUS = responseType, 
                        SCORE = Score, 
                        REMARKS = Remarks,
                        USERNAME = Username,
                        DATENOW = RESPDATE,
                        ID_UP = UpId 
                    });
                    conn.Close();

                    if (respo <= 0)
                        throw new Exception("Gagal");

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return false;
            }
        }

        public List<AuditDetail> GetDetailList(string year)
        {
            List<AuditDetail> auditDetails = new List<AuditDetail>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    auditDetails = (List<AuditDetail>)conn.Query<AuditDetail>("usp_Get_Detail_Audit_Process", new { year = year }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return auditDetails;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return auditDetails;
            }
        }

        public List<Audit> GetList()
        {
            List<Audit> audits = new List<Audit>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    audits = (List<Audit>)conn.Query<Audit>("usp_Get_List_Audit_Process", commandType: CommandType.StoredProcedure);
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

        public List<AuditReview> GetListReview()
        {
            List<AuditReview> audits = new List<AuditReview>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    audits = (List<AuditReview>)conn.Query<AuditReview>("usp_Get_List_Audit_Review", commandType: CommandType.StoredProcedure);
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

        public ResponseJson UpdateScore(int UpId, float Score, string Username)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SCORE", Score);
                parameters.Add("@USERNAME", Username);
                parameters.Add("@ID_UP", UpId);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Update_Score", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();
                }

                return responseJson;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return responseJson;
            }
        }

        public ResponseJson IsReadyComplete(int year)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Year", year);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_IsAlreadyCompleteAudit", parameters, commandType: CommandType.StoredProcedure);
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

        public ResponseJson SetFinishAudit(int year)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Year", year);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Set_Finish_Audit", parameters, commandType: CommandType.StoredProcedure);
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
