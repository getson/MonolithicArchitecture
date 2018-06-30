using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MyApp.Core.Interfaces.Plugin;

namespace MyApp.Services.Plugins
{
    /// <summary>
    /// Represents a service for uploading application extensions (plugins or themes)
    /// </summary>
    public interface IUploadService
    {
        #region Methods

        /// <summary>
        /// Upload plugins and/or themes
        /// </summary>
        /// <param name="archivefile">Archive file</param>
        /// <returns>List of uploaded items descriptor</returns>
        IList<IDescriptor> UploadPluginsAndThemes(IFormFile archivefile);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the path to temp directory with uploads
        /// </summary>
        string UploadsTempPath { get; }

        /// <summary>
        /// Gets the name of the file containing information about the uploaded items
        /// </summary>
        string UploadedItemsFileName { get; }

        #endregion
    }
}