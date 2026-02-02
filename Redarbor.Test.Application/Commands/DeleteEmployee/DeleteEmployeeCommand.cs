using MediatR;

namespace Redarbor.Test.Application.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
