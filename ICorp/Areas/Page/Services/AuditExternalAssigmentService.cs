using InventoryIT.Areas.Page.Interfaces;
using InventoryIT.Data;
using Dapper;
using InventoryIT.Areas.Page.Models;
using InventoryIT.Models;
using System.Data;
using System.Data.SqlClient;

namespace InventoryIT.Areas.Page.Services
{
    public class AuditExternalAssigmentService : IAuditExternalAssigmentService
    {
        private readonly ConnectionDB _connectionDB;

        public AuditExternalAssigmentService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public async Task<List<AuditExternalAssigmentRecomendationList>> GetList(int year)
        {
            List<AuditExternalAssigmentRecomendationList> audits = new List<AuditExternalAssigmentRecomendationList>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                audits = (List<AuditExternalAssigmentRecomendationList>)await conn.QueryAsync<AuditExternalAssigmentRecomendationList>("usp_Get_List_Recomendation_Assigment", new {Year = year }, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return audits;
        }

        public async Task<BaseResponseJson> Create(AuditExternalAssigmentRecomendation data)
        {
            BaseResponseJson responseJson = new BaseResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = await conn.QueryFirstAsync<BaseResponseJson>("usp_Post_Assigned_ExternalRecomendation", new
                    {
                        ID_AUDIT_EXTERNAL_DATA_RECOMENDATION = data.ID_AUDIT_EXTERNAL_DATA_RECOMENDATION,
                        DUE_DATE = data.DUE_DATE,
                        CREATEDBY = data.CREATEDBY,
                        USERID = data.USERID
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
