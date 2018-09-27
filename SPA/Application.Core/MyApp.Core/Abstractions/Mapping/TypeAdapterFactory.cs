using MyApp.Core.Infrastructure;

namespace MyApp.Core.Abstractions.Mapping
{
    public class TypeAdapterFactory
    {
        #region Members
        private readonly ITypeAdapterFactory _currentTypeAdapterFactory;
        #endregion

        #region Public Static Methods

        public static readonly TypeAdapterFactory Instance;

        static TypeAdapterFactory()
        {
            Instance = new TypeAdapterFactory();
        }
        private TypeAdapterFactory()
        {
            _currentTypeAdapterFactory = EngineContext.Current.Resolve<ITypeAdapterFactory>();
        }
        /// <summary>
        /// Create a new type adapter from currect factory
        /// </summary>
        /// <returns>Created type adapter</returns>
        public ITypeAdapter CreateAdapter()
        {
            return _currentTypeAdapterFactory.Create();
        }
        #endregion
    }
}
