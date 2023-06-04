using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeesCRUD.Model
{
    public class DapperDb
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperDb(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
