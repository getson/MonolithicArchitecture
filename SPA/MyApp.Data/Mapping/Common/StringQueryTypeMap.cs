using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Common;

namespace MyApp.Data.Mapping.Common
{
    /// <summary>
    /// Represents a string query type mapping configuration
    /// </summary>
    public partial class StringQueryTypeMap : MyAppQueryTypeConfiguration<StringQueryType>
    {
    }
}