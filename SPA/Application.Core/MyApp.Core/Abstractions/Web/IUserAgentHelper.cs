namespace MyApp.Core.Abstractions.Web
{
    /// <summary>
    /// User agent helper interface
    /// </summary>
    public interface IUserAgentHelper
    {

        /// <summary>
        /// Get a value indicating whether the request is made by mobile device
        /// </summary>
        /// <returns></returns>
        bool IsMobileDevice();

        /// <summary>
        /// Get a value indicating whether the request is made by IE8 browser
        /// </summary>
        /// <returns></returns>
        bool IsIe8();
    }
}