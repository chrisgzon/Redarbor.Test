using MediatR;
using Redarbor.Test.Application.DTOs;

namespace Redarbor.Test.Application.Queries.GetEmployeeByIdQuery
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto?>
    {
        public int Id { get; set; }
    }
}
