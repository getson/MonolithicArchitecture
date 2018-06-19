using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Common;

namespace MyApp.Data.Mapping.Common
{
    /// <summary>
    /// Represents an int query type mapping configuration
    /// </summary>
    public partial class IntQueryTypeMap : MyAppQueryTypeConfiguration<IntQueryType>
    {
    }
}