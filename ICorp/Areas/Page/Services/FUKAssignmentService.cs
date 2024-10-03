using Dapper;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    public class FUKAssignmentService : IFUKAssignmentService
    {
        private readonly ConnectionDB _connectionDB;

        public FUKAssignmentService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public List<FUKAssignment> GetList(int parameterId)
        {
            List<FUKAssignment> fukAssigmentList = new List<FUKAssignment>();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@parameterId", parameterId);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    fukAssigmentList = (List<FUKAssignment>)conn.Query<FUKAssignment>("usp_Get_List_FUK_Assignment", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return fukAssigmentList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return fukAssigmentList;
            }
        }

        public ResponseJson SetAssignment(FUKAssignment fUKAssignment)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Set_FUK_Assignment", new
                    {
                        fukId = fUKAssignment.Id,
                        userId = fUKAssignment.PIC,
                        duedate = fUKAssignment.DueDate,
                        username = fUKAssignment.CreatedBy
                    }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = "0";

                return responseJson;
            }
        }
    }
}
