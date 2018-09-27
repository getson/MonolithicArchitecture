using System.Runtime.Serialization;

namespace MyApp.Core.Configuration
{
   
    /// <summary>
    /// Represents data provider type enumeration
    /// </summary>
    public enum DataProviderType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "")]
        Unknown,

        /// <summary>
        /// MS SQL Server
        /// </summary>
        [EnumMember(Value = "sqlserver")]
        SqlServer,

        /// <summary>
        /// Oracle
        /// </summary>
        [EnumMember(Value = "oracle")]
        Oracle
    }
}