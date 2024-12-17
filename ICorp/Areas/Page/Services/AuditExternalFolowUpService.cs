using Dapper;
using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Data;
using InventoryIT.Models;
using System.Data;

namespace InventoryIT.Areas.Page.Services
{
    public class AuditExternalFolowUpService : IAuditExternalFolowUpService
    {
        private readonly ConnectionDB _connectionDB;

        public AuditExternalFolowUpService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public async Task<List<AuditExternalFollowUpList>> GetList(string username)
        {
            List<AuditExternalFollowUpList> audits = new List<AuditExternalFollowUpList>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                audits = (List<AuditExternalFollowUpList>)await conn.QueryAsync<AuditExternalFollowUpList>("usp_Get_List_Audit_External_User_AssigmentRecomendation", new { @UserName = username },
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
                    responseJson = await conn.QueryFirstAsync<BaseResponseJson>("usp_Post_Audit_External_FollowUp", new
                    {
                        ID_ASSIGMENT_RECOMENDATION = data.ID_ASSIGMENT_RECOMENDATION,
                        AUDITEE_FOLLOWUP = data.AUDITEE_FOLLOWUP,
                        ATTACHMENT = data.ATTACHMENT,
                        FILENAME = data.FILENAME,
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
