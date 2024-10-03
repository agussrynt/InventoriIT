using Dapper;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly ConnectionDB _connectionDB;

        public MonitoringService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public List<MonitoringList> GetList(string userName)
        {
            List<MonitoringList> list = new List<MonitoringList>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    list = (List<MonitoringList>)conn.Query<MonitoringList>("usp_Get_Monitoring_List", commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return list;
            }
        }

        public List<MonitoringDetail> GetDetailList(string year)
        {
            List<MonitoringDetail> list = new List<MonitoringDetail>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    list = (List<MonitoringDetail>)conn.Query<MonitoringDetail>("usp_Get_Detail_Follow_Up", new { year = year }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return list;
            }
        }

        //usp_SetNew_DueDate
        public ResponseJson SetNewDueDate(int idUP, DateTime newDate)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID_UP", idUP);
                parameters.Add("@NEW_DATE", newDate);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_SetNew_DueDate", parameters, commandType: CommandType.StoredProcedure);
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
