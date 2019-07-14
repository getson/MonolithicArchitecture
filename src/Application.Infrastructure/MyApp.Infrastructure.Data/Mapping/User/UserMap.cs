using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApp.Infrastructure.Data.Mapping.User
{
    public class UserMap : MyAppEntityTypeConfiguration<Domain.User.User>
    {
        public override void Configure(EntityTypeBuilder<Domain.User.User> builder)
        {
            builder.Property(x => x.UserName).HasMaxLength(200);
            base.Configure(builder);
        }
    }
}
