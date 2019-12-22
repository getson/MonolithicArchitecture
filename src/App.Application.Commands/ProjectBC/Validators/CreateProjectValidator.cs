using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

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
    public class DeleteProductValidator : AbstractValidator<DeleteProject>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }
}
