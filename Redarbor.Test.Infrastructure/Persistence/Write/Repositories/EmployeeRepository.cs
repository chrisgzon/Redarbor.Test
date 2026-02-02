using Microsoft.EntityFrameworkCore;
using Redarbor.Test.Domain.Entities;
using Redarbor.Test.Domain.Interfaces;

namespace Redarbor.Test.Infrastructure.Persistence.Write.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly RedarborEFDbContext _context;

        public EmployeeRepository(RedarborEFDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.DeletedOn == null);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            return employee;
        }

        public Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            return Task.CompletedTask;
        }
    }
}
