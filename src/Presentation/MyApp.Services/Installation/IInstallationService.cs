namespace MyApp.Services.Installation
{
    /// <summary>
    /// Installation service
    /// </summary>
    public interface IInstallationService
    {
        /// <summary>
        /// Install data
        /// </summary>
        /// <param name="installSampleData">A value indicating whether to install sample data</param>
        void InstallData(bool installSampleData = true);
    }
}
