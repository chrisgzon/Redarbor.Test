using FluentValidation;

namespace Redarbor.Test.Application.Commands.CreateEmployee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0)
                .WithMessage("CompanyId is required and must be greater than 0");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email must be a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");

            RuleFor(x => x.PortalId)
                .GreaterThan(0)
                .WithMessage("PortalId is required and must be greater than 0");

            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .WithMessage("RoleId is required and must be greater than 0");

            RuleFor(x => x.StatusId)
                .GreaterThan(0)
                .WithMessage("StatusId is required and must be greater than 0");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required");
        }
    }
}
