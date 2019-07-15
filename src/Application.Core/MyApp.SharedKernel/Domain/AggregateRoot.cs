﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyApp.SharedKernel.Domain
{
    public abstract class AggregateRoot : BaseEntity, IValidatableObject
    {
        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

        public virtual void GenerateNewIdentity()
        {
            if (IsTransient())
                Id = Guid.NewGuid();
        }
    }
}
