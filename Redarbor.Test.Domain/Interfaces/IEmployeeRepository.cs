using Redarbor.Test.Domain.Entities;

namespace Redarbor.Test.Domain.Interfaces
{
    /// <summary>
    /// Repository for write operations using Entity Framework
    /// </summary>
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
    }
}
