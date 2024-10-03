using Dapper;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Data;
using System.Data;

namespace PlanCorp.Areas.Master.Service
{
    public class FungsiService : IFungsiService
    {
        private readonly ConnectionDB _connectionDB;

        public FungsiService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }

        public List<Fungsi> GetAll()
        {
            List<Fungsi> result = new List<Fungsi>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM tbl_M_Fungsi";
                    result = (List<Fungsi>)conn.Query<Fungsi>(sql);
                    conn.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return result;
                }
            }
        }

        public List<Fungsi> Gets()
        {
            List<Fungsi> result = new List<Fungsi>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT A.ID AS ID_FUNGSI, CONCAT(A.[nama], ' - ', B.[nama]) AS FUNGSINAME FROM[dbo].[tbl_M_RefFungsi] A JOIN[dbo].[tbl_M_RefDirektorat] B ON A.[id_direktorat] = B.[ID]";
                    result = (List<Fungsi>)conn.Query<Fungsi>(sql);
                    conn.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return result;
                }
            }
        }

        public Fungsi Get(string id)
        {
            List<Fungsi> result = new List<Fungsi>();
            Fungsi fungsi = new Fungsi();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                try
                {
                    conn.Open();
                    string sql = @" SELECT A.ID AS ID_FUNGSI, CONCAT(A.[nama], ' - ', B.[nama]) AS FUNGSINAME, B.nama AS DIRECTORATENAME
                              FROM [dbo].[tbl_M_RefFungsi] A
                              JOIN [dbo].[tbl_M_RefDirektorat] B ON A.[id_direktorat] = B.[ID]
                              WHERE A.ID = " + id.ToString();
                    result = (List<Fungsi>)conn.Query<Fungsi>(sql);
                    conn.Close();

                    if(result.Count > 0)
                        fungsi = (Fungsi)result[0];

                    return fungsi;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return fungsi;
                }
            }
        }
    }
}
