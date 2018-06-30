using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApp.Infrastructure.Data.Mapping.User
{
    public class UserMap : MyAppEntityTypeConfiguration<Core.Domain.User.User>
    {
        public override void Configure(EntityTypeBuilder<Core.Domain.User.User> builder)
        {
            builder.ToTable(typeof(Core.Domain.User.User).Name);
            builder.Property(x => x.UserName).HasMaxLength(200);
            base.Configure(builder);
        }
    }
}
