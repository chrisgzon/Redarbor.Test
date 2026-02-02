using Dapper;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Domain.Entities;
using Redarbor.Test.Domain.Interfaces;
using System.Data;

namespace Redarbor.Test.Infrastructure.Persistence.Read.Repositories
{
    public class EmployeeQueryRepository : IEmployeeQueryRepository
    {
        private readonly IDbConnection _connection;

        public EmployeeQueryRepository(RedarborDapperContext context)
        {
            _connection = context.CreateConnection();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT 
                Id, CompanyId, CreatedOn, DeletedOn, Email, 
                Fax, Name, Lastlogin, Password, PortalId, 
                RoleId, StatusId, Telephone, UpdatedOn, Username
            FROM Employees 
            WHERE Id = @Id AND DeletedOn IS NULL";

            return await _connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            const string sql = @"
            SELECT 
                Id, CompanyId, CreatedOn, DeletedOn, Email, 
                Fax, Name, Lastlogin, Password, PortalId, 
                RoleId, StatusId, Telephone, UpdatedOn, Username
            FROM Employees 
            WHERE DeletedOn IS NULL
            ORDER BY CreatedOn DESC";

            return await _connection.QueryAsync<Employee>(sql);
        }

        public async Task<Employee?> GetByUsernameAsync(string username)
        {
            const string sql = @"
            SELECT 
                Id, CompanyId, CreatedOn, DeletedOn, Email, 
                Fax, Name, Lastlogin, Password, PortalId, 
                RoleId, StatusId, Telephone, UpdatedOn, Username
            FROM Employees 
            WHERE Username = @Username AND DeletedOn IS NULL";

            return await _connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Username = username });
        }
    }
}
