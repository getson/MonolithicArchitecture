using System.Runtime.CompilerServices;
using MyApp.Core.Common;
using MyApp.Core.Interfaces.Infrastructure;

namespace MyApp.Core.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the MyApp engine.
    /// </summary>
    public class EngineContext
    {
        #region Methods

        /// <summary>
        /// Create a static instance of the MyApp engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            //create MyAppEngine as engine
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new MyAppEngine());
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton MyApp engine used to access MyApp services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
