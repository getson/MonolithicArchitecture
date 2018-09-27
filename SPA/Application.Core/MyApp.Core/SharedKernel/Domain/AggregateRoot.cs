using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Core.SharedKernel.Domain
{
    public abstract class AggregateRoot : BaseEntity, IValidatableObject
    {
        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
