using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyApp.SharedKernel.Validator
{
    /// <inheritdoc />
    /// <summary>
    /// Validator based on Data Annotations. 
    /// This validator use IMyAppbleObject interface and
    /// ValidationAttribute ( hierachy of this) for
    /// perform validation
    /// </summary>
    public class DataAnnotationsEntityValidator : IEntityValidator
    {
        #region Private Methods

        /// <summary>
        /// Get errors if object implement IMyAppbleObject
        /// </summary>
        /// <typeparam name="TEntity">The typeof entity</typeparam>
        /// <param name="item">The item to validate</param>
        /// <param name="errors">A collection of current errors</param>
        private void SetMyAppbleObjectErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            if (typeof(IValidatableObject).IsAssignableFrom(typeof(TEntity)))
            {
                var validationContext = new ValidationContext(item, null, null);

                var validationResults = ((IValidatableObject)item).Validate(validationContext);

                errors.AddRange(validationResults.Select(vr => vr.ErrorMessage));
            }
        }

        /// <summary>
        /// Get errors on ValidationAttribute
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <param name="item">The entity to validate</param>
        /// <param name="errors">A collection of current errors</param>
        private void SetValidationAttributeErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            var result = (from property in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                          from attribute in property.Attributes.OfType<ValidationAttribute>()
                          where !attribute.IsValid(property.GetValue(item))
                          select attribute.FormatErrorMessage(string.Empty)).ToArray();
            if (result.Any())
            {
                errors.AddRange(result);
            }
        }

        #endregion

        #region IEntityValidator Members


        public bool IsValid<TEntity>(TEntity item) where TEntity : class
        {

            if (item == null)
                return false;

            var validationErrors = new List<string>();

            SetMyAppbleObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);

            return !validationErrors.Any();
        }

        public IEnumerable<string> GetInvalidMessages<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                return new List<string>();
            }

            var validationErrors = new List<string>();

            SetMyAppbleObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);


            return validationErrors;
        }

        #endregion
    }
}
