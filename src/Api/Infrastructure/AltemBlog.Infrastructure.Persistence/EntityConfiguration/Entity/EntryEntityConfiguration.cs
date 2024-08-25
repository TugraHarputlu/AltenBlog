using AltemBlog.Infrastructure.Persistence.Context;
using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AltemBlog.Infrastructure.Persistence.EntityConfiguration.Entity
{
    internal class EntryEntityConfiguration : BaseEntityConfiguration<Entry>
    {
        public override void Configure(EntityTypeBuilder<Entry> builder)
        {
            base.Configure(builder);
            builder.ToTable("entreis", AltenBlogContext.DefaultSchema);

            builder.HasOne(i => i.CreatedUser)
                .WithMany(i => i.Entries)
                .HasForeignKey(i => i.CreatedById);
        }
    }
}
