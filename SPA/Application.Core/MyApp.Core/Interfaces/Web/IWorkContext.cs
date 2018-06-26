using MyApp.Core.Domain.User;

namespace MyApp.Core.Interfaces.Web
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }

        User CurrentUser { get; set; }
    }
}
