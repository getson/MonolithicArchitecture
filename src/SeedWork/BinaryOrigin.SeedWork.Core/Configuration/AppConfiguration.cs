namespace BinaryOrigin.SeedWork.Core.Configuration
{
    /// <summary>
    /// Represents startup App configuration parameters
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// put default values
        /// </summary>
        public string DbConnectionString { get; set; }

        public string Environment { get; set; }
    }
}