using Dapper;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    public class FollowUpService : IFollowUpService
    {
        private readonly ConnectionDB _connectionDB;

        public FollowUpService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public List<FollowUpDetail> GetDetailList(string year)
        {
            List<FollowUpDetail> list = new List<FollowUpDetail>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    list = (List<FollowUpDetail>)conn.Query<FollowUpDetail>("usp_Get_Detail_Follow_Up", new { year = year }, commandType: CommandType.StoredProcedure);
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

        public List<FollowUpList> GetList(string userName)
        {
            List<FollowUpList> list = new List<FollowUpList>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    list = (List<FollowUpList>)conn.Query<FollowUpList>("usp_Get_Follow_Up_List", new { userName = userName }, commandType: CommandType.StoredProcedure);
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

        public ResponseJson FollowUPDocument(FollowUpUpload followUpUpload)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IDUP", followUpUpload.UpID);
                parameters.Add("@REMARKS", followUpUpload.Remarks);
                parameters.Add("@FILENAME", followUpUpload.FileName);
                parameters.Add("@FILEEXT", followUpUpload.FileType);
                parameters.Add("@FILEDATA", followUpUpload.FileData);
                parameters.Add("@username", followUpUpload.userName);
                parameters.Add("@FILEPATH", followUpUpload.FilePath);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_FollowUp_Auditee", parameters, commandType: CommandType.StoredProcedure);
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
    }
}
