using FluentValidation;

namespace App.Application.Commands.ProjectBC.Validators
{
    public class CreateProjectValidator : AbstractValidator<AddProject>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}