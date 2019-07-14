namespace MyApp.Services.Installation
{
    /// <summary>
    /// Represents default values related to installation services
    /// </summary>
    public static class InstallationDefaults
    {

        /// <summary>
        /// Gets a path to the installation required data file
        /// </summary>
        public static string RequiredDataPath => "~/App_Data/Install/Fast/create_required_data.sql";

        /// <summary>
        /// Gets a path to the installation sample data file
        /// </summary>
        public static string SampleDataPath => "~/App_Data/Install/Fast/create_sample_data.sql";


    }
}