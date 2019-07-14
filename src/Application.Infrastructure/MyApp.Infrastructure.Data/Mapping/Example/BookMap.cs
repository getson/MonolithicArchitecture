using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Example.ProductAgg;

namespace MyApp.Infrastructure.Data.Mapping.Example
{
    class BookMap:MyAppEntityTypeConfiguration<Book>
    {
    }
}
