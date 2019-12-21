using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Messages.Validation;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi.Validations
{
    public class FluentValidationProvider : IValidationProvider
    {
        private static ValidationResponse BuildValidationResponse(ValidationResult validationResult)
        {
            return new ValidationResponse
            {
                Errors = validationResult.Errors.Select(failure => new ValidationError
                {
                    PropertyName = failure.PropertyName,
                    ErrorMessage = failure.ErrorMessage
                }).ToList()
            };
        }

        public async Task<ValidationResponse> ValidateAsync<TCommandResult>(ICommand<TCommandResult> command)
        {

            var validator = EngineContext.Current.Resolve(command, typeof(IValidator<>));
            
            var validateMethod = validator.GetType().GetMethod("ValidateAsync", new[] { command.GetType(), typeof(CancellationToken) });
            var validationResult = await (Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { command, default(CancellationToken) });

            return BuildValidationResponse(validationResult);
        }
    }
}
