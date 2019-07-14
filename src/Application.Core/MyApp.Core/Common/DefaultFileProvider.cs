using MyApp.Core.Abstractions.Infrastructure;

namespace MyApp.Core.Common
{
    public static class DefaultFileProvider
    {
        /// <summary>
        /// the default instance that implements IMyAppFileProvider
        /// </summary>
        public static IMyAppFileProvider Instance { get; set; }
    }
}
