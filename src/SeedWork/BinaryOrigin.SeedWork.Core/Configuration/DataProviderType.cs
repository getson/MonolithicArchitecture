using System.Runtime.Serialization;

namespace BinaryOrigin.SeedWork.Core.Configuration
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
        /// MS My Sql
        /// </summary>
        [EnumMember(Value = "sqlserver")]
        SqlServer,

        /// <summary>
        /// MS My Sql
        /// </summary>
        [EnumMember(Value = "mysql")]
        MySql,

        /// <summary>
        /// Oracle
        /// </summary>
        [EnumMember(Value = "oracle")]
        Oracle
    }
}