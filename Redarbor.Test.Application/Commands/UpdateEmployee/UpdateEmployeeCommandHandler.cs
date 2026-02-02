using MediatR;
using Microsoft.Extensions.Logging;
using Redarbor.Test.Domain.Exceptions;
using Redarbor.Test.Domain.Interfaces;

namespace Redarbor.Test.Application.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

        public UpdateEmployeeCommandHandler(
            IEmployeeRepository repository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateEmployeeCommandHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating employee with ID: {EmployeeId}", request.Id);

            var employee = await _repository.GetByIdAsync(request.Id);
            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", request.Id);
                throw new EmployeeNotFoundException(request.Id);
            }

            employee.Update(
                companyId: request.CompanyId,
                email: request.Email,
                password: BCrypt.Net.BCrypt.HashPassword(request.Password),
                portalId: request.PortalId,
                roleId: request.RoleId,
                statusId: request.StatusId,
                username: request.Username,
                fax: request.Fax,
                name: request.Name,
                telephone: request.Telephone
            );

            await _repository.UpdateAsync(employee);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Employee updated successfully with ID: {EmployeeId}", request.Id);

            return Unit.Value;
        }
    }
}
