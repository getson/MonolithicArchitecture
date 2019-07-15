
using MyApp.SharedKernel.Domain;

namespace MyApp.Core.Abstractions.Web
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// Gets or sets value indicating whether we're in admin area
        bool IsAdmin { get; set; }

        User CurrentUser { get; set; }
    }
}
