using System;
using System.Collections.Generic;
using System.Linq;
using MyApp.Core.Infrastructure;

namespace MyApp.Core.Plugins
{
    /// <summary>
    /// Plugin extensions
    /// </summary>
    public static class PluginExtensions
    {
        private static readonly List<string> SupportedLogoImageExtensions = new List<string>
        {
            "jpg",
            "png",
            "gif"
        };

        /// <summary>
        /// Get logo URL
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor</param>
        /// <param name="webHelper">Web helper</param>
        /// <returns>Logo URL</returns>
        public static string GetLogoUrl(this PluginDescriptor pluginDescriptor, IWebHelper webHelper)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            if (webHelper == null)
                throw new ArgumentNullException(nameof(webHelper));

            var fileProvider = EngineContext.Current.Resolve<IMyAppFileProvider>();

            var pluginDirectory = fileProvider.GetDirectoryName(pluginDescriptor.OriginalAssemblyFile);

            if (string.IsNullOrEmpty(pluginDirectory))
            {
                return null;
            }

            var logoExtension = SupportedLogoImageExtensions.FirstOrDefault(ext => fileProvider.FileExists(fileProvider.Combine(pluginDirectory, "logo." + ext)));

            if (string.IsNullOrWhiteSpace(logoExtension))
                return null; //No logo file was found with any of the supported extensions.

            var logoUrl = $"{webHelper.GetTenantLocation()}plugins/{fileProvider.GetDirectoryNameOnly(pluginDirectory)}/logo.{logoExtension}";
            return logoUrl;
        }
    }
}
