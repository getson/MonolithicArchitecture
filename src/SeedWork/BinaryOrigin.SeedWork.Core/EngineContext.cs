using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BinaryOrigin.SeedWork.Core
{
    /// <summary>
    /// Provides access to the singleton instance of the App engine.
    /// </summary>
    public class EngineContext
    {
        [SuppressMessage("NDepend", "ND1901:AvoidNonReadOnlyStaticFields", Justification = "It's not needed")]
        private static EngineContext _context;

        private readonly IEngine _installedEngine;

        private EngineContext(IEngine installedEngine)
        {
            _installedEngine = installedEngine;
        }

        private EngineContext()
        {
        }

        /// <summary>
        /// Gets the singleton App engine used to access App services.
        /// </summary>
        public static IEngine Current => Create<IEngine>();

        /// <summary>
        /// Create a static instance of the App engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create<T>()
        where T : IEngine
        {
            if (_context == null)
            {
                var engine = Activator.CreateInstance<T>();
                _context = new EngineContext(engine);
            }
            return (T)_context._installedEngine;
        }
    }
}