namespace MyApp.Mapping.DTOs
{
    /// <summary>
    /// This is the data transfer object
    /// for software entity. The name
    /// of properties for this type
    /// is based on conventions of many mappers
    /// to simplificate the mapping process
    /// </summary>
    public class SoftwareDto
    {
        /// <summary>
        /// Get or set the id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or set the long description for this product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or set the product title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the unit price for this product
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Get or set the stock items of this product
        /// </summary>
        public int AmountInStock { get; set; }

        /// <summary>
        /// Get or set the license code
        /// </summary>
        public string LicenseCode { get; set; }
    }
}
