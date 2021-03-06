﻿using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Core;
using System.Threading.Tasks;
using System.Threading;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Messages.Validation;
using FluentValidation.Results;
using System.Linq;
using FluentValidation;

namespace BinaryOrigin.SeedWork.WebApi.Validations
{
    public class FluentValidationProvider : ICommandValidationProvider
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
            if (validator == null)
            {
                // No validator found!
                return new ValidationResponse();
            }
            var validateMethod = validator.GetType().GetMethod("ValidateAsync", new[] { command.GetType(), typeof(CancellationToken) });
            var validationResult = await (Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { command, default(CancellationToken) });

            return BuildValidationResponse(validationResult);
        }
    }

}
