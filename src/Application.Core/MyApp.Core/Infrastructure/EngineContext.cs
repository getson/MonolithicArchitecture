using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MyApp.Core.Abstractions.Infrastructure;

namespace MyApp.Core.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the MyApp engine.
    /// </summary>
    public class EngineContext
    {
        private readonly IEngine _installedEngine;

        [SuppressMessage("NDepend", "ND1901:AvoidNonReadOnlyStaticFields", Justification = "It's not needed")]
        private static EngineContext _context;

        /// <summary>
        /// Gets the singleton MyApp engine used to access MyApp services.
        /// </summary>
        public static IEngine Current => Create();

        private EngineContext(IEngine installedEngine)
        {
            _installedEngine = installedEngine;
        }
        /// <summary>
        /// Create a static instance of the MyApp engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            if (_context == null)
            {
                _context = new EngineContext(new MyAppEngine());
            }
            return _context._installedEngine;
        }

        public static void SetEngine(IEngine engine)
        {
            _context=new EngineContext(engine);
        }
    }
}
