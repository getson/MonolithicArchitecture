namespace MyApp.SharedKernel.Validator
{
    /// <inheritdoc />
    /// <summary>
    /// Data Annotations based entity validator factory
    /// </summary>
    public class DataAnnotationsEntityValidatorFactory : IEntityValidatorFactory
    {
        /// <inheritdoc />
        /// <summary>
        /// Create a entity validator
        /// </summary>
        /// <returns></returns>
        public IEntityValidator Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
