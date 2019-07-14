using System;
using System.Threading;
using MyApp.Domain.Logging;
using MyApp.Domain.User;

namespace MyApp.Services.Logging
{
    /// <summary>
    /// Logging extensions
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">user</param>
        public static void Debug(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception,user);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">user</param>
        public static void Information(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception,user);
        }
        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">user</param>
        public static void Warning(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception,user);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">user</param>
        public static void Error(this ILogger logger, string message, Exception exception = null,User user=null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception,user);
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">user</param>
        public static void Fatal(this ILogger logger, string message, Exception exception = null,User user=null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception,user);
        }

        /// <summary>
        /// Add a log records
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="level">Level</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">user</param>
        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null,User user=null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (logger.IsEnabled(level))
            {
                var fullMessage = exception?.ToString() ?? string.Empty;
                logger.InsertLog(level, message, fullMessage,user);
            }
        }
    }
}
