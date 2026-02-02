using FluentValidation;
using Redarbor.Test.Application.Commands.CreateEmployee;

namespace Redarbor.Test.Application.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id is required and must be greater than 0");
        }
    }
}
