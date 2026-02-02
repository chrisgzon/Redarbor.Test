using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Redarbor.Test.Domain.Entities;
using Redarbor.Test.Infrastructure.Persistence.Write;
using Redarbor.Test.Infrastructure.Persistence.Write.Repositories;

namespace IntegrationTests.Infrastructure
{
    public class EmployeeRepositoryIntegrationTests : IDisposable
    {
        private readonly RedarborEFDbContext _context;
        private readonly EmployeeRepository _repository;

        public EmployeeRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<RedarborEFDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new RedarborEFDbContext(options);
            _repository = new EmployeeRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEmployee()
        {
            // Arrange
            var employee = new Employee(
                companyId: 1,
                email: "test@test.com",
                password: "password",
                portalId: 1,
                roleId: 1,
                statusId: 1,
                username: "testuser",
                fax: null,
                name: null,
                telephone: null
            );

            // Act
            var result = await _repository.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);

            var saved = await _context.Employees.FindAsync(result.Id);
            saved.Should().NotBeNull();
            saved!.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmployee()
        {
            // Arrange
            var employee = new Employee(1, "test@test.com", "pass", 1, 1, 1, "user", null, null, null);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(employee.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Username.Should().Be("user");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEmployee()
        {
            // Arrange
            var employee = new Employee(1, "test@test.com", "pass", 1, 1, 1, "original", null, null, null);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            employee.Update(username: "updated");
            await _repository.UpdateAsync(employee);
            await _context.SaveChangesAsync();

            // Assert
            var updated = await _context.Employees.FindAsync(employee.Id);
            updated!.Username.Should().Be("updated");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEmployee()
        {
            // Arrange
            var employee = new Employee(1, "test@test.com", "pass", 1, 1, 1, "user", null, null, null);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            employee.MarkAsDeleted();
            await _repository.UpdateAsync(employee);
            await _context.SaveChangesAsync();

            // Assert
            Employee? deleted = await _repository.GetByIdAsync(employee.Id);
            deleted?.DeletedOn.Should().NotBeNull();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
