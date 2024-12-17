using InventoryIT.Areas.Master.Interface;
using InventoryIT.Areas.Master.Models;
using InventoryIT.Data;
using System.Data;
using Dapper;

namespace InventoryIT.Areas.Master.Service
{
    public class YearService : IYearService
    {
        private readonly ConnectionDB _connectionDB;

        public YearService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }
        public List<YearData> GetAll()
        {
            List<YearData> result = new List<YearData>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM tbl_M_YearData";
                    result = (List<YearData>) conn.Query<YearData>(sql);

                    return result;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return result;
                }
            }
        }
    }
}
