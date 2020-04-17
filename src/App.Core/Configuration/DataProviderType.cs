using System.Runtime.Serialization;

namespace App.Core
{
    public enum DataProviderType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "")]
        Unknown,

        /// <summary>
        /// PostgreSql
        /// </summary>
        [EnumMember(Value = "InMemory")]
        InMemory,
        /// <summary>
        /// MS SQL Server
        /// </summary>
        [EnumMember(Value = "SqlServer")]
        SqlServer,

        /// <summary>
        /// Oracle
        /// </summary>
        [EnumMember(Value = "Oracle")]
        Oracle,

        /// <summary>
        /// MySql
        /// </summary>
        [EnumMember(Value = "MySql")]
        MySql,

        /// <summary>
        /// PostgreSql
        /// </summary>
        [EnumMember(Value = "PostgreSql")]
        PostgreSql
    }
}