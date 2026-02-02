using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Domain.Entities;
using Redarbor.Test.Domain.Interfaces;

namespace Redarbor.Test.Application.Queries.GetEmployeeByIdQuery
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
    {
        private readonly IEmployeeQueryRepository _queryRepository;
        private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(
            IEmployeeQueryRepository queryRepository,
            ILogger<GetEmployeeByIdQueryHandler> logger,
            IMapper mapper)
        {
            _queryRepository = queryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting employee with ID: {EmployeeId}", request.Id);

            Employee? employeeEntitie = await _queryRepository.GetByIdAsync(request.Id);
            return _mapper.Map<EmployeeDto>(employeeEntitie);
        }
    }
}
