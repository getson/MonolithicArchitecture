namespace MyApp.Infrastructure.Mapping.DTOs
{
    /// <summary>
    /// This is the data transfer object
    /// for country entity. The name
    /// of properties for this type
    /// is based on conventions of many mappers
    /// to simplificate the mapping process
    /// </summary>
    public class CountryDto
    {
        /// <summary>
        /// The country identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The country name
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// The country ISO Code
        /// </summary>
        public string CountryIsoCode { get; set; }
    }
}
