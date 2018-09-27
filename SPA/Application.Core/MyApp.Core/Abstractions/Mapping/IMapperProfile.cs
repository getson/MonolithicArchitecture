namespace MyApp.Core.Abstractions.Mapping
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public interface IMapperProfile
    {
        /// <summary>
        /// Gets order of this configuration implementation
        /// </summary>
        int Order { get; }
    }
}
