using App.Core;
using FluentValidation;
using System;

namespace App.Application.Commands.ProjectBC.Validators
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProject>
    {
        public UpdateProjectValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage(ErrorMessages.InvalidId);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

        }
    }
}
