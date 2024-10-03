using Dapper;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    public class DokumenUPService : IDokumenUPService
    {
        private readonly ConnectionDB _connectionDB;

        public DokumenUPService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public List<DokumenList> GetList(string userName)
        {
            List<DokumenList> dokumens = new List<DokumenList>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    dokumens = (List<DokumenList>)conn.Query<DokumenList>("usp_Get_List_Document", new { userName  = userName }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return dokumens;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return dokumens;
            }
        }

        //public List<DokumenUP> GetList()
        //{
        //    List<DokumenUP> dokumens = new List<DokumenUP>();
        //    try
        //    {
        //        using (IDbConnection conn = _connectionDB.Connection)
        //        {
        //            conn.Open();
        //            dokumens = (List<DokumenUP>)conn.Query<DokumenUP>("usp_Get_Template_MKK", commandType: CommandType.StoredProcedure);
        //            conn.Close();

        //            return dokumens;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        return dokumens;
        //    }
        //}

        public bool UpdateUpload(UploadFile uploadFile)
        {
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    var query = @"INSERT INTO tbl_T_Upload_Document(FILENAME, FILEEXT, FILESIZE, FILEDATA) VALUES (@FileName, @FileType, @FileSize, @FileData)";
                    var respo = conn.Execute(query, uploadFile);
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

        public bool TestUpload(UploadFile uploadFile)
        {
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    var query = @"INSERT INTO tbl_T_Upload_Document(FILENAME, FILEEXT, FILESIZE, FILEDATA) VALUES (@FileName, @FileType, @FileSize, @FileData)";
                    var respo = conn.Execute(query, uploadFile);
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

        public List<DokumenUP> GetDetailList(string userName, string year)
        {
            List<DokumenUP> dokumens = new List<DokumenUP>();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    dokumens = (List<DokumenUP>)conn.Query<DokumenUP>("usp_Get_Detail_Document", new { userName = userName, yearData = year }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return dokumens;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return dokumens;
            }
        }

        public ResponseJson UploadDocumentUP(DokumenUpload dokumenUpload)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IDUP", dokumenUpload.UpID);
                parameters.Add("@REMARKS", dokumenUpload.Remarks);
                parameters.Add("@FILENAME", dokumenUpload.FileName);
                parameters.Add("@FILEEXT", dokumenUpload.FileType);
                parameters.Add("@FILEDATA", dokumenUpload.FileData);
                parameters.Add("@username", dokumenUpload.userName);
                parameters.Add("@FilePath", dokumenUpload.FilePath);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    //var query = @"INSERT INTO tbl_T_Upload_Document(FILENAME, FILEEXT, FILESIZE, FILEDATA) VALUES (@FileName, @FileType, @FileSize, @FileData)";
                    //var respo = conn.Execute(query, uploadFile);
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Upload_Document_UP", parameters, commandType: CommandType.StoredProcedure);
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
