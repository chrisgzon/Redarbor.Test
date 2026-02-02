using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Redarbor.Test.Infrastructure.Persistence.Read
{
    public class RedarborDapperContext
    {
        private readonly string _connectionString;

        public RedarborDapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RedarborConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
