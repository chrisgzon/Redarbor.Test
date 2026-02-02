using MediatR;
using Microsoft.Extensions.Logging;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Domain.Entities;
using Redarbor.Test.Domain.Interfaces;
using AutoMapper;

namespace Redarbor.Test.Application.Queries.GetAllEmployees
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IEmployeeQueryRepository _queryRepository;
        private readonly ILogger<GetAllEmployeesQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(
            IEmployeeQueryRepository queryRepository,
            ILogger<GetAllEmployeesQueryHandler> logger,
            IMapper mapper)
        {
            _queryRepository = queryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all employees");

            IEnumerable<Employee> employeesEntitie = await _queryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employeesEntitie);
        }
    }
}
