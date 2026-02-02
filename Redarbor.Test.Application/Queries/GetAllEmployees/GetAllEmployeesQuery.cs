using MediatR;
using Redarbor.Test.Application.DTOs;

namespace Redarbor.Test.Application.Queries.GetAllEmployees
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}
