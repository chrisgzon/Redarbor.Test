using Redarbor.Test.Domain.Entities;

namespace Redarbor.Test.Domain.Interfaces
{
    /// <summary>
    /// Repository for read operations using Dapper
    /// </summary>
    public interface IEmployeeQueryRepository
    {
        Task<Employee?> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByUsernameAsync(string username);
    }
}
