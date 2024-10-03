using Dapper;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    public class AuditExternalMonitoring : IAuditExternalMonitoring
    {
        private readonly ConnectionDB _connectionDB;

        public AuditExternalMonitoring(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public async Task<List<AuditExternalFollowUpList>> GetList()
        {
            List<AuditExternalFollowUpList> audits = new List<AuditExternalFollowUpList>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                audits = (List<AuditExternalFollowUpList>)await conn.QueryAsync<AuditExternalFollowUpList>("usp_Get_List_Audit_External_FollowUpReview", 
                    commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return audits;
        }

        public async Task<BaseResponseJson> Create(AuditExternalFollowUp data)
        {
            BaseResponseJson responseJson = new BaseResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = await conn.QueryFirstAsync<BaseResponseJson>("usp_Post_Audit_External_FollowUp_Review", new
                    {
                        ID_ASSIGMENT_RECOMENDATION = data.ID_ASSIGMENT_RECOMENDATION,
                        REMARK = data.REMARK,
                        CREATEDBY = data.CREATEDBY
                    },
                        commandType: CommandType.StoredProcedure);
                    conn.Close();
                }
                responseJson.Success = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;
            }
            return responseJson;
        }
    }
}
