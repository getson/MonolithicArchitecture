using FluentValidation;
using System;

namespace App.Application.Commands.ProjectBC.Validators
{
    public class DeleteProjectValidator : AbstractValidator<DeleteProject>
    {
        public DeleteProjectValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }
}
