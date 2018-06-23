using MyApp.Core.Domain.Localization;
using MyApp.Core.Domain.User;

namespace MyApp.Core.Interfaces.Web
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }
        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }

        User CurrentUser { get; set; }
    }
}
