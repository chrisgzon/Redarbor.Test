using MediatR;
using Microsoft.Extensions.Logging;
using Redarbor.Test.Domain.Exceptions;
using Redarbor.Test.Domain.Interfaces;

namespace Redarbor.Test.Application.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

        public DeleteEmployeeCommandHandler(
            IEmployeeRepository repository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteEmployeeCommandHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", request.Id);

            var employee = await _repository.GetByIdAsync(request.Id);
            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", request.Id);
                throw new EmployeeNotFoundException(request.Id);
            }

            employee.MarkAsDeleted();
            await _repository.UpdateAsync(employee);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Employee mark as deleted successfully with ID: {EmployeeId}", request.Id);

            return Unit.Value;
        }
    }
}
