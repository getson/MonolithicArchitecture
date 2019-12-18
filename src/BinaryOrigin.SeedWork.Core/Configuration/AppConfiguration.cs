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
        public string AdminDbConnectionString { get; set; }
        /// <summary>
        /// Enable request/response logging
        /// </summary>
        public bool EnableLogging { get; set; }
        public string Authority { get; set; }
        public bool RequireHttps { get; set; }
        public string ApiName { get; set; }
        public string SendGridApiKey { get; set; }
    }
}