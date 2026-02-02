using AutoMapper;
using BCrypt.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Domain.Entities;
using Redarbor.Test.Domain.Interfaces;

namespace Redarbor.Test.Application.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEmployeeCommandHandler> _logger;

        public CreateEmployeeCommandHandler(
            IEmployeeRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CreateEmployeeCommandHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating employee with username: {Username}", request.Username);

            var employee = new Employee(
                request.CompanyId,
                request.Email,
                BCrypt.Net.BCrypt.HashPassword(request.Password),
                request.PortalId,
                request.RoleId,
                request.StatusId,
                request.Username,
                request.Fax,
                request.Name,
                request.Telephone
            );

            await _repository.AddAsync(employee);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Employee created successfully with ID: {EmployeeId}", employee.Id);

            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}
