using System.Data;
using System.Data.SqlClient;

namespace PlanCorp_API.Helper
{
    public class ConnectionDB
    {
        private readonly IConfiguration _configuration;
        public ConnectionDB(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("PlanCorpDbContextConnection");
            providerName = "System.Data.SqlClient";
        }

        public string ConnectionString { get; }
        public string providerName { get; }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }
    }
}
