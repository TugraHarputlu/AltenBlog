using AltemBlog.Infrastructure.Persistence.Context;
using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AltemBlog.Infrastructure.Persistence.EntityConfiguration
{
    public class UserEntityConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.ToTable("users",AltenBlogContext.DefaultSchema);
        }
    }
}
