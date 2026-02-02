using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Redarbor.Test.Application.Commands.CreateEmployee;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Domain.Entities;
using Redarbor.Test.Domain.Interfaces;

namespace Redarbor.Test.UnitTests.Commands
{
    public class CreateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateEmployeeCommandHandler>> _loggerMock;
        private readonly CreateEmployeeCommandHandler _handler;

        public CreateEmployeeCommandHandlerTests()
        {
            _repositoryMock = new Mock<IEmployeeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateEmployeeCommandHandler>>();

            _handler = new CreateEmployeeCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateEmployee()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            {
                CompanyId = 1,
                Email = "test@test.com",
                Password = "password123",
                PortalId = 1,
                RoleId = 1,
                StatusId = 1,
                Username = "testuser"
            };

            var expectedDto = new EmployeeDto
            {
                Id = 1,
                CompanyId = 1,
                Email = "test@test.com",
                Username = "testuser"
            };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Employee>()))
                .ReturnsAsync((Employee e) => e);

            _mapperMock
                .Setup(m => m.Map<EmployeeDto>(It.IsAny<Employee>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Employee_Constructor_WithInvalidEmail_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () => new Employee(1, "", "password", 1, 1, 1, "username", null, null, null);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Email is required*");
        }
    }
}
