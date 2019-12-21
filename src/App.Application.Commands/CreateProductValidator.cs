using App.Application.Commands.ProjectBC;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Commands
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
